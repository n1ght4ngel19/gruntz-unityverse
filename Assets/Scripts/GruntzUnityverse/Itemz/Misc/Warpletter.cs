using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;

namespace GruntzUnityverse.Itemz.Misc {
public class Warpletter : LevelItem {
	public WarpletterType type;

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		yield return base.Pickup(targetGrunt);

		Level.Instance.levelStatz.warpletterzCollected++;
	}
}

public enum WarpletterType {
	W = 1,
	A = 2,
	R = 3,
	P = 4,
}
}
