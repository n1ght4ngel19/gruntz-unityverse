using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class PurplePyramid : Pyramid {
	public List<PurpleSwitch> switchez;
	private bool _toggled;

	public void Start() {
		switchez = transform.parent.GetComponentsInChildren<PurpleSwitch>().ToList();
	}

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
}
}
