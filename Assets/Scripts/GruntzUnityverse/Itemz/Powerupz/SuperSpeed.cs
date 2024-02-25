using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;

namespace GruntzUnityverse.Itemz.Powerupz {
public class SuperSpeed : LevelPowerup {
	/// <summary>
	/// The multiplier to apply to the grunt's base move speed.
	/// </summary>
	public float speedMultiplier;

	protected override void Activate(Grunt targetGrunt) {
		// float originalGruntSpeed = targetGrunt.statz.moveSpeed;

		// targetGrunt.statz.moveSpeed *= speedMultiplier;
	}

	protected override void Deactivate(Grunt targetGrunt) {
		// targetGrunt.statz.moveSpeed originalGruntSpeed;
	}
}
}
