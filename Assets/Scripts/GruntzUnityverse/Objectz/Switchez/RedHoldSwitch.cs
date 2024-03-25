using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class RedHoldSwitch : Switch {
	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		foreach (RedPyramid rp in GameManager.instance.redPyramidz) {
			rp.Toggle();
		}
	}

	protected override void OnTriggerExit2D(Collider2D other) {
		base.OnTriggerExit2D(other);

		foreach (RedPyramid rp in GameManager.instance.redPyramidz) {
			rp.Toggle();
		}
	}
}
}
