using System.Collections;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class YellowArrowToggleSwitch : Switch {
	public TwoWayArrow arrow;

	public override void Setup() {
		base.Setup();

		arrow = transform.parent.GetComponentInChildren<TwoWayArrow>();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		arrow.Toggle();
	}
}
}
