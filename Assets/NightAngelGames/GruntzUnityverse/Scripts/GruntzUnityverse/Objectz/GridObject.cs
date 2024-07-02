using System;
using System.Linq;
using Animancer;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utils;
using HierarchyIcons;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public abstract class GridObject : MonoBehaviour {
	public bool hideComponents;

	/// <summary>
	/// Represents whether this GridObject behaves like an obstacle.
	/// </summary>
	[BoxGroup("Flagz")]
	[DisableIf((nameof(isInstance)))]
	public bool isObstacle;

	[BoxGroup("Flagz")]
	[DisableIf((nameof(isInstance)))]
	public bool isWater;

	[BoxGroup("Flagz")]
	[DisableIf((nameof(isInstance)))]
	public bool isFire;

	[BoxGroup("Flagz")]
	[DisableIf((nameof(isInstance)))]
	public bool isVoid;

	/// <summary>
	/// The location of this GridObject in 2D space.
	/// </summary>
	[BoxGroup("GridObject Data")]
	[ReadOnly]
	public Vector2Int location2D;

	/// <summary>
	/// The <see cref="Node"/> that this GridObject currently occupies.
	/// </summary>
	[BoxGroup("GridObject Data")]
	[ReadOnly]
	public Node node;

	/// <summary>
	/// The SpriteRenderer of this GridObject.
	/// </summary>
	[BoxGroup("GridObject Data")]
	[Required]
	[HideIf((nameof(isInstance)))]
	public SpriteRenderer spriteRenderer;

	/// <summary>
	/// The collider used for checking interactions with this GridObject.
	/// </summary>
	[Required]
	[BoxGroup("GridObject Data")]
	[HideIf((nameof(isInstance)))]
	public CircleCollider2D circleCollider2D;

	public bool isInstance => gameObject.scene.name != null;

	public virtual void Setup() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		circleCollider2D = GetComponent<CircleCollider2D>();
		location2D = Vector2Int.RoundToInt(transform.position);

		node = FindObjectsByType<Node>(FindObjectsSortMode.None)
			.First(n => Vector2Int.RoundToInt(n.transform.position) == Vector2Int.RoundToInt(transform.position));

		node.isBlocked = isObstacle || node.isBlocked;
		node.isWater = isWater || node.isWater;
		node.isFire = isFire || node.isFire;
		node.isVoid = isVoid || node.isVoid;
	}

	public virtual void Dismantle() {
		node.isBlocked = isObstacle;
		node.isWater = isWater;
		node.isFire = isFire;
		node.isVoid = isVoid;
	}

	/// <summary>
	/// Checks whether this GridObject is diagonal to the given GridObject.
	/// </summary>
	/// <param name="other">The <see cref="GridObject"/> to check.</param>
	/// <returns>A boolean indicating whether this GridObject is diagonal to <see cref="other"/> or not.</returns>
	protected bool IsDiagonalTo(GridObject other) {
		return Math.Abs(other.location2D.x - location2D.x) != 0
			&& Math.Abs(other.location2D.y - location2D.y) != 0;
	}

	/// <summary>
	/// Disables the ability of this GridObject to collide with other objects (with a <see cref="Rigidbody2D"/>).
	/// This is needed since OnTriggerExit2D can be called even after the object or its
	/// collider has been destroyed. If we disable the collider's ability to be triggered,
	/// we can check it before inside OnTriggerEnter2D and OnTriggerExit2D.
	/// </summary>
	public void DisableTrigger() {
		circleCollider2D.isTrigger = false;
	}

	protected bool SceneLoaded() {
		return gameObject.scene.isLoaded;
	}

	/// <summary>
	/// Reverts the effects this GridObject has on the Node it is occupying.
	/// Can be overridden to add additional effects specific to child classes.
	/// </summary>
	protected virtual void OnDestroy() {
		// node.isBlocked = isObstacle ? false : node.isBlocked;
	}

	#if UNITY_EDITOR
	private void OnValidate() {
		HideFlags newHideFlags = hideComponents ? HideFlags.HideInInspector : HideFlags.None;

		spriteRenderer.hideFlags = newHideFlags;
		circleCollider2D.hideFlags = newHideFlags;

		GetComponent<HierarchyIcon>().hideFlags = isInstance ? HideFlags.None : HideFlags.HideInInspector;

		if (TryGetComponent(out AnimancerComponent animancer)) {
			animancer.hideFlags = newHideFlags;
			animancer.Animator.hideFlags = newHideFlags;
		}

		if (TryGetComponent(out TrimName trimName)) {
			trimName.hideFlags = newHideFlags;
		}

		if (TryGetComponent(out HierarchyIcon hierarchyIcon)) {
			hierarchyIcon.hideFlags = isInstance ? HideFlags.None : HideFlags.HideInInspector;
		}
	}

	private void OnDrawGizmosSelected() {
		transform.hideFlags = HideFlags.HideInInspector;

		HideFlags newHideFlags = hideComponents ? HideFlags.HideInInspector : HideFlags.None;

		spriteRenderer.hideFlags = newHideFlags;
		circleCollider2D.hideFlags = newHideFlags;

		GetComponent<HierarchyIcon>().hideFlags = isInstance ? HideFlags.None : HideFlags.HideInInspector;

		if (TryGetComponent(out AnimancerComponent animancer)) {
			animancer.hideFlags = newHideFlags;
			animancer.Animator.hideFlags = newHideFlags;
		}

		if (TryGetComponent(out TrimName trimName)) {
			trimName.hideFlags = newHideFlags;
		}

		if (TryGetComponent(out HierarchyIcon hierarchyIcon)) {
			hierarchyIcon.hideFlags = isInstance ? HideFlags.None : HideFlags.HideInInspector;
		}
	}
	#endif
}
}
