using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Brickz {
  public class BrownBrick : Brick {
    public override IEnumerator Explode() {
      // Animancer.Play("BrownBrickExplode");
      Debug.Log("Explosion");

      yield return new WaitForSeconds(0.5f);

      Destroy(gameObject, 0.5f);
    }
  }
}
