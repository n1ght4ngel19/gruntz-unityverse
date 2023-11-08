using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Toyz {
  public class JackInTheBox : Toy {
    protected override void Start() {
      toyName = ToyName.JackInTheBox;
      mapItemName = nameof(JackInTheBox);

      base.Start();
    }
  }
}
