using System;
using System.Collections.Generic;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
[Serializable]
public struct PyramidData {
	public SilverPyramid pyramid;
	public float delay;
	public float duration;
}

public class SilverTimerSwitch : Switch {
	public List<PyramidData> pyramidData;

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		foreach (PyramidData pd in pyramidData) {
			pd.pyramid.delay = pd.delay;
			pd.pyramid.duration = pd.duration;
			pd.pyramid.Toggle();
		}
	}
}
}
