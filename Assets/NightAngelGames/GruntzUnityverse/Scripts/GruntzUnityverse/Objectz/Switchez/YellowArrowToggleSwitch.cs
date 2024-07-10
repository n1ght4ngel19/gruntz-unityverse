using System.Collections.Generic;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class YellowArrowToggleSwitch : Switch {
	public List<TwoWayArrow> arrowz;

	protected override void Start() {
		base.Start();

		arrowz = GetSiblings<TwoWayArrow>();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		arrowz.ForEach(arrow => arrow.Toggle());
	}
}
}
