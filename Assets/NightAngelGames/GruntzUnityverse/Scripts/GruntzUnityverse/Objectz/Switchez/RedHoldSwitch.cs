using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class RedHoldSwitch : Switch {
	public List<RedPyramid> redPyramidz;

	// --------------------------------------------------
	// Overrides
	// --------------------------------------------------

	protected override void Toggle(bool checkPressed = false) {
		base.Toggle(checkPressed);

		redPyramidz.ForEach(rp => rp.Toggle());
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override void Start() {
		base.Start();

		redPyramidz = FindObjectsByType<RedPyramid>(FindObjectsSortMode.None).ToList();
	}
}
}
