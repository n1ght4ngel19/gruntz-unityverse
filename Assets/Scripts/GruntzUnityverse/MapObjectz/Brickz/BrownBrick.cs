﻿using System.Collections;

namespace GruntzUnityverse.MapObjectz.Brickz {
  public class BrownBrick : Brick {
    public override IEnumerator Break(float contactDelay) {
      animancer.Play(BreakAnimation);

      Destroy(gameObject, 1f);

      yield return null;
    }
  }
}