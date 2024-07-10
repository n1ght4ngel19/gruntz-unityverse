using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using NaughtyAttributes;

namespace GruntzUnityverse.Itemz.Base {
public class LevelToy : LevelItem {
	[DisableIf(nameof(isInstance))]
	public EquippedToy toy;

	protected override async void Pickup(Grunt targetGrunt) {
		targetGrunt.GoToState(StateHandler.State.Committed);
		targetGrunt.enabled = false;

		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);

		Level.instance.levelStatz.toyzCollected++;

		// targetGrunt.animationPack = animationPack;
		targetGrunt.toy = toy;
		targetGrunt.gruntEntry.SetToy(codename);

		targetGrunt.enabled = true;
		targetGrunt.GoToState(StateHandler.State.Walking);

		Destroy(gameObject);
	}
}
}
