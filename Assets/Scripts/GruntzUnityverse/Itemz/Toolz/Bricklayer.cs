using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Toolz {
  public class Bricklayer : Tool {
    protected override void Start() {
      toolName = ToolName.Bricklayer;
      range = Range.Melee;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.BricklayerDamage;
      mapItemName = nameof(Bricklayer);
      itemUseContactDelay = 0;
      attackContactDelay = 0.8f;

      base.Start();
    }
  }
}
