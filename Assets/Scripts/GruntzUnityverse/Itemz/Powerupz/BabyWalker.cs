using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Powerupz {
[CreateAssetMenu(fileName = "BabyWalker", menuName = "Gruntz Unityverse/Powerupz/BabyWalker")]
public class BabyWalker : EquippedPowerup {
	// BabyWalker Equip effect
	protected override void ActivateEffect(Actorz.Grunt affectedGrunt) {
		affectedGrunt.moveSpeed = 1.5f;
	}

	// BabyWalker Deactivate effect
	protected override void DeactivateEffect(Actorz.Grunt affectedGrunt) {
		affectedGrunt.moveSpeed = 1f;
	}
}
}
