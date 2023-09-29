using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Powerup {
  public class ZapCola : Item {
    public ZapColaSize size;

    protected override void Start() {
      mapItemName = $"{nameof(ZapCola)}{size.ToString()}";

      base.Start();
    }
  }
}
