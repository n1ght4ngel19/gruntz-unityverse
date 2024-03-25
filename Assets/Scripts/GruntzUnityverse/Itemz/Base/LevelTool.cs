using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
public class LevelTool : LevelItem {
	public EquippedTool tool;

	/// <summary>
	/// The animation pack set for the Grunt picking up the item.
	/// </summary>
	public AnimationPack animationPack;

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		targetGrunt.enabled = false;
		targetGrunt.animancer.Play(pickupAnim);

		yield return new WaitForSeconds(pickupAnim.length);

		Level.instance.levelStatz.toolzCollected++;

		targetGrunt.animationPack = animationPack;
		targetGrunt.equippedTool = tool;
		targetGrunt.gruntEntry.SetTool(codeName);
		targetGrunt.enabled = true;

		targetGrunt.GoToState(StateHandler.State.Walking);

		Destroy(gameObject);
	}
}
}
