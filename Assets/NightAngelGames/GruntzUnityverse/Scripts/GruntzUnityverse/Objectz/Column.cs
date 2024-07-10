using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Column : GridObject {
	protected override void AssignNodeValues() {
		node.isBlocked = true;
		node.hardCorner = true;
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			grunt.Die(AnimationManager.instance.explodeDeathAnimation, leavePuddle: false, playBelow: false);
		}
	}
}
}
