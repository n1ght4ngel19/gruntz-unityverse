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

		if (actAsObstacle) {
			ToggleOff();
		} else {
			ToggleOn();
		}

		node.isBlocked = actAsObstacle;

		yield return new WaitForSeconds(duration);

		if (actAsObstacle) {
			ToggleOff();
		} else {
			ToggleOn();
		}

		node.isBlocked = actAsObstacle;
	}
}
}
