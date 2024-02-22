using UnityEngine;

namespace GruntzUnityverse.Objectz.Interfacez {
public interface IToggleable : IAnimatable {
	AnimationClip ToggleOnAnim { get; set; }

	AnimationClip ToggleOffAnim { get; set; }

	void Toggle();

	void ToggleOn();

	void ToggleOff();
}
}
