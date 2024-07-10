using System.Collections.Generic;
using GruntzUnityverse.Objectz.Bridgez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class BlueHoldSwitch : Switch {
	/// <summary>
	/// The Bridgez that this BlueSwitch controls.
	/// </summary>
	public List<Bridge> bridgez;

	protected override void Start() {
		base.Start();

		bridgez = GetSiblings<Bridge>();
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
