using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.Itemz.Base {
/// <summary>
/// The base class for Powerupz.
/// </summary>
public class LevelPowerup : LevelItem {
	/// <summary>
	/// The duration of the Powerup.
	/// </summary>
	public float duration;

	public EquippedPowerup equippedPowerup;

	protected override async void Pickup(Grunt targetGrunt) {
		targetGrunt.GoToState(StateHandler.State.Committed);
		targetGrunt.enabled = false;

		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);

		Level.instance.levelStatz.powerupzCollected++;

		// targetGrunt.animationPack = animationPack;
		targetGrunt.equippedPowerupz.Add(equippedPowerup);
		equippedPowerup.Equip(targetGrunt, duration);
		// targetGrunt.gruntEntry.SetPowerup(codeName);

		targetGrunt.enabled = true;
		targetGrunt.GoToState(StateHandler.State.Walking);

		Destroy(gameObject);
	}
}
}
