using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class PurplePyramid : Pyramid {
	public List<PurpleSwitch> switchez;

	private void FixedUpdate() {
		if (!switchez.TrueForAll(sw => sw.IsPressed)) {
			return;
		}

		switchez.ForEach(sw => sw.DisableTrigger());
		Toggle();

		enabled = false;
	}

	public override void Setup() {
		base.Setup();

		switchez = transform.parent.GetComponentsInChildren<PurpleSwitch>().ToList();
	}
}
}
