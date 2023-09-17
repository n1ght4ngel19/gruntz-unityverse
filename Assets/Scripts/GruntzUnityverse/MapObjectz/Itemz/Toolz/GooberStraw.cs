using System.Collections;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class GooberStraw : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.GooberStraw;
      range = Range.Melee;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.GooberStrawDamage;
      mapItemName = nameof(GooberStraw);
      itemUseContactDelay = 0.5f;
      attackContactDelay = 0.4f;
    }
    // -------------------------------------------------------------------------------- //

    public override IEnumerator UseTool() {
      // Todo: Implement
      yield return null;
    }
  }
}
