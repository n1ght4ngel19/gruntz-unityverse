using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Column : GridObject {
	private void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		grunt.Die(AnimationManager.Instance.explodeDeathAnimation, leavePuddle: false, playBelow: false);
	}
}
}
