using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class RedToggleSwitch : Switch {
	public override void Toggle() {
		base.Toggle();

		FindObjectsByType<RedPyramid>(FindObjectsSortMode.None)
			.ToList()
			.ForEach(pyramid => pyramid.Toggle());
	}

	protected override void OnTriggerExit2D(Collider2D other) {
		base.Toggle();
	}
}
}
