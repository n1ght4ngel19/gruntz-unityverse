using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class YellowArrowToggleSwitch : Switch {
	public List<TwoWayArrow> arrowz;

	public override void Setup() {
		base.Setup();

		arrowz = transform.parent.GetComponentsInChildren<TwoWayArrow>().ToList();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		arrowz.ForEach(ar => ar.Toggle());
	}
}
}
