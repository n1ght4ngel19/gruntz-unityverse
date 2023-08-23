using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.Itemz.Toyz {
  public class Beachball : Toy {
    protected override void Start() {
      base.Start();

      toyName = ToyName.Beachball;
      mapItemName = nameof(Beachball);
    }
  }
}
