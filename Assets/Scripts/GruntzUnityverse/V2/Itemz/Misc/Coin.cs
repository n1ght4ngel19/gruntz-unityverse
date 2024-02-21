using System.Collections;
using GruntzUnityverse.V2.Actorz;
using GruntzUnityverse.V2.Core;

namespace GruntzUnityverse.V2.Itemz.Misc {
public class Coin : LevelItem {
	protected override IEnumerator Pickup(Grunt target) {
		yield return base.Pickup(target);

		GameManager.Instance.levelStatz.coinzCollected++;
	}
}
}
