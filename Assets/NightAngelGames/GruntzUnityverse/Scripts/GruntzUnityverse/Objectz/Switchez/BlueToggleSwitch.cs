using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Bridgez;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class BlueToggleSwitch : Switch {
	/// <summary>
	/// The Bridgez that this BlueSwitch controls.
	/// </summary>
	[BoxGroup("Switch Data")]
	[ReadOnly]
	public List<Bridge> bridgez;

	public void Start() {
		bridgez = transform.parent.GetComponentsInChildren<Bridge>().ToList();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		bridgez.ForEach(bridge => bridge.Toggle());
	}
}
}
