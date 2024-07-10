using System.Collections.Generic;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class BlackOneTimeSwitch : Switch {
	public List<BlackPyramid> pyramidz;

	protected override void Start() {
		base.Start();

		pyramidz = GetSiblings<BlackPyramid>();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		pyramidz.ForEach(pyramid => pyramid.Toggle());

		Deactivate();
	}
}
}
