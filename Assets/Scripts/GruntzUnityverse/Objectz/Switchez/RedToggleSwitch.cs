using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class RedToggleSwitch : Switch {
	public override void Toggle() {
		base.Toggle();

		foreach (RedPyramid rp in GameManager.instance.redPyramidz) {
			rp.Toggle();
		}
	}

	protected override void OnTriggerExit2D(Collider2D other) {
		base.Toggle();
	}
}
}
