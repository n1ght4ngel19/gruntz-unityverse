using System.Collections.Generic;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class PurplePyramid : Pyramid {
	public List<PurpleSwitch> switchez;

	public bool toggled;

	protected override void Start() {
		base.Start();

		switchez = GetSiblings<PurpleSwitch>();
	}

	protected override void Update() {
		if (switchez.TrueForAll(sw => sw.isPressed) && !toggled) {
			toggled = true;

			Toggle();
		}

		if (!switchez.TrueForAll(sw => sw.isPressed) && toggled) {
			toggled = false;

			Toggle();
		}
	}
}
}
