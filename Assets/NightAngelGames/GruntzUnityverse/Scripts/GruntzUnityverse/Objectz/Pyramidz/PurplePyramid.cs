using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class PurplePyramid : Pyramid {
	public List<PurpleSwitch> switchez;
	private bool _toggled;

	private void FixedUpdate() {
		if (switchez.TrueForAll(sw => sw.isPressed) && !_toggled) {
			_toggled = true;
			Toggle();

			return;
		}

		if (!switchez.TrueForAll(sw => sw.isPressed) && _toggled) {
			_toggled = false;
			Toggle();
		}
	}

	public override void Setup() {
		base.Setup();

		switchez = transform.parent.GetComponentsInChildren<PurpleSwitch>().ToList();
	}
}
}
