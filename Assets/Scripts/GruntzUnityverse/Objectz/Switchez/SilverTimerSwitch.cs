using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class SilverTimerSwitch : Switch {
	public List<SilverPyramid> pyramidz;

	public override void Setup() {
		pyramidz = transform.parent.GetComponentsInChildren<SilverPyramid>().ToList();
	}

	protected override IEnumerator OnTriggerEnter2D(Collider2D other) {
		yield return base.OnTriggerEnter2D(other);

		pyramidz.ForEach(pyramid => pyramid.Toggle());
	}
}
}
