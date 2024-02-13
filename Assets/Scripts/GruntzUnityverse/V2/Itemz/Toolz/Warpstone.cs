using System.Collections;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Grunt;

namespace GruntzUnityverse.V2.Itemz.Toolz {
public class Warpstone : Tool {
	protected override IEnumerator Pickup(GruntV2 target) {
		yield return base.Pickup(target);

		GM.Instance.levelStatz.warpstoneRecovered = true;
	}
}
}
