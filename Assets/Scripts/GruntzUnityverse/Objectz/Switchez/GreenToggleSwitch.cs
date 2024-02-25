using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class GreenToggleSwitch : Switch {
	public List<GreenPyramid> pyramidz = new List<GreenPyramid>();

	public override void Setup() {
		pyramidz = transform.parent.GetComponentsInChildren<GreenPyramid>().ToList();
	}

	protected override IEnumerator OnTriggerEnter2D(Collider2D other) {
		yield return base.OnTriggerEnter2D(other);

		pyramidz.ForEach(pyramid => pyramid.Toggle());
	}
}
}
