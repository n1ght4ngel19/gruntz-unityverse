using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz.Data;
using GruntzUnityverse.Actorz.UI;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using GruntzUnityverse.DataPersistence;
using GruntzUnityverse.Editor.PropertyDrawers;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Secretz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.UI;
using GruntzUnityverse.Utils;
using GruntzUnityverse.Utils.Extensionz;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace GruntzUnityverse.Actorz {
/// <summary>
/// The class representing a Grunt in the game.
/// </summary>
public class Grunt : MonoBehaviour, IDataPersistence {
	public UnityEvent onStateChanged;

	#region Fieldz
	// --------------------------------------------------
	// Statz
	// --------------------------------------------------

	[Header("Statz")]
	public int gruntId;

	public string displayName;

	public Material skinColor;

	public GruntColor textSkinColor;

	public Statz statz;

	[Range(0, 5)]
	public float moveSpeed;

	// --------------------------------------------------
	// Flagz
	// --------------------------------------------------

	[Header("Flagz")]
	public bool selected;

	public bool between;

	public bool forced;

	public bool isTrigger;

	public bool waiting;

	public bool committed;

	public bool canFly => equippedTool is Wingz;

	[SerializeField] private bool checkedForBlockedTravelGoal;

	// --------------------------------------------------
	// Equipment
	// --------------------------------------------------

	[Header("Equipment")]
	public EquippedTool equippedTool;

	public EquippedToy equippedToy;

	/// <summary>
	/// The powerup(z) currently active on this Grunt.
	/// </summary>
	public List<EquippedPowerup> equippedPowerupz;

	// --------------------------------------------------
	// Animation
	// --------------------------------------------------

	[Header("Animation")]
	public AnimancerComponent animancer;

	public AnimationPack animationPack;

	public Direction facingDirection;

	// -------------------------------------------------- //
	// Pathfinding
	// -------------------------------------------------- //

	#region Pathfinding
	/// <summary>
	/// The node this Grunt is currently on.
	/// </summary>
	[Header("Pathfinding")]
	public Node node;

	/// <summary>
	/// The node the Grunt is moving towards.
	/// </summary>
	public Node travelGoal;

	/// <summary>
	/// The next node the Grunt will move to.
	/// </summary>
	public Node next;

	/// <summary>
	/// The location of this Grunt in 2D space.
	/// </summary>
	public Vector2 location2D => node.location2D;
	#endregion

	// --------------------------------------------------
	// Interaction
	// --------------------------------------------------

	#region Interaction
	/// <summary>
	/// The target the Grunt will try to interact with.
	/// </summary>
	[Header("Action")]
	public GridObject interactionTarget;

	/// <summary>
	/// The target the Grunt will try to attack.
	/// </summary>
	public Grunt attackTarget;
	#endregion

	// --------------------------------------------------
	// UI
	// --------------------------------------------------

	#region UI
	[Header("UI")]
	public GruntEntry gruntEntry;
	#endregion

	// --------------------------------------------------
	// Eventz
	// --------------------------------------------------

	#region Eventz
	[Header("Eventz")]
	[HideInNormalInspector]
	public UnityEvent onStaminaDrained;

	[HideInNormalInspector]
	public UnityEvent onStaminaRegenerated;

	[HideInNormalInspector]
	public UnityEvent onHit;

	public UnityEvent onDeath;
	#endregion

	// --------------------------------------------------
	// Componentz
	// --------------------------------------------------

	[Header("Componentz")]
	public SpriteRenderer spriteRenderer;

	public CircleCollider2D circleCollider2D;

	public GameObject selectionMarker;

	public Barz barz;
	#endregion

	public StateHandler stateHandler;

	public void Idle(bool hostile = false) {
		checkedForBlockedTravelGoal = false;
		travelGoal = node;
		next = node;

		AnimationClip toPlay = hostile
			? AnimationPack.GetRandomClip(facingDirection, animationPack.hostileIdle)
			: AnimationPack.GetRandomClip(facingDirection, animationPack.idle);

		animancer.Play(toPlay);
	}

	public void TryWalk() {
		List<Node> path = Pathfinder.AstarSearch(node, travelGoal, Level.instance.levelNodes, canFly);

		if (path.Count <= 0) {
			travelGoal = node;
			GoToState(StateHandler.State.Idle);

			return;
		}

		// When first node of path is free or is reserved by this Grunt
		if (path[0].reservedBy == null || path[0].reservedBy == this) {
			// Manually to kick off movement
			between = true;
			next = path[0];

			FaceTowardsNode(next);
			animancer.Play(AnimationPack.GetRandomClip(facingDirection, animationPack.walk));

			GoToState(StateHandler.State.Walking);
		}
	}

	public void GoToState(StateHandler.State toState) {
		stateHandler.goToState = toState;

		if (!between) {
			onStateChanged.Invoke();
		}
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	#region Lifecycle
	private void Start() {
		if (gameObject.CompareTag("PlayerGrunt")) {
			gruntId = GameManager.instance.playerGruntz.ToList().IndexOf(this) + 1;

			selectionMarker = transform.Find("SelectionMarker").gameObject;

			gruntEntry = FindObjectsByType<GruntEntry>(FindObjectsSortMode.None)
				.First(entry => entry.entryId == gruntId);

			Debug.Log(gruntEntry is null);

			if (equippedTool != null) {
				gruntEntry.SetTool(equippedTool.toolName.Replace(" ", ""));
			}

			if (equippedToy != null) {
				gruntEntry.SetToy(equippedToy.toyName.Replace(" ", ""));
			}

			gruntEntry.SetHealth(statz.health);
			gruntEntry.SetStamina(statz.stamina);
		}

		node = Level.instance.levelNodes.First(n => n.location2D == Vector2Int.RoundToInt(transform.position));

		animancer.Play(AnimationPack.GetRandomClip(facingDirection, animationPack.idle));
	}

	private void FixedUpdate() {
		if (between) {
			ChangePosition();
		}
	}
	#endregion

	// --------------------------------------------------
	// Input Actions
	// --------------------------------------------------

	#region Input Actions
	#region Selection
	/// <summary>
	/// Try to select the Grunt if he is under the selector.
	/// </summary>
	private void OnSelect() {
		if (GameManager.instance.selector.location2D == node.location2D) {
			Select();
		} else {
			Deselect();
		}
	}

	/// <summary>
	/// Try to select the Grunt if he is under the selector in addition to any already selected Grunt(z).
	/// If he is already selected, deselect him.
	/// </summary>
	private void OnAdditionalSelect() {
		if (GameManager.instance.selector.location2D != location2D) {
			return;
		}

		if (selected) {
			Deselect();
		} else {
			Select();
		}
	}

	/// <summary>
	/// Try selecting the Grunt even if it's not under the selector.
	/// </summary>
	private void OnSelectAll() {
		Select();
	}
	#endregion

	/// <summary>
	/// Set the Grunt's travel goal to the selector's location, then evaluate the Grunt's state if possible.
	/// </summary>
	private void OnMove() {
		if (!selected) {
			return;
		}

		interactionTarget = null;
		attackTarget = null;

		Node selectorNode = GameManager.instance.selector.node;

		if (selectorNode == node || selectorNode == null) {
			travelGoal = node;
			GoToState(StateHandler.State.Idle);

			return;
		}

		travelGoal = selectorNode;
		GoToState(StateHandler.State.Walking);
	}

	/// <summary>
	/// Search for an interaction or attack target under the selector, set the appropriate target, then evaluate the Grunt's state if possible.
	/// </summary>
	private void OnAction() {
		if (!selected) {
			return;
		}

		interactionTarget = FindObjectsByType<GridObject>(FindObjectsSortMode.None)
			.FirstOrDefault(go => go.enabled && go.node == GameManager.instance.selector.node && equippedTool.CompatibleWith(go));


		if (interactionTarget != null) {
			// Debug.Log("Interaction target found");
			TryTakeAction(interactionTarget.node, "Interact");

			return;
		}

		attackTarget = GameManager.instance.allGruntz
			.FirstOrDefault(g => g.node == GameManager.instance.selector.node && g.enabled && g != this);

		if (attackTarget != null) {
			// Debug.Log("Attack target found");
			TryTakeAction(attackTarget.node, "Attack");

			return;
		}

		// Todo: Play voice line for having an incompatible tool

		// Debug.Log("No interaction or attack target found");
		GoToState(StateHandler.State.Idle);
	}

	/// <summary>
	/// Try to take action.
	/// </summary>
	/// <param name="targetNode">The node of the target.</param>
	/// <param name="action">The action to take.</param>
	public void TryTakeAction(Node targetNode, string action) {
		switch (action) {
			case "Interact": {
				if (InRange((targetNode))) {
					travelGoal = node;

					StartCoroutine(TryInteract());
				} else {
					travelGoal = targetNode;

					GoToState(StateHandler.State.Walking);
				}

				break;
			}
			case "Attack": {
				if (InRange((targetNode))) {
					travelGoal = node;

					StartCoroutine(TryAttack());
				} else {
					travelGoal = targetNode;

					GoToState(StateHandler.State.Walking);
				}

				break;
			}
			default: {
				GoToState(StateHandler.State.Walking);

				break;
			}
		}
	}

	/// <summary>
	/// Force the Grunt into idling.
	/// </summary>
	private void OnForceIdle() {
		if (!selected) {
			return;
		}

		Debug.Log("Log6");
		GoToState(StateHandler.State.Idle);
	}

	/// <summary>
	/// Todo
	/// </summary>
	private void OnGive() {
		if (!selected) {
			return;
		}
	}
	#endregion

	// --------------------------------------------------
	// Selection
	// --------------------------------------------------

	public void Select() {
		selected = true;
		selectionMarker.SetActive(true);
		gruntEntry.HighLight();
	}

	public void Deselect() {
		selected = false;
		selectionMarker.SetActive(false);
		gruntEntry.HighLight(false);
	}

	public void ArrowMove() {
		next = travelGoal;
		FaceTowardsNode(next);
		forced = false;
	}

	public IEnumerator TryInteract() {
		if (!InRange(interactionTarget.node)) {
			GoToState(StateHandler.State.Idle);

			yield break;
		}

		// Out of stamina, waiting until it is regenerated
		if (statz.stamina < Statz.MAX_VALUE) {
			FaceTowardsNode(interactionTarget.node);

			GoToState(StateHandler.State.Idle);

			yield break;
		}

		FaceTowardsNode(interactionTarget.node);

		committed = true;

		if (interactionTarget is Rock rock) {
			yield return BreakRock(rock);
		}

		if (interactionTarget is Hole hole) {
			yield return Dig(hole);
		}

		if (interactionTarget is GruntPuddle puddle) {
			yield return SuckPuddle(puddle);
		}

		committed = false;

		DrainStamina();

		GoToState(StateHandler.State.Idle);
	}

	private IEnumerator Dig(Hole hole) {
		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.interact);

		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length * 0.5f);

		hole.dirt.GetComponent<AnimancerComponent>().Play(AnimationManager.instance.dirtEffect);

		yield return new WaitForSeconds(toPlay.length * 1f);

		hole.Dig();

		yield return new WaitForSeconds(toPlay.length * 0.5f);

		interactionTarget = null;
	}

	private IEnumerator BreakRock(Rock rock) {
		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.interact);

		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length * 0.75f);

		rock.Break();

		yield return new WaitForSeconds(toPlay.length * 0.25f);

		interactionTarget = null;
	}

	private IEnumerator SuckPuddle(GruntPuddle puddle) {
		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.interact);

		animancer.Play(toPlay);

		yield return new WaitForSeconds(0.5f);

		puddle.Disappear();

		yield return new WaitForSeconds(toPlay.length - 0.5f);

		GameManager.instance.gooWell.Fill(puddle.gooAmount);

		interactionTarget = null;
	}

	public IEnumerator TryAttack() {
		if (!attackTarget.enabled) {
			GoToState(StateHandler.State.Idle);

			yield break;
		}

		if (!InRange(attackTarget.node)) {
			GoToState(StateHandler.State.Idle);

			yield break;
		}

		// Out of stamina, waiting until it is regenerated
		if (statz.stamina < Statz.MAX_VALUE) {
			FaceTowardsNode(interactionTarget.node);

			GoToState(StateHandler.State.HostileIdle);

			yield break;
		}

		FaceTowardsNode(attackTarget.node);

		committed = true;

		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.attack);
		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length / 2);

		attackTarget.TakeDamage(equippedTool.damage);

		yield return new WaitForSeconds(toPlay.length / 2);

		committed = false;

		DrainStamina();

		if (attackTarget.statz.health <= 0) {
			attackTarget = null;

			GoToState(StateHandler.State.Idle);
		}
	}

	public void RegenerateStamina() {
		if (statz.stamina >= Statz.MAX_VALUE) {
			CancelInvoke(nameof(RegenerateStamina));

			statz.stamina = Statz.MAX_VALUE;
			onStaminaRegenerated.Invoke();

			if (attackTarget != null) {
				Debug.Log("stamina is full");
				GoToState(StateHandler.State.Attacking);

				return;
			}

			if (attackTarget != null) {
				GoToState(StateHandler.State.Interacting);

				return;
			}
		}

		statz.stamina++;
		barz.staminaBar.Adjust(statz.stamina);

		if (!CompareTag("Dizgruntled")) {
			gruntEntry.SetStamina(statz.stamina);
		}
	}

	public void DrainStamina() {
		statz.stamina = 0;
		onStaminaDrained.Invoke();
		InvokeRepeating(nameof(RegenerateStamina), 0, statz.staminaRegenRate);

		if (attackTarget != null || interactionTarget != null) {
			GoToState(StateHandler.State.HostileIdle);
		}
	}

	/// <summary>
	/// Continually moves the Grunt's physical position between the current and next node.
	/// </summary>
	private void ChangePosition() {
		Vector3 moveVector = (next.transform.position - node.transform.position);
		gameObject.transform.position += moveVector * (Time.fixedDeltaTime / moveSpeed);
	}

	/// <summary>
	/// Faces the Grunt towards the given node.
	/// </summary>
	/// <param name="toFace">The node to face towards.</param>
	private void FaceTowardsNode(Node toFace) {
		Vector2 direction = (toFace.location2D - node.location2D);

		facingDirection = direction switch {
			_ when direction == Vector2.up => facingDirection = Direction.Up,
			_ when direction == Vector2.down => facingDirection = Direction.Down,
			_ when direction == Vector2.left => facingDirection = Direction.Left,
			_ when direction == Vector2.right => facingDirection = Direction.Right,
			_ when direction == (Vector2.up + Vector2.right) => facingDirection = Direction.UpRight,
			_ when direction == (Vector2.up + Vector2.left) => facingDirection = Direction.UpLeft,
			_ when direction == (Vector2.down + Vector2.right) => facingDirection = Direction.DownRight,
			_ when direction == (Vector2.down + Vector2.left) => facingDirection = Direction.DownLeft,
			_ => facingDirection,
		};
	}

	/// <summary>
	/// Checks whether the given node is in range of the Grunt's equipped tool.
	/// </summary>
	/// <param name="otherNode">The node to check.</param>
	/// <returns>True when the node is in range, false otherwise.</returns>
	private bool InRange(Node otherNode) {
		return Mathf.Abs(node.location2D.x - otherNode.location2D.x) <= equippedTool.range
			&& Mathf.Abs(node.location2D.y - otherNode.location2D.y) <= equippedTool.range;
	}

	/// <summary>
	/// Damage the grunt, adjusting its health bar as well.
	/// </summary>
	/// <param name="damage">The amount of damage to be dealt.</param>
	public void TakeDamage(int damage) {
		statz.health = Math.Clamp(statz.health - damage, 0, Statz.MAX_VALUE);
		barz.healthBar.Adjust(statz.health);

		if (!CompareTag("Dizgruntled")) {
			gruntEntry.SetHealth(statz.health);
		}

		if (statz.health <= 0) {
			onDeath.Invoke();
		} else {
			onHit.Invoke();
		}
	}

	/// <summary>
	/// Kills the Grunt. (duh)
	/// </summary>
	public async void Die() {
		enabled = false;

		CancelInvoke(nameof(RegenerateStamina));
		DeactivateBarz();

		Addressables.InstantiateAsync($"GruntPuddle_{textSkinColor.ToString()}", GameObject.Find("Puddlez").transform).Completed += handle => {
			GruntPuddle puddle = handle.Result.GetComponent<GruntPuddle>();
			puddle.transform.position = transform.position;
		};

		spriteRenderer.sortingLayerName = "AlwaysBottom";
		spriteRenderer.sortingOrder = 0;

		animancer.Play(animationPack.deathAnimation);
		await UniTask.WaitForSeconds(animationPack.deathAnimation.length);

		if (CompareTag("PlayerGrunt")) {
			gruntEntry.Clear();
		}

		GameManager.instance.allGruntz.Remove(this);
		Level.instance.levelStatz.deathz++;
		Destroy(gameObject, 1f);
	}

	/// <summary>
	/// Kills the Grunt. (duh)
	/// </summary>
	public async void Die(AnimationClip toPlay, bool leavePuddle = true, bool playBelow = true) {
		enabled = false;

		CancelInvoke(nameof(RegenerateStamina));
		DeactivateBarz();

		if (leavePuddle) {
			Addressables.InstantiateAsync($"GruntPuddle_{textSkinColor.ToString()}", GameObject.Find("Puddlez").transform).Completed += handle => {
				GruntPuddle puddle = handle.Result.GetComponent<GruntPuddle>();
				puddle.transform.position = transform.position;
			};
		}

		if (playBelow) {
			spriteRenderer.sortingLayerName = "AlwaysBottom";
			spriteRenderer.sortingOrder = 0;
		}

		animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		spriteRenderer.enabled = false;
		GameManager.instance.allGruntz.Remove(this);
		Level.instance.levelStatz.deathz++;
		Destroy(gameObject);
	}

	public IEnumerator Teleport(Transform destination, Warp fromWarp) {
		enabled = false;

		// The first part of the animation plays where the Grunt is sucked into the warp
		animancer.Play(AnimationManager.instance.gruntWarpOutEndAnimation);

		yield return new WaitForSeconds(AnimationManager.instance.gruntWarpOutEndAnimation.length);

		Camera.main.transform.position = new Vector3(destination.position.x, destination.position.y, -10);
		transform.position = destination.position;
		node = Level.instance.levelNodes.First(n => n.location2D == Vector2Int.RoundToInt(transform.position));

		// The second part of the animation plays where the Grunt is spat out of the warp
		animancer.Play(AnimationManager.instance.gruntWarpEnterAnimation);

		yield return new WaitForSeconds(AnimationManager.instance.gruntWarpEnterAnimation.length);

		enabled = true;

		GoToState(StateHandler.State.Idle);

		fromWarp.Deactivate();
	}

	private void DeactivateBarz() {
		barz.healthBar.gameObject.SetActive(false);
		barz.staminaBar.gameObject.SetActive(false);
		barz.toyTimeBar.gameObject.SetActive(false);
		barz.wingzTimeBar.gameObject.SetActive(false);
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
			gruntName = displayName,
			position = transform.position,
		};

		data.gruntData.InitializeListAdd(saveData);

		Debug.Log($"Saving {displayName} at {transform.position} with GUID {Guid}");
	}

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
		displayName = data.gruntName;
		// transform.position = data.position;
	}

	public void GenerateGuid() {
		Guid = System.Guid.NewGuid().ToString();
	}
	#endregion

	#if UNITY_EDITOR
	/// <summary>
	/// Hide components not meant to be edited or observed in the Inspector.
	/// This also removes visual clutter in the Inspector for easier editing.
	/// </summary>
	private void OnValidate() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.material = skinColor;
		spriteRenderer.hideFlags = HideFlags.HideInInspector;

		textSkinColor = (GruntColor)Enum.Parse(typeof(GruntColor), skinColor.name);

		circleCollider2D = GetComponent<CircleCollider2D>();
		circleCollider2D.isTrigger = isTrigger;

		GetComponent<Animator>().hideFlags = HideFlags.HideInInspector;

		animancer = GetComponent<AnimancerComponent>();
		animancer.hideFlags = HideFlags.HideInInspector;

		GetComponent<TrimName>().hideFlags = HideFlags.HideInInspector;
	}
	#endif
}
}
