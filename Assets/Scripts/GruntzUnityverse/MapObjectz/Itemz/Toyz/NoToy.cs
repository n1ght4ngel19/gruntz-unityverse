using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Itemz.Toyz {
  public class NoToy : Toy {
    protected override void Start() {
      toyName = ToyName.None;
    }
  }
}
