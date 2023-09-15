using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Itemz;

namespace GruntzUnityverse.MapObjectz.MapItemz.Misc {
  public class Coin : Item {
    protected override void Start() {
      base.Start();

      category = "Misc";
      mapItemName = nameof(Coin);
    }
  }
}
