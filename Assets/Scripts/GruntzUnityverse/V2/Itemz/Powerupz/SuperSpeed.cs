﻿using GruntzUnityverse.V2.Actorz;

namespace GruntzUnityverse.V2.Itemz.Powerupz {
public class SuperSpeed : LevelPowerup {
	/// <summary>
	/// The multiplier to apply to the grunt's base move speed.
	/// </summary>
	public float speedMultiplier;

	protected override void Activate(Grunt targetGrunt) {
		// float originalGruntSpeed = targetGrunt.statz.moveSpeed;

		// targetGrunt.statz.moveSpeed *= speedMultiplier;
	}

	protected override void DeActivate(Grunt targetGrunt) {
		// targetGrunt.statz.moveSpeed originalGruntSpeed;
	}
}
}
