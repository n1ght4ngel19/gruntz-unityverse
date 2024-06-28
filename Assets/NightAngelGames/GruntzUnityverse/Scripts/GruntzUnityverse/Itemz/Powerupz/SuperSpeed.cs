using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Powerupz {
[CreateAssetMenu(fileName = "SuperSpeed", menuName = "Gruntz Unityverse/Powerupz/Super Speed")]
public class SuperSpeed : EquippedPowerup {
	/// <summary>
	/// The multiplier to apply to the grunt's base move speed.
	/// </summary>
	public float speedMultiplier;

	protected override void ActivateEffect(Grunt affectedGrunt) {
		Debug.Log("SuperSpeed activated");
		affectedGrunt.moveSpeed /= speedMultiplier;
	}

	protected override void DeactivateEffect(Grunt affectedGrunt) {
		affectedGrunt.moveSpeed *= speedMultiplier;
	}
}
}
