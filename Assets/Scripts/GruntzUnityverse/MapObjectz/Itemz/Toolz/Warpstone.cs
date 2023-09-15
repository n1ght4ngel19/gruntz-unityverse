using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Warpstone : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Warpstone;
      toolRange = RangeType.None;
      damage = GlobalValuez.WarpstoneDamage;
      mapItemName = nameof(Warpstone);
    }
    // -------------------------------------------------------------------------------- //

    public override IEnumerator UseTool() {
      yield return null;
    }
  }
}
