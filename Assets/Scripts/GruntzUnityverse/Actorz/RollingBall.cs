using System.Collections;
using System.Linq;
using Animancer;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
public class RollingBall : MonoBehaviour {
	[Header("Pathfinding")]
	public Node node;

	public Node next;

	[Range(0, 5)]
	public float moveSpeed;

	public Direction direction;

	public AnimationClip rollAnimUp;
	public AnimationClip rollAnimDown;
	public AnimationClip rollAnimLeft;
	public AnimationClip rollAnimRight;
	public AnimationClip breakAnim;
	public AnimationClip sinkAnim;

	public AnimancerComponent animancer;

	public void Setup() {
		node = FindObjectsByType<Node>(FindObjectsSortMode.None)
			.First(n => n.location2D == new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));

		animancer ??= GetComponent<AnimancerComponent>();
	}

	private void Start() {
		animancer.Play(rollAnimUp);
	}

	private void FixedUpdate() {
		ChangePosition();
	}

	private void ChangePosition() {
		Vector3 moveVector = (next.transform.position - node.transform.position);
		gameObject.transform.position += moveVector * (Time.fixedDeltaTime / moveSpeed);
	}

	private IEnumerator OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out RollingBall _)) {
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
}
