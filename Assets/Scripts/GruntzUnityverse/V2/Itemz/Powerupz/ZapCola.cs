using GruntzUnityverse.V2.Actorz;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Powerupz {
public class ZapCola : LevelPowerup {
	[Range(5, 20)]
	public int healAmount;

	protected override void Activate(Grunt targetGrunt) {
		targetGrunt.statz.health += healAmount;
		targetGrunt.barz.healthBar.Adjust(targetGrunt.statz.health);
	}

	protected override void DeActivate(Grunt targetGrunt) {
		// Nothing to do here.
	}
}
}
