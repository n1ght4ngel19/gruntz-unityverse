using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.Objectz.Itemz.Toyz {
  public class NoToy : Toy {
    protected override void Start() {
      Name = ToyName.None;
    }

    public override IEnumerator Use(Grunt grunt) {
      // Not applicable
      yield return null;
    }
  }
}
