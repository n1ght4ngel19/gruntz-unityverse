using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.Itemz.Base {
public class LevelTool : LevelItem {
	public EquippedTool tool;

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		yield return base.Pickup(targetGrunt);

		targetGrunt.equippedTool = tool;

		Level.Instance.levelStatz.toolzCollected++;

		Destroy(gameObject);
	}
}
}
