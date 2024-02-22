using Animancer;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
public abstract class Pyramid : GridObject, IToggleable {

	#region IToggleable
	// --------------------------------------------------
	// IToggleable
	// --------------------------------------------------
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	[field: SerializeField] public AnimationClip ToggleOnAnim { get; set; }
	[field: SerializeField] public AnimationClip ToggleOffAnim { get; set; }

	public virtual void Toggle() {
		if (actAsObstacle) {
			ToggleOff();
		} else {
			ToggleOn();
		}

		node.isBlocked = actAsObstacle;
	}

	public virtual void ToggleOn() {
		actAsObstacle = true;
		Animancer.Play(ToggleOnAnim);
	}

	public virtual void ToggleOff() {
		actAsObstacle = false;
		Animancer.Play(ToggleOffAnim);
	}
	#endregion

}
}
