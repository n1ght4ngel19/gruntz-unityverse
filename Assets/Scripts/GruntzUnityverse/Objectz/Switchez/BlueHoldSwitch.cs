using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Bridgez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class BlueHoldSwitch : Switch {
	/// <summary>
	/// The Bridgez that this BlueSwitch controls.
	/// </summary>
	public List<Bridge> bridgez;

	public override void Setup() {
		base.Setup();

		bridgez = transform.parent.GetComponentsInChildren<Bridge>().ToList();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		bridgez.ForEach(bridge => bridge.Toggle());
	}

	protected override void OnTriggerExit2D(Collider2D other) {
		base.OnTriggerExit2D(other);

		bridgez.ForEach(bridge => bridge.Toggle());
	}
}
}
