using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Powerupz {
[CreateAssetMenu(fileName = "ZapCola", menuName = "Gruntz Unityverse/Powerupz/Zap Cola")]
public class ZapCola : EquippedPowerup {
	[Range(5, 20)]
	public int healAmount;

	protected override void ActivateEffect(Grunt affectedGrunt) {
		affectedGrunt.TakeDamage(-healAmount);
	}

	protected override void DeactivateEffect(Grunt affectedGrunt) {
		// Nothing to do here.
	}
}
}
