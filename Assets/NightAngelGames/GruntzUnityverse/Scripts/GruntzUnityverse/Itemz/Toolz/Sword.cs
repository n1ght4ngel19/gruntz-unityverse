using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
[CreateAssetMenu(fileName = "Sword", menuName = "Gruntz Unityverse/Toolz/Sword")]
public class Sword : EquippedTool {
	public override AnimationClip cursor => AnimationManager.instance.cursorSword;
}
}
