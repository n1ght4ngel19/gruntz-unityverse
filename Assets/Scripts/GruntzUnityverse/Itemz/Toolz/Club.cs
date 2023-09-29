using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Toolz {
  public class Club : Tool {
    protected override void Start() {
      toolName = ToolName.Club;
      range = Range.Melee;
      deathInflicted = DeathName.Default;
      damage = GlobalValuez.ClubDamage;
      mapItemName = nameof(Club);
      // itemUseContactDelay = Not applicable;
      attackContactDelay = 0.8f;

      base.Start();
    }
  }
}
