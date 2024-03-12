using System.Linq;
using Animancer;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Objectz.Interfacez;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
public class RollingBall : MonoBehaviour, IAnimatable {
	[Header("Pathfinding")]
	public Vector2 Location2D => node.location2D;

	public Node node;

	public Node next;

	[Range(0, 5)]
	public float moveSpeed;

	public Direction direction;

	public AnimationClip rollAnim;
	public AnimationClip breakAnim;
	public AnimationClip sinkAnim;

	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }

	private void Start() {
		node = FindObjectsByType<Node>(FindObjectsSortMode.None)
			.First(n => n.location2D == new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)));

		Animancer.Play(rollAnim);
	}

	private void FixedUpdate() {
		ChangePosition();
	}

	private void ChangePosition() {
		Vector3 moveVector = (next.transform.position - transform.position).normalized;
		gameObject.transform.position += moveVector * (Time.deltaTime / moveSpeed);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out RollingBall otherBall)) {
			enabled = false;
			Animancer.Play(breakAnim);

			return;
		}
	}
}
}
