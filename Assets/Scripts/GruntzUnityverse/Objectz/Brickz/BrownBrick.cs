using System.Collections;

namespace GruntzUnityverse.Objectz.Brickz {
  public class BrownBrick : Brick {
    public override IEnumerator Break() {
      animancer.Play(BreakAnimation);

      Destroy(gameObject, 1f);

      yield return null;
    }
  }
}
