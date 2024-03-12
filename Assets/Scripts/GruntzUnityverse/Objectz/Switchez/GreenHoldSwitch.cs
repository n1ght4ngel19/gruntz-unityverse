using System.Collections;
using System.Collections.Generic;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class GreenHoldSwitch : Switch {
	public List<GreenPyramid> pyramidz;

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		pyramidz.ForEach(pyramid => pyramid.Toggle());
	}

	protected override IEnumerator OnTriggerExit2D(Collider2D other) {
		yield return base.OnTriggerExit2D(other);

		pyramidz.ForEach(pyramid => pyramid.Toggle());
	}
}
}
