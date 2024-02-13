using System.Collections.Generic;
using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.DataPersistence;
using GruntzUnityverse.V2.Itemz;
using GruntzUnityverse.V2.Itemz.Toolz;
using GruntzUnityverse.V2.Objectz;
using GruntzUnityverse.V2.Pathfinding;
using GruntzUnityverse.V2.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace GruntzUnityverse.V2.Grunt {
/// <summary>
/// The class representing a Grunt in the game.
/// </summary>
public class GruntV2 : GridObject, IDataPersistence, IAnimatable {
	public UnityEvent onNodeChanged;

	// --------------------------------------------------
	// Statz
	// --------------------------------------------------

	#region Statz
	/// <summary>
	/// The name of this Grunt.
	/// </summary>
	[Header("Statz")]
	public string gruntName;

	/// <summary>
	/// The statz of this Grunt, such as health or stamina. 
	/// </summary>
	public Statz statz;

	/// <summary>
	/// The flagz representing the current state of this Grunt.
	/// </summary>
	public Flagz flagz;

	public bool IsInterrupted => flagz.interrupted;

	/// <summary>
	/// The attribute barz of this Grunt.
	/// </summary>
	public Barz barz;
	#endregion

	// --------------------------------------------------
	// Equipment
	// --------------------------------------------------

	#region Equipment
	/// <summary>
	/// The tool currently equipped by this Grunt.
	/// </summary>
	[Header("Equipment")]
	public EquippedTool equippedTool;

	// public EquippedToy equippedToy;

	/// <summary>
	/// The toy currently equipped by this Grunt.
	/// </summary>
	public Toy toy;

	/// <summary>
	/// The powerup that is currently active on this Grunt.
	/// </summary>
	public Powerup powerup;
	#endregion

	// --------------------------------------------------
	// Animation
	// --------------------------------------------------

	[Header("Animation")]
	public AnimationPackV2 animationPack;

	public GameObject selectionMarker;

	#region IAnimatable
	// --------------------------------------------------
	// IAnimatable
	// --------------------------------------------------
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	#endregion

	// --------------------------------------------------
	// Pathfinding
	// --------------------------------------------------

	[Header("Pathfinding")]
	public List<NodeV2> path;

	public NodeV2 targetNode;
	public NodeV2 next;

	// --------------------------------------------------
	// Events
	// --------------------------------------------------

	#region Events
	protected override void Start() {
		base.Start();

		Animancer.Play(animationPack.idle.down[0]);
	}

	private void Update() {
		if (flagz.moving) {
			ChangePosition();
		}
	}
	#endregion

	private void RegenerateStamina() {
		statz.stamina += statz.staminaRegenRate;
	}

	// --------------------------------------------------
	// Input Actions
	// --------------------------------------------------

	#region Input Actions
	// Left click
	private void OnSelect() {
		if (GM.Instance.selector.location2D == location2D) {
			flagz.selected = true;
			selectionMarker.SetActive(true);

			GM.Instance.selectedGruntz.UniqueAdd(this);
		} else {
			flagz.selected = false;
			selectionMarker.SetActive(false);

			GM.Instance.selectedGruntz.Remove(this);
		}
	}

	// Left click & Ctrl
	private void OnAdditionalSelect() {
		if (GM.Instance.selector.location2D != location2D) {
			return;
		}

		flagz.selected = !flagz.selected;
		selectionMarker.SetActive(!selectionMarker.activeSelf);

		if (flagz.selected) {
			GM.Instance.selectedGruntz.UniqueAdd(this);
		} else {
			GM.Instance.selectedGruntz.Remove(this);
		}
	}

	// Ctrl & A
	private void OnSelectAll() {
		flagz.selected = true;
		selectionMarker.SetActive(true);

		GM.Instance.selectedGruntz.UniqueAdd(this);
	}

	// Right click
	private void OnMove() {
		// No need to check for IsInterrupted(),
		// since we need to be able to set another target while the Grunt is moving
		if (!flagz.selected) {
			return;
		}

		flagz.movingToAct = false;

		targetNode = LevelV2.Instance.levelNodes
			.First(n => n.location2D == GM.Instance.selector.location2D);

		Move();
	}

	// Left click & Shift
	private async void OnAction() {
		if (!flagz.selected || IsInterrupted) {
			return;
		}

		Debug.Log("OnAction");

		GridObject interactable =
			FindObjectsByType<GridObject>(FindObjectsSortMode.None)
				.FirstOrDefault(go => go.location2D == GM.Instance.selector.location2D && go is IInteractable);

		GruntV2 grunt = GM.Instance.allGruntz
			.FirstOrDefault(g => g.location2D == GM.Instance.selector.location2D);

		// There was nothing found to interact with
		if (interactable == null && grunt == null) {
			// Todo: Play voice line for being unable to interact with nothing
			flagz.movingToAct = false;

			return;
		}

		// There was an interactable object found
		if (interactable != null) {
			// Check whether the Grunt has an appropriate tool equipped
			if (!((IInteractable)interactable).CompatibleItemz.Contains(equippedTool.toolName)) {
				Debug.Log("Cannot interact with this! It's not compatible!");
				flagz.movingToAct = false;

				// Todo: Play voice line for having an incompatible tool
				return;
			}

			flagz.movingToAct = true;
			targetNode = interactable.node;

			Move();

			// await UniTask.WaitWhile(
			//   () => !node.neighbours.Contains(interactable.node),
			//   cancellationToken: waitCancellationToken.Token,
			//   cancelImmediately: true
			// );

			// Grunt is ready to act
			if (flagz.movingToAct) {
				Animancer.Play(animationPack.interact.down[0]);

				// Wait the duration of the interaction animation
				await UniTask.WaitForSeconds(0.5f);

				equippedTool.Use(interactable);
			}
		}
		// There was a Grunt found
		else if (grunt != null) {
			/*
			 * Todo: Take into account the following:
			 * - whether the target is friendly or not
			 * - the tool's reach
			 * - the Grunt's ability to reach the target
			 */

			// StartCoroutine(tool.Use(grunt));
		}
	}

	// Todo: Needs similar logic to OnAction
	// Left click & Alt
	private void OnGive() {
		if (!flagz.selected || IsInterrupted) {
			return;
		}

		if (toy == null) {
			// Todo: Play voice line for not having a toy

			return;
		}

		Debug.Log("OnGive");

		GruntV2 target = GM.Instance.allGruntz
			.FirstOrDefault(grunt => grunt.location2D == GM.Instance.selector.location2D);

		// Todo: Move beside target

		// Todo: Give toy to target
		Debug.Log($"Giving with {gruntName}");
	}
	#endregion

	// --------------------------------------------------
	// Movement
	// --------------------------------------------------

	#region Movement
	private void ChangePosition() {
		Vector3 moveVector = (next.transform.position - transform.position).normalized;
		gameObject.transform.position += moveVector * (Time.deltaTime / .6f);
	}

	/// <summary>
	/// Moves the Grunt to the current target node.
	/// </summary>
	public void Move() {
		#region Validate move
		if (flagz.moving) {
			return;
		}

		// Grunt's move is forced (e.g. by an Arrow)
		if (flagz.moveForced) {
			Debug.Log("Move is forced!");

			return;
		}

		// Grunt cannot move
		if (flagz.interrupted) {
			Debug.Log("I'm interrupted!");

			return;
		}

		// Grunt has reached his target (or is already there)
		if (targetNode == node || targetNode == null) {
			return;
		}
		#endregion

		List<NodeV2> newPath = Pathfinder.AstarSearch(node, targetNode, LevelV2.Instance.levelNodes.ToHashSet());

		if (newPath.Count <= 0) {
			Debug.Log("New path is zero");

			return;
		}

		next = newPath[0];
		flagz.moving = true;

		Vector2Int moveVector = (next.location2D - node.location2D);
		// FaceTowards(moveVector);
	}

	/// <summary>
	/// Forcibly moves the Grunt to the current target node.
	/// </summary>
	public void MoveToNode() {
		#region Validate move
		if (flagz.moving) {
			return;
		}

		// Grunt cannot move
		if (flagz.interrupted) {
			Debug.Log("I'm interrupted!");

			return;
		}

		// Grunt has reached his target (or is already there)
		if (targetNode == node || targetNode == null) {
			return;
		}
		#endregion

		flagz.moving = true;

		Vector2Int moveVector = (targetNode.location2D - node.location2D);
		// FaceTowards(moveVector);
	}
	#endregion

	public async void Act() {
		if (IsInterrupted) {
			flagz.movingToAct = false;

			return;
		}

		GruntV2 grunt = GM.Instance.allGruntz
			.FirstOrDefault(g => g.location2D == GM.Instance.selector.location2D);

		// There was nothing found to interact with
		if (grunt == null) {
			// Todo: Play voice line for being unable to interact with nothing
			flagz.movingToAct = false;

			return;
		}

		// There was a Grunt found

		flagz.movingToAct = true;
		targetNode = grunt.node;

		Move();

		// await UniTask.WaitWhile(
		//   () => !node.neighbours.Contains(grunt.node),
		//   cancellationToken: waitCancellationToken.Token,
		//   cancelImmediately: true
		// );

		Debug.Log("Acting");

		// if (!flagz.movingToAct) {
		//   return;
		// }

		if (statz.stamina == Statz.MaxValue) {
			Attack(grunt);
		}

		/*
		 * Todo: Take into account the following:
		 * - whether the target is friendly or not
		 * - the tool's reach
		 * - the Grunt's ability to reach the target
		 */
	}

	public async void Attack(GruntV2 target) {
		Debug.Log("Alright, we're doing this!");
		statz.stamina = 0;

		Animancer.Play(animationPack.attack.down[0]);

		await UniTask.WaitForSeconds(0.5f);

		equippedTool.Attack(target);
	}

	public void TakeDamage(int damage) {
		statz.health -= damage;

		if (statz.health <= 0) {
			// Die();
			Debug.Log("Im dead!");
		}
	}

	public void PlaceOnGround(NodeV2 placeNode) {
		// Todo: Go beside placeNode and place the Toy on it (if there is one equipped)
	}

	// --------------------------------------------------
	// IDataPersistence
	// --------------------------------------------------

	#region IDataPersistence
	public string Guid { get; set; }

	/// <summary>
	/// Saves the data to a GruntDataV2 object.
	/// </summary>
	/// <param name="data"></param>
	public void Save(ref GameData data) {
		GruntDataV2 saveData = new GruntDataV2 {
			guid = Guid,
			gruntName = gruntName,
			position = transform.position,
		};

		data.gruntData.CheckNotExistsAdd(saveData);

		Debug.Log($"Saving {gruntName} at {transform.position} with GUID {Guid}");
	}

	// ?Unnecessary
	public void Load(GameData data) {
		// GruntDataV2 loadData = data.gruntData.First(); // Remove the data from the list so it doesn't get loaded again
		//
		// Guid = loadData.guid;
		// gruntName = loadData.gruntName;
		// transform.position = loadData.position;
	}

	/// <summary>
	/// Loads the data from a GruntDataV2 object.
	/// </summary>
	/// <param name="data"></param>
	public void Load(GruntDataV2 data) {
		Guid = data.guid;
		gruntName = data.gruntName;
		// transform.position = data.position;
	}

	public void GenerateGuid() {
		Guid = System.Guid.NewGuid().ToString();
	}
	#endregion

}

// --------------------------------------------------
// Custom Editor
// --------------------------------------------------

#if UNITY_EDITOR
[CustomEditor(typeof(GruntV2))]
public class GruntV2Editor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		GruntV2 grunt = (GruntV2)target;

		GUILayout.Space(10);

		if (GUILayout.Button("Generate Guid")) {
			grunt.GenerateGuid();
		}

		GUILayout.Space(10);

		base.OnInspectorGUI();
	}
}
#endif
}
