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
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Itemz.Toyz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.UI;
using GruntzUnityverse.Utils;
using GruntzUnityverse.Utils.Extensionz;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GruntzUnityverse.Actorz {
/// <summary>
/// The class representing a Grunt in the game.
/// </summary>
public class Grunt : MonoBehaviour, IDataPersistence {
	public int spriteSortingOrder;

	public UnityEvent onStateChanged;

	#region Fieldz
	// --------------------------------------------------
	// Statz
	// --------------------------------------------------

	[Header("Statz")]
	public int gruntId;

	[Range(0, 5)]
	public int team;

	public string displayName;

	public Sprite previewSprite;

	[Tooltip("The material to be applied to achieve the Grunt's final look.")]
	public Material skinColor;

	public string textSkinColor => skinColor.name.Split("_").Last();

	public Statz statz;

	[Range(0, 5)]
	[Tooltip("The movement speed of the Grunt, in seconds/tile.")]
	public float moveSpeed;

	[Tooltip("Reduce damage dealt by non-hazard sourcez by this percentage.")]
	public float damageReductionPercentage;

	[Tooltip("Reflect damage dealt by other actorz by this percentage ")]
	public float damageReflectionPercentage;

	// --------------------------------------------------
	// Flagz
	// --------------------------------------------------

	[Header("Flagz")]
	public bool selected;

	[Tooltip("Whether the Grunt is currently in between two nodes.")]
	public bool between;

	[Tooltip("Whether the Grunt is currently being forced to move.")]
	public bool forced;

	public bool isTrigger;

	public bool waiting;

	public bool committed;

	private bool _onSpikez;

	public bool canFly => equippedTool is Wingz;

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
	private Vector2Int location2D => Vector2Int.RoundToInt(transform.position);
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

	/// <summary>
	/// The target the Grunt will try to give a toy to.
	/// </summary>
	public Grunt giveTarget;

	public List<Grunt> enemiez => gameManager.allGruntz.Where(gr => gr.team != team).ToList();
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
	public UnityEvent onStaminaDrained;

	public UnityEvent onStaminaRegenerated;

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

	public GameManager gameManager;

	public void Idle(bool hostile = false) {
		travelGoal = node;
		next = node;

		AnimationClip toPlay = hostile
			? AnimationPack.GetRandomClip(facingDirection, animationPack.hostileIdle)
			: AnimationPack.GetRandomClip(facingDirection, animationPack.idle);

		animancer.Play(toPlay);
	}

	public void TryWalk() {
		if (forced) {
			forced = false;
			between = true;
			next = travelGoal;

			FaceTowardsNode(next);
			animancer.Play(AnimationPack.GetRandomClip(facingDirection, animationPack.walk));

			GoToState(StateHandler.State.Walking);

			return;
		}

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
		if (stateHandler.currentState == StateHandler.State.Dying) {
			return;
		}

		stateHandler.goToState = toState;

		if (!between) {
			onStateChanged.Invoke();
		}
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	#region Lifecycle
	private void Awake() {
		spriteSortingOrder = spriteRenderer.sortingOrder;

		gameManager = FindFirstObjectByType<GameManager>();

		if (team == 0) {
			gameManager.playerGruntz.Add(this);

			gruntId = gameManager.playerGruntz.Count;
		} else {
			gameManager.dizgruntled.Add(this);
		}
	}

	private void Start() {
		if (team == 0) {
			selectionMarker = transform.Find("SelectionMarker").gameObject;

			gruntEntry = FindObjectsByType<GruntEntry>(FindObjectsSortMode.None)
				.First(entry => entry.entryId == gruntId);

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

	private void OnTriggerEnter2D(Collider2D other) {
		if (gameManager.spikez.Any(sp => sp.location2D == location2D)) {
			if (_onSpikez) {
				return;
			}

			_onSpikez = true;

			InvokeRepeating(nameof(SpikezDamage), 0f, 1f);
		} else if (_onSpikez) {
			_onSpikez = false;

			CancelInvoke(nameof(SpikezDamage));
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
		if (gameManager.selector.location2D == node.location2D) {
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
		if (gameManager.selector.location2D != location2D) {
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

		Node selectorNode = gameManager.selector.node;

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

		attackTarget = gameManager.allGruntz
			.FirstOrDefault(g => g.enabled && g.node == gameManager.selector.node && !g.between && g != this);

		if (attackTarget != null) {
			interactionTarget = null;
			TryTakeAction(attackTarget.node, "Attack");

			return;
		}

		interactionTarget = gameManager.gridObjectz
			.FirstOrDefault(go => go.enabled && go.node == gameManager.selector.node && equippedTool.CompatibleWith(go));

		if (interactionTarget != null) {
			attackTarget = null;
			TryTakeAction(interactionTarget.node, "Interact");

			return;
		}

		// Todo: Play voice line for having an incompatible tool

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
				if (InToolRange((targetNode))) {
					travelGoal = node;

					StartCoroutine(TryInteract());
				} else {
					travelGoal = targetNode;

					GoToState(StateHandler.State.Walking);
				}

				break;
			}
			case "Attack": {
				if (InToolRange((targetNode))) {
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

		attackTarget = null;
		interactionTarget = null;

		giveTarget = gameManager.allGruntz
			.FirstOrDefault(g => g.enabled && g.node == gameManager.selector.node && !g.between && g != this);

		GoToState(StateHandler.State.Giving);
	}
	#endregion

	public void TryGiveToy() {
		if (giveTarget == null) {
			GoToState(StateHandler.State.Idle);

			return;
		}

		if (!node.neighbours.Contains(giveTarget.node)) {
			travelGoal = giveTarget.node;

			GoToState(StateHandler.State.Walking);
		} else {
			travelGoal = node;

			StartCoroutine(giveTarget.PlayWithToy(equippedToy));

			giveTarget = null;
			equippedToy = null;
			gruntEntry.ClearSlot("Toy");

			GoToState(StateHandler.State.Idle);
		}
	}

	public IEnumerator PlayWithToy(EquippedToy toy) {
		if (TryGetComponent(out AI.AI ai)) {
			ai.enabled = false;
		}

		attackTarget = null;
		interactionTarget = null;
		giveTarget = null;

		GoToState(StateHandler.State.Playing);

		if (toy is Beachball ball) {
			animancer.Play(ball.playAnim);

			yield return new WaitForSeconds(ball.duration);
		} else if (toy is JackInTheBox box) {
			animancer.Play(box.playAnim);

			yield return new WaitForSeconds(box.duration);
		} else if (toy is MonsterWheelz wheelz) {
			float duration = wheelz.duration;

			while (duration > 0) {
				if (stateHandler.currentState == StateHandler.State.Dying) {
					yield break;
				}

				Node toNode = node.freeNeighbours.ToArray()[Random.Range(0, node.freeNeighbours.Count - 1)];

				AnimationClip toAnim = node.neighbourSet.right == toNode ? wheelz.rightAnim
					: node.neighbourSet.left == toNode ? wheelz.leftAnim
					: node.neighbourSet.up == toNode ? wheelz.upAnim
					: node.neighbourSet.down == toNode ? wheelz.downAnim
					: node.neighbourSet.upLeft == toNode ? wheelz.upLeftAnim
					: node.neighbourSet.upRight == toNode ? wheelz.upRightAnim
					: node.neighbourSet.downLeft == toNode ? wheelz.downLeftAnim
					: wheelz.downRightAnim;

				RandomMove(toNode, toAnim);

				yield return new WaitForSeconds(0.6f);

				duration -= 0.6f;
			}
		}

		if (ai != null) {
			ai.enabled = true;
		}
	}

	private void RandomMove(Node toNode, AnimationClip anim) {
		travelGoal = toNode;

		// Manually to kick off movement
		between = true;
		next = travelGoal;

		FaceTowardsNode(next);
		animancer.Play(anim);
	}

	// --------------------------------------------------
	// Selection
	// --------------------------------------------------

	public void Select() {
		selected = true;
		gameManager.selectedGruntz.Add(this);
		selectionMarker.SetActive(true);
		gruntEntry.HighLight();
	}

	public void Deselect() {
		selected = false;
		gameManager.selectedGruntz.Remove(this);
		selectionMarker.SetActive(false);
		gruntEntry.HighLight(false);
	}

	public void ArrowMove() {
		next = travelGoal;
		FaceTowardsNode(next);
		forced = false;
	}

	public IEnumerator TryInteract() {
		if (committed) {
			yield break;
		}

		if (!InToolRange(interactionTarget.node)) {
			GoToState(StateHandler.State.Idle);

			yield break;
		}

		FaceTowardsNode(interactionTarget.node);

		// Out of stamina, waiting until it is regenerated
		if (statz.stamina < Statz.MAX_VALUE) {
			GoToState(StateHandler.State.Idle);

			yield break;
		}

		committed = true;

		if (interactionTarget is Rock rock) {
			yield return BreakRock(rock);
		}

		if (interactionTarget is BrickBlock brickBlock) {
			if (equippedTool is Gauntletz) {
				yield return BreakBlock(brickBlock);
			} else if (equippedTool is SpyGear) {
				yield return IdentifyBlock(brickBlock);
			} else if (equippedTool is BrickLayer) {
				yield return BuildBlock(brickBlock);
			}
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

		yield return new WaitForSeconds(toPlay.length * 2f);

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

	private IEnumerator BreakBlock(BrickBlock brickBlock) {
		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.interact);

		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length * 0.75f);

		bool breakGauntletz = brickBlock.topMostBrick.type == BrickType.Red;

		brickBlock.Break();

		yield return new WaitForSeconds(toPlay.length * 0.25f);

		if (breakGauntletz) {
			Addressables.LoadAssetAsync<BareHandz>("BareHandz").Completed += handle => {
				animationPack = handle.Result.animationPack;
				equippedTool = handle.Result;
				gruntEntry.SetTool("BareHandz");

				animancer.Play(AnimationPack.GetRandomClip(facingDirection, animationPack.idle));
			};
		}

		interactionTarget = null;
	}

	private IEnumerator IdentifyBlock(BrickBlock brickBlock) {
		List<BrickBlock> toReveal = FindObjectsByType<BrickBlock>(FindObjectsSortMode.None)
			.Where(bb => node.neighbours.Contains(bb.node))
			.ToList();

		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.interact);

		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length);

		brickBlock.Reveal();
		toReveal.ForEach(bb => bb.Reveal());

		interactionTarget = null;
	}

	private IEnumerator BuildBlock(BrickBlock brickBlock, BrickType brickType = BrickType.Brown) {
		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.interact);

		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length);

		brickBlock.BuildBrick(brickType);

		interactionTarget = null;
	}

	private IEnumerator SuckPuddle(GruntPuddle puddle) {
		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.interact);

		animancer.Play(toPlay);

		yield return new WaitForSeconds(0.5f);

		puddle.Disappear();

		yield return new WaitForSeconds(toPlay.length - 0.5f);

		gameManager.gooWell.Fill(puddle.gooAmount);

		interactionTarget = null;
	}

	public IEnumerator TryAttack() {
		if (committed) {
			yield break;
		}

		Debug.Log($"TryAttack: {displayName} attacking {attackTarget.displayName}");

		if (!attackTarget.enabled) {
			GoToState(StateHandler.State.Idle);

			yield break;
		}

		if (!InToolRange(attackTarget.node)) {
			GoToState(StateHandler.State.Idle);

			yield break;
		}

		// Out of stamina, waiting until it is regenerated
		if (statz.stamina < Statz.MAX_VALUE) {
			FaceTowardsNode(attackTarget.node);

			GoToState(StateHandler.State.HostileIdle);

			yield break;
		}

		if (attackTarget.between) {
			FaceTowardsNode(attackTarget.node);

			GoToState(StateHandler.State.HostileIdle);

			yield break;
		}

		FaceTowardsNode(attackTarget.node);

		committed = true;

		AnimationClip toPlay = AnimationPack.GetRandomClip(facingDirection, animationPack.attack);
		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length / 2);

		attackTarget.TakeDamage(equippedTool.damage, attacker: this);

		// Reflected damage dealt to self, when applicable
		if (attackTarget.damageReflectionPercentage > 0) {
			TakeDamage(equippedTool.damage * attackTarget.damageReflectionPercentage);
		}

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
			barz.staminaBar.Adjust(statz.stamina);

			onStaminaRegenerated.Invoke();

			if (attackTarget != null) {
				GoToState(StateHandler.State.Attacking);
			} else if (interactionTarget != null)
				GoToState(StateHandler.State.Interacting);
			else {
				GoToState(StateHandler.State.Idle);
			}

			if (TryGetComponent(out AI.AI ai)) {
				ai.enabled = true;
			}

			return;
		}

		barz.staminaBar.Adjust(++statz.stamina);

		if (team == 0) {
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
	public bool InToolRange(Node otherNode) {
		return Mathf.Abs(node.location2D.x - otherNode.location2D.x) <= equippedTool.range
			&& Mathf.Abs(node.location2D.y - otherNode.location2D.y) <= equippedTool.range;
	}

	/// <summary>
	/// Damage the grunt, adjusting its health bar as well.
	/// </summary>
	/// <param name="damage">The amount of damage to be dealt.</param>
	/// <param name="hazardDamage">Whether the damage is inflicted by a hazard.</param>
	/// <param name="attacker">The attacking Grunt</param>
	public void TakeDamage(float damage, bool hazardDamage = false, Grunt attacker = null) {
		damage = hazardDamage ? damage : damage * (1 - damageReductionPercentage);

		statz.health = Math.Clamp(statz.health - damage, 0, Statz.MAX_VALUE);
		barz.healthBar.Adjust(statz.health);

		if (team == 0) {
			gruntEntry.SetHealth(statz.health);
		}

		if (statz.health <= 0) {
			Die();
		} else if (attacker != null) {
			// Todo: Is this needed?
			onHit.Invoke();

			interactionTarget = null;
			attackTarget = attacker;

			GoToState(StateHandler.State.Attacking);
		}
	}

	public void SpikezDamage() {
		TakeDamage(2f, hazardDamage: true);
	}

	/// <summary>
	/// Kills the Grunt.
	/// </summary>
	public async void Die() {
		StopAllCoroutines();

		enabled = false;
		transform.position = node.transform.position;
		animancer.Stop();

		gameManager.selectedGruntz.Remove(this);
		gameManager.allGruntz.Remove(this);

		if (team == 0) {
			gruntEntry.Clear();
			gameManager.playerGruntz.Remove(this);
		} else {
			gameManager.dizgruntled.Remove(this);
		}

		GoToState(StateHandler.State.Dying);

		CancelInvoke(nameof(RegenerateStamina));
		DeactivateBarz();

		Addressables.InstantiateAsync($"GruntPuddle_{textSkinColor}", GameObject.Find("Puddlez").transform).Completed += handle => {
			GruntPuddle puddle = handle.Result.GetComponent<GruntPuddle>();
			puddle.GetComponent<SpriteRenderer>().sortingOrder = 7;
			puddle.transform.position = transform.position;
			puddle.Setup();
			puddle.Appear();
			gameManager.gridObjectz.Add(puddle);
			Debug.Log("Added puddle");
		};

		spriteRenderer.sortingLayerName = "AlwaysBottom";
		spriteRenderer.sortingOrder = 8;

		animancer.Play(animationPack.deathAnimation);
		await UniTask.WaitForSeconds(animationPack.deathAnimation.length);

		if (team == 0) {
			gruntEntry.Clear();
		}

		Level.instance.levelStatz.deathz++;
		Destroy(gameObject, 0.5f);
	}

	/// <summary>
	/// Kills the Grunt.
	/// </summary>
	/// <param name="toPlay">The death animation to play, defined on a case-by-case basis.</param>
	/// <param name="leavePuddle">Whether the Grunt should leave a puddle after death.</param>
	/// <param name="playBelow">Whether to play the death animation closer to the ground level.</param>
	public async void Die(AnimationClip toPlay, bool leavePuddle = true, bool playBelow = true) {
		StopAllCoroutines();

		enabled = false;
		transform.position = node.transform.position;
		animancer.Stop();

		gameManager.selectedGruntz.Remove(this);
		gameManager.allGruntz.Remove(this);

		if (team == 0) {
			gruntEntry.Clear();
			gameManager.playerGruntz.Remove(this);
		} else {
			gameManager.dizgruntled.Remove(this);
		}

		GoToState(StateHandler.State.Dying);

		CancelInvoke(nameof(RegenerateStamina));
		DeactivateBarz();

		if (leavePuddle) {
			Addressables.InstantiateAsync($"GruntPuddle_{textSkinColor}", GameObject.Find("Puddlez").transform).Completed += handle => {
				GruntPuddle puddle = handle.Result.GetComponent<GruntPuddle>();
				puddle.GetComponent<SpriteRenderer>().sortingOrder = 7;
				puddle.transform.position = transform.position;
				puddle.Setup();
				puddle.Appear();
				gameManager.gridObjectz.Add(puddle);
				Debug.Log("Added puddle");
			};
		}

		if (playBelow) {
			transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			spriteRenderer.sortingLayerName = "AlwaysBottom";
			spriteRenderer.sortingOrder = 8;
		}

		animancer.Stop();
		animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		spriteRenderer.enabled = false;
		gameManager.allGruntz.Remove(this);
		Level.instance.levelStatz.deathz++;
		Destroy(gameObject, 0.5f);
	}

	public async void Teleport(Transform destination) {
		enabled = false;

		Debug.Log("Sucked in anim");
		animancer.Play(AnimationManager.instance.gruntWarpEnterAnim);
		await UniTask.WaitForSeconds(AnimationManager.instance.gruntWarpEnterAnim.length);

		Camera.main.transform.position = new Vector3(destination.position.x, destination.position.y, -10);
		transform.position = destination.position;

		Debug.Log("Spat out anim");
		animancer.Play(AnimationManager.instance.gruntFallingEntranceAnim);
		await UniTask.WaitForSeconds(AnimationManager.instance.gruntFallingEntranceAnim.length);

		enabled = true;
		between = false;

		GoToState(StateHandler.State.Idle);
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
		spriteRenderer.sprite = previewSprite;

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
