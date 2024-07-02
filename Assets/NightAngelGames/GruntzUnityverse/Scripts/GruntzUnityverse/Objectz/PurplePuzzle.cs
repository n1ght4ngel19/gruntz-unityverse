using System.Collections.Generic;
using GruntzUnityverse.Objectz.Pyramidz;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class PurplePuzzle : MonoBehaviour {
	public List<PurpleSwitch> switchez;
	public List<PurplePyramid> pyramidz;

	private void Update() {
		if (!switchez.TrueForAll(sw => sw.isPressed)) {
			return;
		}

		switchez.ForEach(sw => sw.DisableTrigger());
		pyramidz.ForEach(pyramid => pyramid.Toggle());

		enabled = false;
	}
}
}
