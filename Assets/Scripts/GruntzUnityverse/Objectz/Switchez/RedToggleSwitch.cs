using System.Collections;
using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class RedToggleSwitch : Switch {
	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		FindObjectsByType<RedPyramid>(FindObjectsSortMode.None)
			.ToList()
			.ForEach(pyramid => pyramid.Toggle());
	}
}
}
