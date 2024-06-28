using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Powerupz {
[CreateAssetMenu(fileName = "ReactiveArmor", menuName = "Gruntz Unityverse/Powerupz/Reactive Armor")]
public class ReactiveArmor : EquippedPowerup {
	protected override void ActivateEffect(Grunt affectedGrunt) {
		// Receive 25% of the damage normally taken.
		affectedGrunt.damageReductionPercentage = 0.75f;

		// Reflect 75% of the damage taken back to the attacker.
		affectedGrunt.damageReflectionPercentage = 0.75f;
	}

	protected override void DeactivateEffect(Grunt affectedGrunt) {
		affectedGrunt.damageReductionPercentage = 0f;

		affectedGrunt.damageReflectionPercentage = 0f;
	}
}
}
