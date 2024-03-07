using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class YellowArrowHoldSwitch : Switch {
	public List<TwoWayArrow> arrowz;

	public override void Setup() {
		base.Setup();

		arrowz = transform.parent.GetComponentsInChildren<TwoWayArrow>().ToList();
	}

	protected override IEnumerator OnTriggerEnter2D(Collider2D other) {
		yield return base.OnTriggerEnter2D(other);

		arrowz.ForEach(ar => ar.Toggle());
	}

	protected override IEnumerator OnTriggerExit2D(Collider2D other) {
		yield return base.OnTriggerExit2D(other);

		arrowz.ForEach(ar => ar.Toggle());
	}
}
}
