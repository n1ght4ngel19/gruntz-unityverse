using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapItemz.Misc {
  public class Coin : Item {
    protected override void Start() {
      base.Start();

      category = "Misc";
      mapItemName = nameof(Coin);
    }
  }
}
