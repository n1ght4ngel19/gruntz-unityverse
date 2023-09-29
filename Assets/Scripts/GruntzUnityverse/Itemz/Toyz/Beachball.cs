using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Toyz {
  public class Beachball : Toy {
    protected override void Start() {
      toyName = ToyName.Beachball;
      mapItemName = nameof(Beachball);

      base.Start();
    }
  }
}
