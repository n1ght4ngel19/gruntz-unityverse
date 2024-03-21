using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.Itemz.Base {
public class LevelToy : LevelItem {
	public EquippedToy toy;

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		yield return base.Pickup(targetGrunt);

		targetGrunt.equippedToy = toy;
		targetGrunt.gruntEntry.SetToy(codeName);
		Level.instance.levelStatz.toyzCollected++;

		Destroy(gameObject);
	}
}
}
