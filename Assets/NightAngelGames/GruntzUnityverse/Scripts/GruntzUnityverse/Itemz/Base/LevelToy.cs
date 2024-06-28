using System.Collections;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.Itemz.Base {
public class LevelToy : LevelItem {
	public EquippedToy toy;

	protected override async void Pickup(Grunt targetGrunt) {
		targetGrunt.enabled = false;

		targetGrunt.animancer.Stop();
		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);

		Level.instance.levelStatz.toyzCollected++;

		// targetGrunt.animationPack = animationPack;
		targetGrunt.equippedToy = toy;
		targetGrunt.gruntEntry.SetToy(codeName);

		targetGrunt.enabled = true;

		targetGrunt.GoToState(StateHandler.State.Walking);

		Destroy(gameObject, 0.5f);
	}
}
}
