using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class WarpSwitch : GridObject {
	[ReadOnly]
	public List<Warp> warpz;

	protected override void Start() {
		base.Start();

		warpz = GetSiblings<Warp>();
		spriteRenderer.enabled = false;
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		circleCollider2D.isTrigger = false;
		Deactivate();
		warpz.ForEach(warp => warp.Activate());

		Destroy(gameObject);
	}

	protected override void OnDestroy() {
		base.OnDestroy();

		gameManager.gridObjectz.Remove(this);
	}
}
}
