using System.Collections;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Toolz {
  public class WarpstoneV2 : Tool {
    protected override IEnumerator Pickup(GruntV2 target) {
      yield  return base.Pickup(target);

      GM.Instance.levelStatz.warpstoneRecovered = true;
    }

    // No implementation (for now), Gruntz carrying a Warpstone cannot interact
    public override IEnumerator Use(GruntV2 target) {
      yield break;
    }

    // No implementation (for now), Gruntz carrying a Warpstone cannot interact
    public override IEnumerator Use(GameObject target) {
      yield break;
    }
  }
}
