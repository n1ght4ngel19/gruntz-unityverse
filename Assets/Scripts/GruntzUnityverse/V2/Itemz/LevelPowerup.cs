﻿using System.Collections;
using GruntzUnityverse.V2.Actorz;
using GruntzUnityverse.V2.Core;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz {
/// <summary>
/// The base class for Powerupz.
/// </summary>
public abstract class LevelPowerup : LevelItem {
	/// <summary>
	/// The duration of the Powerup. A duration of 0 means the Powerup has an instantaneous effect.
	/// </summary>
	public float duration;

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		yield return base.Pickup(targetGrunt);

		Activate(targetGrunt);

		Level.Instance.levelStatz.powerupzCollected++;

		enabled = false;

		yield return new WaitForSeconds(duration);

		DeActivate(targetGrunt);
		Destroy(gameObject);
	}

	protected abstract void Activate(Grunt targetGrunt);

	protected abstract void DeActivate(Grunt targetGrunt);
}
}
