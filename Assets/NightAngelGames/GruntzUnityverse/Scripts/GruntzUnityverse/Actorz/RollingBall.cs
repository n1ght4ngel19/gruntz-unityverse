using System.Collections;
using System.Linq;
using Animancer;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Pathfinding;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
public class RollingBall : MonoBehaviour {
	[BoxGroup("Pathfinding")]
	[ReadOnly]
	public Node node;

	[BoxGroup("Pathfinding")]
	[ReadOnly]
	public Node next;

	[BoxGroup("Ball Data")]
	[Range(0, 5)]
	public float moveSpeed;

	[BoxGroup("Ball Data")]
	public Direction direction;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip rollAnimUp;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip rollAnimDown;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip rollAnimLeft;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip rollAnimRight;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip breakAnim;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip sinkAnim;
	
	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip currentRollAnim;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimancerComponent animancer;

	public bool isInstance => gameObject.scene.name != null;

	public void Setup() {
		node = FindObjectsByType<Node>(FindObjectsSortMode.None)
			.First(n => n.location2D == new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));

		animancer ??= GetComponent<AnimancerComponent>();
	}

	private void FixedUpdate() {
		ChangePosition();
		animancer.Play(currentRollAnim);
	}

	private void ChangePosition() {
		Vector3 moveVector = (next.transform.position - node.transform.position);
		gameObject.transform.position += moveVector * (Time.fixedDeltaTime / moveSpeed);
	}

	private IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out RollingBall ball) || !ball.enabled) {
			yield break;
		}

		enabled = false;

		transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		GetComponent<SpriteRenderer>().sortingOrder = 4;

		animancer.Play(breakAnim);

		yield return new WaitForSeconds(breakAnim.length);

		transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		GetComponent<SpriteRenderer>().sortingLayerName = "AlwaysBottom";
		GetComponent<SpriteRenderer>().sortingOrder = 4;
	}
}
}
