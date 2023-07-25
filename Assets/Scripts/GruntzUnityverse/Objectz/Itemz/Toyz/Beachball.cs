using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using System.Collections;

namespace GruntzUnityverse.Objectz.Itemz.Toyz {
  public class Beachball : Toy {
    protected override void Start() {
      Name = ToyName.Beachball;
    }

    public override IEnumerator Use(Grunt grunt) {
      // Not applicable
      yield return null;
    }
  }
}
