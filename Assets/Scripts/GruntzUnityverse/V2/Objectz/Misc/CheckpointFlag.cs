using Animancer;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Misc {
public class CheckpointFlag : MonoBehaviour, IAnimatable {

	#region IAnimatable
	// --------------------------------------------------
	// IAnimatable
	// --------------------------------------------------
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	#endregion

	[SerializeField] private AnimationClip wavingAnim;

	public void PlayAnim() {
		Animancer.Play(wavingAnim);
	}
}
}
