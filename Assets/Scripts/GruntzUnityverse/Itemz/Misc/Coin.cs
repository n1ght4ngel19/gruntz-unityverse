using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;

namespace GruntzUnityverse.Itemz.Misc {
public class Coin : LevelItem {
	protected override IEnumerator Pickup(Grunt target) {
		yield return base.Pickup(target);

		GameManager.Instance.levelStatz.coinzCollected++;
	}
}
}
