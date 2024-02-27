using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class WarpSwitch : GridObject {
	public List<Warp> warpz;

	protected override void Start() {
		base.Start();

		warpz = transform.parent.GetComponentsInChildren<Warp>().ToList();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		circleCollider2D.isTrigger = false;
		warpz.ForEach(warp => warp.Activate());
	}
}
}
