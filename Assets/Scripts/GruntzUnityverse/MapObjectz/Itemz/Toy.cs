using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.Itemz {
  public abstract class Toy : Item {
    public ToyName toyName;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      category = nameof(Toy);
    }
  }
}
