using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
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

		yield return new WaitForSeconds(duration);

		Deactivate(targetGrunt);
		Destroy(gameObject);
	}

	protected abstract void Activate(Grunt targetGrunt);

	protected abstract void Deactivate(Grunt targetGrunt);
}
}
