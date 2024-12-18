﻿using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
[CreateAssetMenu(fileName = "Gauntletz", menuName = "Gruntz Unityverse/Toolz/Gauntletz")]
public class Gauntletz : EquippedTool {
	public override bool CompatibleWith(GridObject target) => target is Rock || (target is BrickBlock bb && bb.bottomBrick != null);

	public override AnimationClip cursor => AnimationManager.instance.cursorGauntletz;
}
}
