using System.Collections;
using GruntzUnityverse.Objectz;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class GauntletzGrunt : Grunt {
    [field: SerializeField] public Rock TargetRock { get; set; }

    public IEnumerator BreakRock(Rock rock) {
      // Animator.Play($"UseItem_{NavComponent.FacingDirection}");
      Animator.Play("Pickup_Item");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);

      IsMovementInterrupted = false;
      Destroy(rock);
    }
  }
}
