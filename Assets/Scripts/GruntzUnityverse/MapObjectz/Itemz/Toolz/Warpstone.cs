using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class Warpstone : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Warpstone;
      rangeType = RangeType.None;
      damage = GlobalValuez.WarpstoneDamage;
      mapItemName = nameof(Warpstone);
    }

    public override IEnumerator Use(Grunt grunt) {
      yield return null;
    }
  }
}
