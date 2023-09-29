using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapItemz.Misc {
  public class Warpletter : Item {
    public WarpletterType warpletterType;

    protected override void Start() {
      base.Start();

      category = "Misc";
      mapItemName = nameof(Warpletter);
    }
  }
}
