using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Powerupz {
[CreateAssetMenu(fileName = "ZapCola", menuName = "Gruntz Unityverse/Powerupz/ZapCola")]
public class ZapCola : EquippedPowerup {
	[Range(5, 20)]
	public int healAmount;

	protected override void ActivateEffect(Grunt affectedGrunt) {
		affectedGrunt.statz.health += healAmount;
		affectedGrunt.barz.healthBar.Adjust(affectedGrunt.statz.health);
		affectedGrunt.gruntEntry.SetHealth(affectedGrunt.statz.health);
	}

	protected override void DeactivateEffect(Grunt affectedGrunt) {
		// Nothing to do here.
	}
}
}
