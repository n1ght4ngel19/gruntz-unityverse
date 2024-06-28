using System.Collections;
using Cysharp.Threading.Tasks;
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

	protected override async void Pickup(Grunt targetGrunt) {
		targetGrunt.enabled = false;

		targetGrunt.animancer.Stop();
		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);

		Level.instance.levelStatz.toolzCollected++;

		targetGrunt.animationPack = animationPack;
		targetGrunt.equippedTool = tool;
		targetGrunt.gruntEntry.SetTool(codeName);

		targetGrunt.enabled = true;

		targetGrunt.GoToState(StateHandler.State.Walking);

		Destroy(gameObject, 0.5f);
	}
}
}
