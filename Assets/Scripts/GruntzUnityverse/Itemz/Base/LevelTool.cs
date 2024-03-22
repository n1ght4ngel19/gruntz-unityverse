using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.Itemz.Base {
public class LevelTool : LevelItem {
	public EquippedTool tool;

	/// <summary>
	/// The animation pack set for the Grunt picking up the item.
	/// </summary>
	public AnimationPack animationPack;

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		yield return base.Pickup(targetGrunt);

		targetGrunt.animationPack = animationPack;
		targetGrunt.equippedTool = tool;
		targetGrunt.gruntEntry.SetTool(codeName);
		Level.instance.levelStatz.toolzCollected++;

		Destroy(gameObject);
	}
}
}
