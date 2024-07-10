using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Misc;
using GruntzUnityverse.Objectz.Pyramidz;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Checkpoint : MonoBehaviour {
	public List<CheckpointSwitch> switchez;

	public List<CheckpointPyramid> pyramidz;

	public List<Flag> flagz;

	private void Start() {
		switchez = GetComponentsInChildren<CheckpointSwitch>().ToList();
		pyramidz = GetComponentsInChildren<CheckpointPyramid>().ToList();
		flagz = GetComponentsInChildren<Flag>().ToList();
	}

	private void Update() {
		if (!switchez.TrueForAll(sw => sw.isPressed)) {
			return;
		}

		enabled = false;

		switchez.ForEach(sw => sw.Deactivate());
		pyramidz.ForEach(py => py.Deactivate());
		flagz.ForEach(flag => flag.PlayAnim());
	}
}
}
