using System.Collections;
using GruntzUnityverse.V2.Actorz;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Itemz.Toolz;

namespace GruntzUnityverse.V2.Itemz {
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
