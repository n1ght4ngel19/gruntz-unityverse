﻿using System;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.DataPersistence;
using GruntzUnityverse.V2.Editor;
using GruntzUnityverse.V2.Itemz.Toolz;
using GruntzUnityverse.V2.Objectz;
using GruntzUnityverse.V2.Pathfinding;
using GruntzUnityverse.V2.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace GruntzUnityverse.V2.Grunt {
/// <summary>
/// The class representing a Grunt in the game.
/// </summary>
public class GruntFSM : MonoBehaviour, IDataPersistence, IAnimatable {
	/// <summary>
	/// The name of this Grunt.
	/// </summary>
	[Header("Statz")]
	public string gruntName;

	public Statz statz;

	[Header("State Handling")]
	public State state;

	public Intent intent;

	[Header("Flagz")]
	public bool selected;

	public bool waiting;

	public bool committed;

	public bool betweenNodes;

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
	#endregion

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

	// -------------------------
	// IAnimatable
	// -------------------------

	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	#endregion

	// -------------------------------------------------- //
	// Pathfinding
	// -------------------------------------------------- //

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
	public NodeV2 travelGoal;

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
	public GruntFSM attackTarget;
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

	#region Componentz
	[Header("Componentz")]
	public SpriteRenderer spriteRenderer;

	public CircleCollider2D circleCollider2D;

	public GameObject selectionMarker;
	#endregion

	public Barz barz;

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	#region Lifecycle
	private void Awake() {
		location2D = Vector2Int.RoundToInt(transform.position);
		spriteRenderer = GetComponent<SpriteRenderer>();
		circleCollider2D = GetComponent<CircleCollider2D>();
	}

	private void Start() {
		node = LevelV2.Instance.levelNodes.First(n => n.location2D == location2D);

		Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.idle));
	}

	private void Update() {
		if (state == State.Idle) {
			Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.idle));
		} else if (state == State.Moving) {
			ChangePosition();
		} else if (waiting) {
			Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.hostileIdle));
		}
	}
	#endregion

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<NodeV2>()) {
			node = other.GetComponent<NodeV2>();
			node.isReserved = false;
			transform.position = node.transform.position;

			betweenNodes = false;
			EvaluateState();
		}
	}

	// --------------------------------------------------
	// Input Actions
	// --------------------------------------------------

	#region Input Actions
	#region Selection
	/// <summary>
	/// Try to select the Grunt if he is under the selector.
	/// </summary>
	private void OnSelect() {
		if (GM.Instance.selector.location2D == node.location2D) {
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
		if (GM.Instance.selector.location2D != location2D) {
			return;
		}

		selected = !selected;
		selectionMarker.SetActive(!selectionMarker.activeSelf);
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

		travelGoal = LevelV2.Instance.levelNodes
			.First(n => n.location2D == GM.Instance.selector.location2D);

		intent = Intent.ToMove;
		EvaluateState(whenFalse: (betweenNodes || committed));
	}

	/// <summary>
	/// Search for an interaction or attack target under the selector, set the appropriate target, then evaluate the Grunt's state if possible.
	/// </summary>
	private void OnAction() {
		if (!selected) {
			return;
		}

		interactionTarget =
			FindObjectsByType<GridObject>(FindObjectsSortMode.None)
				.FirstOrDefault(go => go.location2D == GM.Instance.selector.location2D && go is IInteractable);

		if (interactionTarget != null) {
			HandleActionCommand(interactionTarget.node, Intent.ToInteract);

			return;
		}

		attackTarget = GM.Instance.allGruntz
			.FirstOrDefault(g => g.node == GM.Instance.selector.node && g.enabled);

		if (attackTarget != null) {
			HandleActionCommand(attackTarget.node, Intent.ToAttack);
		}
	}

	/// <summary>
	/// Take appropriate action according to the interaction/attack target's node and intent.
	/// </summary>
	/// <param name="targetNode">The node of the target. </param>
	/// <param name="newIntent">The intent against the target.</param>
	private void HandleActionCommand(NodeV2 targetNode, Intent newIntent) {
		travelGoal = InRange(targetNode) ? node : targetNode;

		if (travelGoal == null) {
			travelGoal = node;
			intent = Intent.ToIdle;
			EvaluateState(whenFalse: betweenNodes);
		}

		intent = newIntent;
		EvaluateState(whenFalse: betweenNodes || committed);
	}

	/// <summary>
	/// Force the Grunt into an idle state.
	/// </summary>
	private void OnForceIdle() {
		if (!selected) {
			return;
		}

		intent = Intent.ToIdle;
		state = State.Idle;
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

	/// <summary>
	/// Evaluate the Grunt's current state, then take appropriate action.
	/// </summary>
	/// <param name="whenFalse">A condition potentially blocking evaluation.</param>
	private async void EvaluateState(bool whenFalse = false) {
		if (whenFalse) {
			return;
		}

		if (node == travelGoal) {
			if (waiting) {
				await UniTask.WaitWhile(() => statz.stamina < Statz.MaxValue);
				waiting = false;

				switch (intent) {
					case Intent.ToInteract:
						state = State.Interacting;
						Interact(interactionTarget);

						return;
					case Intent.ToAttack:
						state = State.Attacking;
						Attack(attackTarget);

						return;
				}
			}

			switch (intent) {
				case Intent.ToMove or Intent.ToIdle:
					intent = Intent.ToIdle;
					state = State.Idle;

					return;
				case Intent.ToInteract:
					state = State.Interacting;
					Interact(interactionTarget);

					return;
				case Intent.ToAttack:
					state = State.Attacking;
					Attack(attackTarget);

					return;
			}
		} else {
			Move(travelGoal);
		}
	}

	private void Select() {
		selected = true;
		selectionMarker.SetActive(true);
	}

	private void Deselect() {
		selected = false;
		selectionMarker.SetActive(false);
	}

	private void Move(NodeV2 target) {
		if (target == node) {
			GoToIdleIfNotActing();

			return;
		}

		if (target == null) {
			return;
		}

		// When target is unreachable, search for a new target adjacent to it
		if (!target.IsWalkable) {
			if (node.neighbours.Contains(target)) {
				travelGoal = node;

				GoToIdleIfNotActing();

				return;
			}

			List<NodeV2> freeNeighbours = target.neighbours.Where(n => n.IsWalkable).ToList();

			if (freeNeighbours.Count == 0) {
				travelGoal = node;

				GoToIdleIfNotActing();

				return;
			}

			target = freeNeighbours.OrderBy(n => Pathfinder.CalculateHeuristic(node, n)).First();
		}

		// Search for path to target
		List<NodeV2> newPath = Pathfinder.AstarSearch(node, target, LevelV2.Instance.levelNodes.ToHashSet());

		// When no path found, evaluate state
		if (newPath.Count <= 0) {
			travelGoal = node;

			GoToIdleIfNotActing();

			return;
		}

		state = State.Moving;
		next = newPath[0];
		next.isReserved = true;
		betweenNodes = true;

		FaceTowardsNode(next);
	}

	private void GoToIdleIfNotActing() {
		if (intent is Intent.ToMove or Intent.ToIdle) {
			intent = Intent.ToIdle;
		}

		EvaluateState(whenFalse: betweenNodes);
	}

	private async void Interact(GridObject target) {
		if (!target.enabled) {
			// Todo: Play voice line for being unable to interact

			intent = Intent.ToIdle;
			EvaluateState(whenFalse: betweenNodes);

			return;
		}

		// Check that the Grunt has a compatible item equipped
		if (!((IInteractable)target).CompatibleItemz.Contains(equippedTool.toolName)) {
			Debug.Log("Cannot interact with this! It's not compatible!");
			// Todo: Play voice line for having an incompatible tool

			intent = Intent.ToIdle;
			EvaluateState(whenFalse: betweenNodes);

			return;
		}

		if (!InRange(target.node)) {
			travelGoal = target.node;
			EvaluateState(whenFalse: betweenNodes);

			return;
		}

		if (statz.stamina < Statz.MaxValue) {
			FaceTowardsNode(target.node);

			waiting = true;
			intent = Intent.ToInteract;
			EvaluateState(whenFalse: betweenNodes);

			return;
		}

		FaceTowardsNode(target.node);

		AnimationClip toPlay = AnimationPackV2.GetRandomClip(facingDirection, animationPack.interact);
		Animancer.Play(toPlay);

		committed = true;
		await UniTask.WaitForSeconds(toPlay.length / 2);

		((IInteractable)interactionTarget).Interact();

		await UniTask.WaitForSeconds(toPlay.length / 2);
		committed = false;

		DrainStamina();

		intent = Intent.ToIdle;
		EvaluateState(whenFalse: betweenNodes);
	}

	private async void Attack(GruntFSM target) {
		if (!target.enabled) {
			// Todo: Play voice line for being unable to interact

			intent = Intent.ToIdle;
			EvaluateState();

			return;
		}

		if (!InRange(target.node)) {
			travelGoal = target.node;
			EvaluateState(whenFalse: betweenNodes);

			return;
		}

		if (statz.stamina < Statz.MaxValue) {
			FaceTowardsNode(target.node);

			waiting = true;
			intent = Intent.ToAttack;
			EvaluateState(whenFalse: betweenNodes);

			return;
		}

		FaceTowardsNode(target.node);

		AnimationClip toPlay = AnimationPackV2.GetRandomClip(facingDirection, animationPack.attack);
		Animancer.Play(toPlay);

		committed = true;
		await UniTask.WaitForSeconds(toPlay.length / 2);

		attackTarget.TakeDamage(equippedTool.damage);
		attackTarget.onHit.Invoke();

		await UniTask.WaitForSeconds(toPlay.length / 2);
		committed = false;

		DrainStamina();

		if (attackTarget.statz.health <= 0) {
			attackTarget = null;

			intent = Intent.ToIdle;
		}

		EvaluateState(whenFalse: betweenNodes);
	}

	public void RegenerateStamina() {
		if (statz.stamina >= Statz.MaxValue) {
			CancelInvoke(nameof(RegenerateStamina));

			statz.stamina = Statz.MaxValue;
			onStaminaRegenerated.Invoke();

			return;
		}

		statz.stamina++;
		barz.staminaBar.Adjust(statz.stamina);
	}

	public void DrainStamina() {
		statz.stamina = 0;
		onStaminaDrained.Invoke();
		InvokeRepeating(nameof(RegenerateStamina), 0, 0.2f);
	}

	/// <summary>
	/// Continually moves the Grunt's physical position between the current and next node.
	/// </summary>
	private void ChangePosition() {
		Vector3 moveVector = (next.transform.position - transform.position).normalized;
		gameObject.transform.position += moveVector * (Time.deltaTime / .6f);

		Animancer.Play(AnimationPackV2.GetRandomClip(facingDirection, animationPack.walk));
	}

	/// <summary>
	/// Faces the Grunt towards the given node.
	/// </summary>
	/// <param name="toFace">The node to face towards.</param>
	private void FaceTowardsNode(NodeV2 toFace) {
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
	/// Checks whether the given node is in range of the Grunt's equipped tool.
	/// </summary>
	/// <param name="otherNode">The node to check.</param>
	/// <returns>True when the node is in range, false otherwise.</returns>
	private bool InRange(NodeV2 otherNode) {
		return Mathf.Abs(node.location2D.x - otherNode.location2D.x) <= equippedTool.range
			&& Mathf.Abs(node.location2D.y - otherNode.location2D.y) <= equippedTool.range;
	}

	/// <summary>
	/// Damage the grunt, adjusting its health bar as well.
	/// </summary>
	/// <param name="damage">The amount of damage to be dealt.</param>
	private void TakeDamage(int damage) {
		statz.health = Math.Clamp(statz.health - damage, 0, Statz.MaxValue);
		barz.healthBar.Adjust(statz.health);

		if (statz.health <= 0) {
			onDeath.Invoke();
		}
	}

	/// <summary>
	/// Kills the Grunt. (duh)
	/// </summary>
	public async void Die() {
		enabled = false;

		DeactivateBarz();
		SetSortingData("AlwaysBottom", 0);

		AnimationClip toPlay = animationPack.deathAnimation;

		Animancer.Play(toPlay);

		await UniTask.WaitForSeconds(toPlay.length);

		GM.Instance.allGruntz.Remove(this);
		// Destroy(gameObject);
		// Instantiate(gruntPuddle, transform.position, Quaternion.identity, GameObject.Find("Puddlez").transform);
	}

	/// <summary>
	/// Set the sorting layer and order of the sprite renderer.
	/// </summary>
	/// <param name="layer">The sorting layer to set.</param>
	/// <param name="order">The sorting order to set.</param>
	private void SetSortingData(string layer, int order) {
		spriteRenderer.sortingLayerName = layer;
		spriteRenderer.sortingOrder = order;
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
			gruntName = gruntName,
			position = transform.position,
		};

		data.gruntData.InitializeListAdd(saveData);

		Debug.Log($"Saving {gruntName} at {transform.position} with GUID {Guid}");
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
		gruntName = data.gruntName;
		// transform.position = data.position;
	}

	public void GenerateGuid() {
		Guid = System.Guid.NewGuid().ToString();
	}
	#endregion

}
}
