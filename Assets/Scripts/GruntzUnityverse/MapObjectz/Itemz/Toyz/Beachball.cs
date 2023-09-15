using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Itemz.Toyz {
  public class Beachball : Toy {
    protected override void Start() {
      base.Start();

      toyName = ToyName.Beachball;
      mapItemName = nameof(Beachball);
    }
  }
}
