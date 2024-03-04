using System.Collections;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class YellowArrowHoldSwitch : Switch {
	public TwoWayArrow arrow;

	public override void Setup() {
		base.Setup();

		arrow = GetComponentInChildren<TwoWayArrow>();
	}

	protected override IEnumerator OnTriggerEnter2D(Collider2D other) {
		yield return base.OnTriggerEnter2D(other);

		arrow.Toggle();
	}

	protected override IEnumerator OnTriggerExit2D(Collider2D other) {
		yield return base.OnTriggerExit2D(other);

		arrow.Toggle();
	}
}
}
