using System.Collections;
using System.Linq;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Actorz.BehaviourManagement;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Interfacez;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
public abstract class LevelItem : MonoBehaviour, IAnimatable {
	/// <summary>
	/// The name of the item.
	/// </summary>
	public string itemName;

	/// <summary>
	/// The animation clip used to display the item on the level.
	/// </summary>
	public AnimationClip rotatingAnim;

	/// <summary>
	/// The animation clip used by a Grunt picking up the item.
	/// </summary>
	public AnimationClip pickupAnim;

	/// <summary>
	/// The animation pack set for the Grunt picking up the item.
	/// </summary>
	public AnimationPack animationPack;

	public Node node;
	public Vector2Int location2D;

	// --------------------------------------------------
	// IAnimatable
	// --------------------------------------------------

	#region IAnimatable
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	#endregion

	protected virtual void Start() {
		node = Level.Instance.levelNodes.First(n => n.location2D == location2D);
		Animator = GetComponent<Animator>();
		Animancer = GetComponent<AnimancerComponent>();

		Animancer.Play(rotatingAnim);
	}

	/// <summary>
	/// Called when a <see cref="Grunt"/> picks up this item.
	/// (Provides no implementation since child classes need to modify
	/// different properties of the Grunt picking up the item.)
	/// </summary>
	protected virtual IEnumerator Pickup(Grunt targetGrunt) {
		targetGrunt.intent = Intent.ToIdle;
		targetGrunt.EvaluateState(whenFalse: targetGrunt.betweenNodes);

		yield return new WaitUntil(() => !targetGrunt.betweenNodes);

		targetGrunt.Animancer.Play(pickupAnim);

		yield return new WaitForSeconds(pickupAnim.length);
	}

	/// <summary>
	/// Called when an <see cref="Grunt"/> moves onto this Item.
	/// Other than RollingBallz, only Gruntz have the ability to collide with Items.
	/// This is checked inside the method, so there is no need to expose this method to child classes.
	/// </summary>
	/// <param name="other">The collider of the colliding object.</param>
	private void OnTriggerEnter2D(Collider2D other) {
		Grunt grunt = other.gameObject.GetComponent<Grunt>();

		if (grunt == null) {
			return;
		}

		StartCoroutine(Pickup(grunt));
	}
}
}
