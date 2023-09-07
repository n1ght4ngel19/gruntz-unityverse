using System.Collections;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.Itemz.Toolz {
  public class GooberStraw : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.GooberStraw;
      toolRange = RangeType.Melee;
      damage = GlobalValuez.GooberStrawDamage;
      mapItemName = nameof(GooberStraw);
    }
    // -------------------------------------------------------------------------------- //

    public override IEnumerator UseTool() {
      // Todo: Implement
      yield return null;
    }
  }
}
