using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class CheckpointPyramid : Pyramid {
	public List<CheckpointSwitch> switchez;

	private void Start() {
		switchez = transform.parent.GetComponentsInChildren<CheckpointSwitch>().ToList();
	}

	private void FixedUpdate() {
		if (!switchez.TrueForAll(sw => sw.isPressed)) {
			return;
		}

		switchez.ForEach(sw => sw.DisableTrigger());
		Toggle();

		enabled = false;
	}
}
}
