using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class WarpSwitch : GridObject {
	public List<Warp> warpz;

	private void Start() {
		spriteRenderer.enabled = false;
	}

	public override void Setup() {
		base.Setup();

		// node.circleCollider2D.isTrigger = false;
		warpz = transform.parent.GetComponentsInChildren<Warp>().ToList();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt _)) {
			circleCollider2D.isTrigger = false;
			warpz.ForEach(warp => warp.Activate());
			
			GameManager.instance.gridObjectz.Remove(this);
			Destroy(gameObject, 1f);
		}
	}
}
}
