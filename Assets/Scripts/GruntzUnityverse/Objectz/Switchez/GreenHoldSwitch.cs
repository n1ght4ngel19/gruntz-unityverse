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

	protected override void OnTriggerExit2D(Collider2D other) {
		base.OnTriggerExit2D(other);

		pyramidz.ForEach(pyramid => pyramid.Toggle());
	}
}
}
