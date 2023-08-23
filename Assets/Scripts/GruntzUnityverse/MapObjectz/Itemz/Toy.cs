using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.Itemz {
  public abstract class Toy : Item {
    public ToyName toyName;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      category = nameof(Toy);
    }
    // -------------------------------------------------------------------------------- //

    public override IEnumerator Use(Grunt grunt) {
      // Not applicable
      yield return null;
    }
  }
}
