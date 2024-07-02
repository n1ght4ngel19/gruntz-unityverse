using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class OrangeSwitch : Switch {
	public List<OrangePyramid> pyramidz;
	public List<OrangeSwitch> otherSwitchez;

	public override void Setup() {
		base.Setup();

		pyramidz = transform.parent.GetComponentsInChildren<OrangePyramid>()
			.ToList();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt _) && !other.TryGetComponent(out RollingBall _)) {
			return;
		}

		if (!isPressed) {
			Toggle();

			pyramidz.ForEach(pyramid => pyramid.Toggle());

			otherSwitchez
				.Where(sw => sw.isPressed)
				.ToList()
				.ForEach(
					sw1 => {
						sw1.Toggle();
						sw1.pyramidz.ForEach(py => py.Toggle());
					}
				);
		}
	}

	protected override void OnTriggerExit2D(Collider2D other) { }
}
}
