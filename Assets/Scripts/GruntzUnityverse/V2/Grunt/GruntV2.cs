using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.DataPersistence;
using GruntzUnityverse.V2.Editor;
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
public class GruntV2 : MonoBehaviour, IDataPersistence, IAnimatable {
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

	public SpriteRenderer spriteRenderer;
	public CircleCollider2D circleCollider2D;
	public GameObject selectionMarker;

	// --------------------------------------------------
	// Animation
	// --------------------------------------------------

	#region Animation
	/// <summary>
	/// The direction this Grunt is facing.
	/// </summary>
	[Header("Animation")]
	public DirectionV2 facingDirection;

	/// <summary>
	/// The animation pack this Grunt uses.
	/// </summary>
	public AnimationPackV2 animationPack;
	#endregion

	// --------------------------------------------------
	// IAnimatable
	// --------------------------------------------------

	#region IAnimatable
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	#endregion

	// --------------------------------------------------
	// Pathfinding
	// --------------------------------------------------

	#region Pathfinding
	/// <summary>
	/// The location of this Grunt in 2D space.
	/// </summary>
	[Header("Pathfinding")]
	public Vector2 location2D;

	/// <summary>
	/// The node this Grunt is currently on.
	/// </summary>
	public NodeV2 node;

	/// <summary>
	/// The node the Grunt is moving towards.
	/// </summary>
	public NodeV2 targetNode;

	/// <summary>
	/// The next node the Grunt will move to.
	/// </summary>
	public NodeV2 next;
	#endregion

	// --------------------------------------------------
	// Interaction
	// --------------------------------------------------

	#region Interaction
	/// <summary>
	/// The target the Grunt will try to interact with.
	/// </summary>
	[Header("Interaction")]
	public GridObject interactionTarget;

	/// <summary>
	/// The target the Grunt will try to attack.
	/// </summary>
	public GruntV2 attackTarget;
	#endregion

	// --------------------------------------------------
	// Eventz
	// --------------------------------------------------

	#region Eventz
	[Header("Eventz")]
	public UnityEvent onStaminaDrained;

	public UnityEvent onDeath;

	[HideInNormalInspector]
	public UnityEvent onNodeChanged;

	[HideInNormalInspector]
	public UnityEvent onTargetReached;

	[HideInNormalInspector]
	public UnityEvent onHit;
	#endregion

	// --------------------------------------------------
	// Lifecycle Eventz
	// --------------------------------------------------

	#region Lifecycle Eventz
	protected virtual void Awake() {
		location2D = Vector2Int.RoundToInt(transform.position);
		spriteRenderer = GetComponent<SpriteRenderer>();
		circleCollider2D = GetComponent<CircleCollider2D>();
	}

