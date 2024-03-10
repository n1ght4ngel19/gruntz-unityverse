using Animancer;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;

namespace GruntzUnityverse.Core {
public class Cursor : MonoBehaviour, IAnimatable {
	public SpriteRenderer spriteRenderer;
	public AnimationClip toPlay;

	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }

	private void FixedUpdate() {
		transform.position = new Vector3(
			Camera.main.ScreenToWorldPoint(Input.mousePosition).x + 0.5f,
			Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f,
			transform.position.z
		);

		Animancer.Play(toPlay);
	}

	public void SwapCursor(AnimationClip newCursor) {
		toPlay = newCursor;
	}
}
}
