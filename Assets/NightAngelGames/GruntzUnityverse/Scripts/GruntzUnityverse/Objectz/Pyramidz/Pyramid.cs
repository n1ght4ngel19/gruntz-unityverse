using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
public abstract class Pyramid : GridObject {
	[BoxGroup("Pyramid Data")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip raiseAnim;

	[BoxGroup("Pyramid Data")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip lowerAnim;

	[BoxGroup("Pyramid Data")]
	[DisableIf(nameof(isInstance))]
	public AnimancerComponent animancer;

	protected Grunt gruntOnTop => node.grunt;

	public virtual void Toggle() {
		animancer.Play(isObstacle ? lowerAnim : raiseAnim);

		isObstacle = !isObstacle;
		node.isBlocked = isObstacle;
		node.hardCorner = isObstacle;
		spriteRenderer.sortingLayerName = isObstacle ? "HighObjectz" : "AlwaysBottom";

		if (gruntOnTop == null || !node.isBlocked || gruntOnTop.between) {
			return;
		}

		gruntOnTop.Die(AnimationManager.instance.explodeDeathAnimation, false, false);
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override void Reset() {
		GetComponent<CircleCollider2D>().excludeLayers = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI");
		GetComponent<CircleCollider2D>().includeLayers = LayerMask.GetMask("RollingBall", "Grunt");
	}

	protected override void OnValidate() {
		base.OnValidate();

		GetComponent<CircleCollider2D>().excludeLayers = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI");
		GetComponent<CircleCollider2D>().includeLayers = LayerMask.GetMask("RollingBall", "Grunt");
	}

	protected override void AssignNodeValues() {
		node.isBlocked = isObstacle;
		node.hardCorner = isObstacle;
	}

	protected override void Start() {
		base.Start();

		animancer = GetComponent<AnimancerComponent>();
	}
}
}
