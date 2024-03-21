using Animancer;
using UnityEngine;

namespace GruntzUnityverse.Core {
public class GameCursor : MonoBehaviour {
	public SpriteRenderer spriteRenderer;
	public AnimationClip toPlay;

	public AnimancerComponent animancer;

	private void Awake() {
		animancer = GetComponent<AnimancerComponent>();
	}

	private void Update() {
		transform.localScale = new Vector3(
			Camera.main.orthographicSize / 10f,
			Camera.main.orthographicSize / 10f,
			1f
		);

		transform.position = new Vector3(
			Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
			Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
			transform.position.z
		);

		animancer.Play(toPlay);
	}

	public void SwapCursor(AnimationClip newCursor) {
		toPlay = newCursor;
	}
}
}
