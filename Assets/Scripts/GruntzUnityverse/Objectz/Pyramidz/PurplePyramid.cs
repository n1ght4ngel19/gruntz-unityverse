using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class PurplePyramid : Pyramid {
	public List<PurpleSwitch> switchez;

	public override void Setup() {
		base.Setup();

		switchez = transform.parent.GetComponentsInChildren<PurpleSwitch>()
			.ToList();
	}

	private void FixedUpdate() {
		if (!switchez.All(sw => sw.IsPressed)) {
			return;
		}

		enabled = false;
		Toggle();
	}
}
}
