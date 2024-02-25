using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Powerupz {
public class ZapCola : LevelPowerup {
	[Range(5, 20)]
	public int healAmount;

	protected override void Activate(Grunt targetGrunt) {
		targetGrunt.statz.health += healAmount;
		targetGrunt.barz.healthBar.Adjust(targetGrunt.statz.health);
	}

	protected override void Deactivate(Grunt targetGrunt) {
		// Nothing to do here.
	}
}
}
