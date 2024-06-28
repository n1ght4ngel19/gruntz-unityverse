using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;

namespace GruntzUnityverse.Itemz.Misc {
public class Warpletter : LevelItem {
	public WarpletterType type;

	protected override async void Pickup(Grunt targetGrunt) {
		targetGrunt.enabled = false;

		targetGrunt.animancer.Stop();
		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);

		Level.instance.levelStatz.warpletterzCollected++;

		targetGrunt.enabled = true;

		targetGrunt.GoToState(StateHandler.State.Walking);

		Destroy(gameObject, 0.5f);
	}
}

public enum WarpletterType {
	W = 1,
	A = 2,
	R = 3,
	P = 4,
}
}
