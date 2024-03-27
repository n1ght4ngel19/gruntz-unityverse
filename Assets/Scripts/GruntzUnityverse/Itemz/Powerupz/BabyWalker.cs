using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Powerupz {
[CreateAssetMenu(fileName = "BabyWalker", menuName = "Gruntz Unityverse/Powerupz/Baby Walker")]
public class BabyWalker : EquippedPowerup {
	protected override void ActivateEffect(Actorz.Grunt affectedGrunt) {
		affectedGrunt.moveSpeed = 1.5f;
	}

	protected override void DeactivateEffect(Actorz.Grunt affectedGrunt) {
		affectedGrunt.moveSpeed = 1f;
	}
}
}
