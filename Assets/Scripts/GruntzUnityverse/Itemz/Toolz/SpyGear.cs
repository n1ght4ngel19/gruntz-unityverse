using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
[CreateAssetMenu(fileName = "SpyGear", menuName = "Gruntz Unityverse/Toolz/Spy Gear")]
public class SpyGear : EquippedTool {
	public override bool CompatibleWith(GridObject target) => target is BrickBlock bb && bb.bottomBrick != null;
}
}
