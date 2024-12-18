﻿using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;
using NaughtyAttributes;

namespace GruntzUnityverse.Itemz.Misc {
public class Warpletter : LevelItem {
	[DisableIf(nameof(isInstance))]
	public WarpletterType type;

	protected override async void Pickup(Grunt targetGrunt) {
		targetGrunt.GoToState(StateHandler.State.Committed);
		targetGrunt.enabled = false;

		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);

		Level.instance.levelStatz.warpletterzCollected++;

		targetGrunt.enabled = true;
		targetGrunt.GoToState(StateHandler.State.Walking);

		Destroy(gameObject);
	}
}

public enum WarpletterType {
	W = 1,
	A = 2,
	R = 3,
	P = 4,
}
}
