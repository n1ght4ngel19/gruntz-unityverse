using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Powerupz {
  public class ZapCola : Powerup {
    public int healAmount;
    public ZapColaSize size;

    protected override void Start() {
      mapItemName = $"{nameof(ZapCola)}{size.ToString()}";

      healAmount = size switch {
        ZapColaSize.Can => 5,
        ZapColaSize.Bottle => 10,
        ZapColaSize.Keg => 20,
        _ => 0,
      };
      
      base.Start();
    }
  }
}
