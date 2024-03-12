using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class SilverPyramid : Pyramid {
	public int delay;
	public int duration;

	public override async void Toggle() {
		if (duration <= 0) {
			return;
		}

		StartCoroutine(WaitingToggle());
	}

	private IEnumerator WaitingToggle() {
		yield return new WaitForSeconds(delay);

		if (isObstacle) {
			ToggleOff();
		} else {
			ToggleOn();
		}

		node.isBlocked = isObstacle;

		yield return new WaitForSeconds(duration);

		if (isObstacle) {
			ToggleOff();
		} else {
			ToggleOn();
		}

		node.isBlocked = isObstacle;
	}
}
}
