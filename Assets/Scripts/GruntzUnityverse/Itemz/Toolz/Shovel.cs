using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Toolz {
[CreateAssetMenu(fileName = "Shovel", menuName = "Gruntz Unityverse/Toolz/Shovel")]
public class Shovel : EquippedTool {
	public override bool CompatibleWith(GridObject target) => target is Hole;
}
}
