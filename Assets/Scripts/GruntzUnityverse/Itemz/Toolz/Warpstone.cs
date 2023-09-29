using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Toolz {
  public class Warpstone : Tool {
    protected override void Start() {
      toolName = ToolName.Warpstone;
      range = Range.None;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.WarpstoneDamage;
      mapItemName = nameof(Warpstone);
      // itemUseContactDelay = Not applicable;
      // attackContactDelay = Not applicable;

      base.Start();
    }
    // -------------------------------------------------------------------------------- //

    public override IEnumerator UseTool() {
      yield return null;

      ownGrunt.CleanState();
    }
  }
}
