using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using NaughtyAttributes;

namespace GruntzUnityverse.Itemz.Base {
/// <summary>
/// The base class for Powerupz.
/// </summary>
public class LevelPowerup : LevelItem {
	/// <summary>
	/// The duration of the Powerup.
	/// </summary>
	[BoxGroup("Item Data")]
	[EnableIf(nameof(isEditable))]
	public float duration;

	[BoxGroup("Item Data")]
	[DisableIf(nameof(isInstance))]
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
