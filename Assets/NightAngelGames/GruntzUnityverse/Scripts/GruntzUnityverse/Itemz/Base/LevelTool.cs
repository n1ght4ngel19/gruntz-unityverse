using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using NaughtyAttributes;

namespace GruntzUnityverse.Itemz.Base {
public class LevelTool : LevelItem {
	[DisableIf(nameof(isInstance))]
	public EquippedTool tool;

	/// <summary>
	/// The animation pack set for the Grunt picking up the item.
	/// </summary>
	[DisableIf(nameof(isInstance))]
	public AnimationPack animationPack;

	protected override async void Pickup(Grunt targetGrunt) {
		targetGrunt.GoToState(StateHandler.State.Committed);
		targetGrunt.enabled = false;

		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);

		Level.instance.levelStatz.toolzCollected++;

		targetGrunt.animationPack = animationPack;
		targetGrunt.equippedTool = tool;
		targetGrunt.gruntEntry.SetTool(codename);

		targetGrunt.enabled = true;
		targetGrunt.GoToState(StateHandler.State.Walking);

		Destroy(gameObject, 0.5f);
	}
}
}
