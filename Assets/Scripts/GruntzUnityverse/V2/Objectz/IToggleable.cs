using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
public interface IToggleable : IAnimatable {
	AnimationClip ToggleOnAnim { get; set; }

	AnimationClip ToggleOffAnim { get; set; }

	void Toggle();

	void ToggleOn();

	void ToggleOff();
}
}
