using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.MapObjectz.Itemz;

namespace GruntzUnityverse.MapObjectz.MapItemz.Misc {
  public class Coin : Item {
    protected override void Start() {
      base.Start();

      mapItemName = nameof(Coin);
    }

    public override IEnumerator Use(Grunt grunt) {
      yield return null;
    }
  }
}
