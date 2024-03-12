using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class BlackOneTimeSwitch : Switch {
	public List<BlackPyramid> pyramidz;

	public override void Setup() {
		base.Setup();

		pyramidz = transform.parent.GetComponentsInChildren<BlackPyramid>().ToList();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		DisableTrigger();
		pyramidz.ForEach(pyramid => pyramid.Toggle());
	}

	protected override void OnTriggerExit2D(Collider2D other) { }
}
}
