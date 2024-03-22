using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
[CreateAssetMenu(fileName = "GooberStraw", menuName = "Gruntz Unityverse/Toolz/Goober Straw")]
public class GooberStraw : EquippedTool {
	public override bool CompatibleWith(GridObject target) => target is GruntPuddle;
}
}
