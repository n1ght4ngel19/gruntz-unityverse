using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.MiscItemz {
  public class Coin : MiscItem {
    protected override void Start() {
      mapItemName = nameof(Coin);

      base.Start();
    }
  }
}
