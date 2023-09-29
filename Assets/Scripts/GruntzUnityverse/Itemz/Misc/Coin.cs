using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Misc {
  public class Coin : MiscItem {
    protected override void Start() {
      mapItemName = nameof(Coin);

      base.Start();
    }
  }
}
