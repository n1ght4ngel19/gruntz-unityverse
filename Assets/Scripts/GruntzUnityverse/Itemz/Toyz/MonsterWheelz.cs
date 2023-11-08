using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.Itemz.Toyz {
  public class MonsterWheelz : Toy {
    protected override void Start() {
      toyName = ToyName.MonsterWheelz;
      mapItemName = nameof(MonsterWheelz);

      base.Start();
    }
  }
}
