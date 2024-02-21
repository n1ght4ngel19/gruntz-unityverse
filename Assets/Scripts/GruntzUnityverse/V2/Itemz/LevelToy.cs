using System.Collections;
using GruntzUnityverse.V2.Actorz;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Itemz.Toyz;

namespace GruntzUnityverse.V2.Itemz {
public class LevelToy : LevelItem {
	public EquippedToy toy;

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		yield return base.Pickup(targetGrunt);

		targetGrunt.equippedToy = toy;

		Level.Instance.levelStatz.toyzCollected++;

		Destroy(gameObject);
	}
}
}
