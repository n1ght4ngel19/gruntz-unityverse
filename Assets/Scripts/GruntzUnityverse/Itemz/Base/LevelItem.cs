using System.Collections;
using System.Linq;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Actorz.BehaviourManagement;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Interfacez;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
public abstract class LevelItem : MonoBehaviour, IAnimatable {
	/// <summary>
	/// The display name of the item.
	/// </summary>
	public string displayName;

	/// <summary>
	/// The code-name of the item.
	/// </summary>
	public string codeName;

	/// <summary>
	/// The animation clip used to display the item on the level.
	/// </summary>
	public AnimationClip rotatingAnim;

	/// <summary>
	/// The animation clip used by a Grunt picking up the item.
	/// </summary>
	public AnimationClip pickupAnim;

	public bool hideUnderMound;

	// --------------------------------------------------
	// IAnimatable
	// --------------------------------------------------

	#region IAnimatable
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	#endregion

	public void Setup() {
		Rock rock = FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (rock != null) {
			rock.HeldItem = this;
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<CircleCollider2D>().isTrigger = false;

			return;
		}

		Hole hole = FindObjectsByType<Hole>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (hole != null && hideUnderMound) {
			hole.HeldItem = this;
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<CircleCollider2D>().isTrigger = false;

			return;
		}
	}

	protected virtual void Start() {
		Animancer.Play(rotatingAnim);
	}

	/// <summary>
	/// Called when a <see cref="Grunt"/> picks up this item.
	/// (Provides no implementation since child classes need to modify
	/// different properties of the Grunt picking up the item.)
	/// </summary>
	protected virtual IEnumerator Pickup(Grunt targetGrunt) {
		targetGrunt.enabled = false;

		targetGrunt.Animancer.Play(pickupAnim);

		yield return new WaitForSeconds(pickupAnim.length);

		targetGrunt.enabled = true;
		targetGrunt.intent = Intent.ToIdle; // --> ToMove
		targetGrunt.EvaluateState();
	}

	/// <summary>
	/// Called when an <see cref="Grunt"/> moves onto this Item.
	/// Other than RollingBallz, only Gruntz have the ability to collide with Items.
	/// This is checked inside the method, so there is no need to expose this method to child classes.
	/// </summary>
	/// <param name="other">The collider of the colliding object.</param>
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<CircleCollider2D>().isTrigger = false;

			grunt.travelGoal = grunt.node;
			grunt.intent = Intent.ToStop;
			grunt.state = State.Stopped;
			grunt.EvaluateState(whenFalse: grunt.BetweenNodes);

			StartCoroutine(Pickup(grunt));
		}
	}

	public void LocalizeName(string newDisplayName) {
		displayName = newDisplayName;
	}
}
}
