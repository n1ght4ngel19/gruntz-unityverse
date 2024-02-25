using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class OrangeSwitch : Switch {
	public List<OrangePyramid> pyramidz;
	public List<OrangeSwitch> otherSwitchez;

	public override void Setup() {
		pyramidz = transform.parent.GetComponentsInChildren<OrangePyramid>()
			.ToList();

		otherSwitchez = transform.parent.GetComponentsInChildren<OrangeSwitch>()
			.Where(sw => sw != this)
			.ToList();
	}

	protected override IEnumerator OnTriggerEnter2D(Collider2D other) {
		yield return base.OnTriggerEnter2D(other);

		pyramidz.ForEach(pyramid => pyramid.Toggle());
		otherSwitchez.ForEach(sw => sw.Toggle());
	}
}
}
