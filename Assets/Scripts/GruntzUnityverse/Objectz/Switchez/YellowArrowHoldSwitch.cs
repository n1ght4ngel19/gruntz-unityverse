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

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		arrowz.ForEach(ar => ar.Toggle());
	}

	protected override void OnTriggerExit2D(Collider2D other) {
		base.OnTriggerExit2D(other);

		arrowz.ForEach(ar => ar.Toggle());
	}
}
}
