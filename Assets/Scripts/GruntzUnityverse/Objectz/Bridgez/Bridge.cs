using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class Bridge : GridObject, IToggleable {
	public bool isDeathBridge;

	public override void Setup() {
		base.Setup();

		node.isWater = actAsWater;
	}

	#region IToggleable
	// --------------------------------------------------
	// IToggleable
	// --------------------------------------------------
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	[field: SerializeField] public AnimationClip ToggleOnAnim { get; set; }
	[field: SerializeField] public AnimationClip ToggleOffAnim { get; set; }

	public void Toggle() {
		if (node.isWater) {
			ToggleOn();
		} else {
			ToggleOff();
		}
	}

	public async void ToggleOn() {
		await Animancer.Play(ToggleOnAnim);

		node.isWater = false;
	}

	public async void ToggleOff() {
		await Animancer.Play(ToggleOffAnim);

		node.isWater = true;
	}
	#endregion

}
}
