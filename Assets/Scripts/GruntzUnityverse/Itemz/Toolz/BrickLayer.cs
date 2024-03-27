using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
[CreateAssetMenu(fileName = "BrickLayer", menuName = "Gruntz Unityverse/Toolz/Brick Layer")]
public class BrickLayer : EquippedTool {
	public override bool CompatibleWith(GridObject target) => target is BrickBlock bb && bb.topBrick == null;
}
}
