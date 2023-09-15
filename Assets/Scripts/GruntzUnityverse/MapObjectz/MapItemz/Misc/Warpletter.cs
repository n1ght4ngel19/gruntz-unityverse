using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Itemz;

namespace GruntzUnityverse.MapObjectz.MapItemz.Misc {
  public class Warpletter : Item {
    public WarpletterType warpletterType;

    protected override void Start() {
      base.Start();

      category = "Misc";
      mapItemName = nameof(Warpletter);
    }
  }
}