	protected virtual void Start() {
		node = LevelV2.Instance.levelNodes.First(n => n.location2D == location2D);

		Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.idle));
	}

	private void Update() {
		if (attackTarget == null) {
			flagz.setToAttack = false;
		}

		if (interactionTarget == null) {
			flagz.setToInteract = false;
		}

		if (flagz.moving) {
			// Change position and play walk animation
			HandleMovement();
		} else if (flagz.hostileIdle) {
			// Play hostile idle animation
			Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.hostileIdle));
		} else if (!flagz.SetToAct) {
			// Play normal idle animation
			Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.idle));
		}
	}
	#endregion

	public async void RegenerateStamina() {
		while (statz.stamina < Statz.MaxValue) {
			statz.stamina++;
			barz.staminaBar.Adjust(statz.stamina);

			await UniTask.Delay(200);
		}

		statz.stamina = Statz.MaxValue;
	}

	private void Select() {
		flagz.selected = true;
		selectionMarker.SetActive(true);

		GM.Instance.selectedGruntz.UniqueAdd(this);
	}

	private void Deselect() {
		flagz.selected = false;
		selectionMarker.SetActive(false);

		GM.Instance.selectedGruntz.Remove(this);
	}

	// --------------------------------------------------
	// Input Actions
	// --------------------------------------------------

	#region Input Actions
	// Left click
	private void OnSelect() {
		if (GM.Instance.selector.location2D == node.location2D) {
			Select();
		} else {
			Deselect();
		}
	}

	// Left click & Ctrl
	private void OnAdditionalSelect() {
		if (GM.Instance.selector.location2D != location2D) {
			return;
		}

		// When the Grunt is already selected, we want to deselect it if it's clicked again
		// Otherwise, we want to select it, adding it to the selected Gruntz

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
		// Select Grunt no matter what
		Select();
	}

	// Right click
	private void OnMove() {
		if (!flagz.selected) {
			return;
		}

		ResetAction();

		targetNode = LevelV2.Instance.levelNodes
			.First(n => n.location2D == GM.Instance.selector.location2D);

		Move();
	}

	// Left click & Shift
	private void OnAction() {
		if (!flagz.selected) {
			return;
		}

		ResetAction();

		// Find new interaction target
		interactionTarget =
			FindObjectsByType<GridObject>(FindObjectsSortMode.None)
				.FirstOrDefault(go => go.location2D == GM.Instance.selector.location2D && go is IInteractable);

		// Find new attack target
		// attackTarget = GM.Instance.allGruntz
		// 	.FirstOrDefault(g => g.location2D == GM.Instance.selector.location2D && g.enabled);

		// There was nothing found to interact with
		if (interactionTarget == null && attackTarget == null) {
			// Todo: Play voice line for being unable to interact with nothing
			return;
		}

		// There was an interactable object found
		if (interactionTarget != null) {
			// Check whether the Grunt has an appropriate tool equipped
			if (!((IInteractable)interactionTarget).CompatibleItemz.Contains(equippedTool.toolName)) {
				Debug.Log("Cannot interact with this! It's not compatible!");
				// Todo: Play voice line for having an incompatible tool

				return;
			}

			if (InRange(interactionTarget.node)) {
				Debug.Log("I'm in range!");
				targetNode = node;

				FaceTowards(interactionTarget.node);

				Interact();

				return;
			}

			Debug.Log("Adding listener to onTargetReached");
			onTargetReached.AddListener(Interact);
			flagz.setToInteract = true;
			targetNode = interactionTarget.node;

			Move();

			return;
		}

		// There was a Grunt found
		if (attackTarget != null && attackTarget.enabled) {
			// Check team (friend or enemy)
			// if (attackTarget.team == team) {
			// 	attackTarget = null;
			//  targetNode = node;
			//
			// 	return;
			// }

			if (InRange(attackTarget.node)) {
				Debug.Log("I'm in range!");
				targetNode = node;

				FaceTowards(attackTarget.node);

				Attack();

				return;
			}

			Debug.Log("Adding listener to onTargetReached");
			onTargetReached.AddListener(Attack);
			flagz.setToAttack = true;
			targetNode = attackTarget.node;

			Move();
		}
	}

	// Todo: Needs similar logic to OnAction
	// Left click & Alt
	private void OnGive() {
		if (!flagz.selected || IsInterrupted) {
			return;
		}

		interactionTarget = null;
		attackTarget = null;

		if (toy == null) {
			// Todo: Play voice line for not having a toy

			return;
		}

		Debug.Log("OnGive");

		// GruntV2 target = GM.Instance.allGruntz
		// 	.FirstOrDefault(grunt => grunt.location2D == GM.Instance.selector.location2D);

		// Todo: Move beside target

		// Todo: Give toy to target
		Debug.Log($"Giving with {gruntName}");
	}
	#endregion

	// --------------------------------------------------
	// Movement
	// --------------------------------------------------

	#region Movement
	/// <summary>
	/// Continually moves the Grunt's physical position while it is set to move.
	/// </summary>
	private void HandleMovement() {
		Vector3 moveVector = (next.transform.position - transform.position).normalized;
		gameObject.transform.position += moveVector * (Time.deltaTime / .6f);

		Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.walk));
	}

	public void FaceTowards(NodeV2 toFace) {
		Vector2 direction = (toFace.location2D - node.location2D);

		facingDirection = direction switch {
			_ when direction == Vector2.up => facingDirection = DirectionV2.Up,
			_ when direction == Vector2.down => facingDirection = DirectionV2.Down,
			_ when direction == Vector2.left => facingDirection = DirectionV2.Left,
			_ when direction == Vector2.right => facingDirection = DirectionV2.Right,
			_ when direction == (Vector2.up + Vector2.right) => facingDirection = DirectionV2.UpRight,
			_ when direction == (Vector2.up + Vector2.left) => facingDirection = DirectionV2.UpLeft,
			_ when direction == (Vector2.down + Vector2.right) => facingDirection = DirectionV2.DownRight,
			_ when direction == (Vector2.down + Vector2.left) => facingDirection = DirectionV2.DownLeft,
			_ => facingDirection,
		};
	}

	/// <summary>
	/// Moves the Grunt to the current target node.
	/// </summary>
	public void Move() {
		if (!ValidateMove()) {
			return;
		}

		List<NodeV2> newPath = Pathfinder.AstarSearch(node, targetNode, LevelV2.Instance.levelNodes.ToHashSet());

		if (newPath.Count <= 0) {
			Debug.Log("No further path found!");
			targetNode = node;

			if (attackTarget != null) {
				FaceTowards(attackTarget.node);
			} else if (interactionTarget != null) {
				FaceTowards(interactionTarget.node);
			}

			onNodeChanged.RemoveAllListeners();
			onTargetReached.Invoke(); // Attack/Interact/Give
			Debug.Log("Target reached!");

			return;
		}

		next = newPath[0];
		next.isReserved = true;
		flagz.moving = true;

		FaceTowards(next);

		onNodeChanged.RemoveAllListeners();
		onNodeChanged.AddListener(Move);
	}

	/// <summary>
	/// Forcibly moves the Grunt to the current target node.
	/// </summary>
	public void MoveToNode() {
		#region Validation
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

		FaceTowards(next);

		flagz.moving = true;
	}
	#endregion

	public bool InRange(NodeV2 otherNode) {
		return Mathf.Abs(node.location2D.x - otherNode.location2D.x) <= equippedTool.range
			&& Mathf.Abs(node.location2D.y - otherNode.location2D.y) <= equippedTool.range;
	}

	public async void Interact() {
		if (!ValidateInteraction()) {
			return;
		}

		flagz.hostileIdle = true;

		await UniTask.WaitUntil(() => statz.stamina == Statz.MaxValue);

		if (interactionTarget == null) {
			return;
		}

		flagz.hostileIdle = false;

		Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.interact));

		// Wait the duration of the interaction animation
		await UniTask.WaitForSeconds(0.75f);

		// equippedTool.InteractWith(interactionTarget);

		statz.stamina = 0;
		onStaminaDrained.Invoke();

		onTargetReached.RemoveAllListeners();
		onNodeChanged.RemoveAllListeners();
		flagz.setToInteract = false;
	}

	public async void Attack() {
		#region Validation
		if (flagz.moving) {
			Debug.Log("Don't attack because I'm moving!");

			return;
		}

		if (flagz.interrupted || flagz.moveForced) {
			Debug.Log("Can't attack because I'm interrupted or move is forced!");
			flagz.setToAttack = false;

			return;
		}

		// There is no attack target
		if (attackTarget == null) {
			Debug.Log("Don't attack because there's nothing to attack!");
			// Todo: Play voice line for being unable to attack nothing
			flagz.setToAttack = false;

			return;
		}
		#endregion


		flagz.hostileIdle = true;

		await UniTask.WaitUntil(() => statz.stamina == Statz.MaxValue);

		flagz.hostileIdle = false;

		if (attackTarget == null || !attackTarget.enabled) {
			return;
		}

		if (!InRange(attackTarget.node)) {
			onTargetReached.AddListener(Attack);
			flagz.setToAttack = true;
			targetNode = attackTarget.node;

			Move();

			return;
		}

		Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.attack));

		// Wait the duration of the attack animation
		await UniTask.WaitForSeconds(0.5f);

		if (attackTarget == null || !attackTarget.enabled) {
			return;
		}

		// equippedTool.Attack(attackTarget);

		statz.stamina = 0;
		onStaminaDrained.Invoke();

		onTargetReached.RemoveAllListeners();
		onNodeChanged.RemoveAllListeners();

		if (attackTarget.statz.health <= 0) {
			attackTarget = null;
		}

		// Try to attack again until commanded otherwise, cannot attack, or target doesn't exist anymore
		if (attackTarget != null && attackTarget.enabled) {
			Attack();
		}
	}

	public void TakeDamage(int damage) {
		statz.health = Math.Clamp(statz.health - damage, 0, Statz.MaxValue);
		barz.healthBar.Adjust(statz.health);
		onHit.Invoke();

		if (statz.health <= 0) {
			onDeath.Invoke();
		}
	}

	public async void Die() {
		ResetAction();

		enabled = false;
		spriteRenderer.sortingLayerName = "AlwaysBottom";
		Animancer.Play(animationPack.deathAnimation);

		// await UniTask.WaitForSeconds(1.5f);
		await UniTask.WaitForSeconds(animationPack.deathAnimation.length);

		Debug.Log("Im dead!");
		// GM.Instance.allGruntz.Remove(this);
		Destroy(gameObject);
		// Instantiate(gruntPuddle, transform.position, Quaternion.identity, GameObject.Find("Puddlez").transform);
	}

	/// <summary>
	/// Resets the Grunt's action statez.
	/// </summary>
	public void ResetAction() {
		onNodeChanged.RemoveAllListeners();
		onTargetReached.RemoveAllListeners();

		interactionTarget = null;
		attackTarget = null;

		flagz.setToInteract = false;
		flagz.setToAttack = false;
		flagz.setToGive = false;
	}

	#region Validationz
	private bool ValidateMove() {
		// Prevent starting new movement while moving (e.g. from consecutive OnMove() calls)
		if (flagz.moving) {
			return false;
		}

		// Grunt cannot move
		if (flagz.interrupted) {
			onNodeChanged.RemoveAllListeners();
			Debug.Log("I'm interrupted!");

			return false;
		}

		// Grunt's move is forced (e.g. by an Arrow)
		if (flagz.moveForced) {
			onNodeChanged.RemoveAllListeners();
			Debug.Log("Move is forced!");

			return false;
		}

		// Grunt has reached his target (or is already there)
		if (targetNode == node || targetNode == null) {
			if (flagz.moveForced) {
				return false;
			}

			onNodeChanged.RemoveAllListeners();
			onTargetReached.Invoke();
			Debug.Log("Target reached!");

			return false;
		}

		return true;
	}

	private bool ValidateInteraction() {
		// Prevent starting new movement while moving (e.g. from consecutive OnMove() calls)
		if (flagz.moving) {
			Debug.Log("Don't interact because I'm moving!");

			return false;
		}

		if (flagz.interrupted) {
			Debug.Log("Don't interact because I'm interrupted!");
			onNodeChanged.RemoveAllListeners();
			flagz.setToInteract = false;

			return false;
		}

		if (flagz.moveForced) {
			Debug.Log("Don't interact because move is forced!");
			onNodeChanged.RemoveAllListeners();

			return false;
		}

		// There was no interaction target found
		if (interactionTarget == null) {
			Debug.Log("Don't interact because there's nothing to interact with!");
			// Todo: Play voice line for being unable to interact with nothing
			onTargetReached.RemoveAllListeners();
			onNodeChanged.RemoveAllListeners();
			flagz.setToInteract = false;

			return false;
		}

		return true;
	}

	private bool ValidateAttack() {
		if (flagz.moving) {
			Debug.Log("Don't attack because I'm moving!");

			return false;
		}

		if (flagz.interrupted || flagz.moveForced) {
			Debug.Log("Can't attack because I'm interrupted or move is forced!");
			flagz.setToAttack = false;

			return false;
		}

		// There is no attack target
		if (attackTarget == null) {
			Debug.Log("Don't attack because there's nothing to attack!");
			// Todo: Play voice line for being unable to attack nothing
			// onTargetReached.RemoveAllListeners();
			// onNodeChanged.RemoveAllListeners();
			flagz.setToAttack = false;

			return false;
		}

		return true;
	}
	#endregion

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

		data.gruntData.InitializeListAdd(saveData);

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
