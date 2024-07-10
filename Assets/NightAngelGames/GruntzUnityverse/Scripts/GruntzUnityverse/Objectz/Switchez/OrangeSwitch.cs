using System.Collections.Generic;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class OrangeSwitch : Switch {
	public List<OrangePyramid> pyramidz;

	public List<OrangeSwitch> otherSwitchez;

	private void ToggleOnlyIfPressed() {
		if (isPressed) {
			Toggle();
		}
	}

	// --------------------------------------------------
	// Overrides
	// --------------------------------------------------

	protected override void Toggle(bool checkPressed = false) {
		base.Toggle(checkPressed);

		pyramidz.ForEach(pyramid => pyramid.Toggle());
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override void Start() {
		base.Start();

		pyramidz = GetSiblings<OrangePyramid>();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		otherSwitchez.ForEach(sw => sw.ToggleOnlyIfPressed());
	}
}
}
