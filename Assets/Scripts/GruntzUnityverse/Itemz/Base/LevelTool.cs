using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.Itemz.Base {
public class LevelTool : LevelItem {
	public EquippedTool tool;

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		yield return base.Pickup(targetGrunt);

		targetGrunt.animationPack = animationPack;
		targetGrunt.equippedTool = tool;
		targetGrunt.gruntEntry.SetTool(codeName);
		Level.Instance.levelStatz.toolzCollected++;

		Destroy(gameObject);
	}
}
}
