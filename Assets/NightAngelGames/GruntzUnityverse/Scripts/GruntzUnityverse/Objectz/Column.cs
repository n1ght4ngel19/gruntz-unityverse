using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Column : GridObject {
	public override void Setup() {
		base.Setup();

		node.isBlocked = true;
		node.hardCorner = true;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			grunt.Die(AnimationManager.instance.explodeDeathAnimation, leavePuddle: false, playBelow: false);
		}
	}
}
}
