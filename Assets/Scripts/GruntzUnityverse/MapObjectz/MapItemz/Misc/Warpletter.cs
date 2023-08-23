using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.Itemz;

namespace GruntzUnityverse.MapObjectz.MapItemz.Misc {
  public class Warpletter : Item {
    public WarpletterType warpletterType;

    protected override void Start() {
      base.Start();

      mapItemName = $"{nameof(Warpletter)}{warpletterType}";
    }

    public override IEnumerator Use(Grunt grunt) {
      yield return null;
    }
  }
}
