using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class CheckpointPyramid : Pyramid {
	public List<CheckpointSwitch> switchez;

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

		switchez = transform.parent.GetComponentsInChildren<CheckpointSwitch>().ToList();

		if (switchez.Count == 0) {
			enabled = false;
		}
	}
}
}
