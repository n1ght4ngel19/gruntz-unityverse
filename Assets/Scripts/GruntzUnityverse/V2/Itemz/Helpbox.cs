using System.Collections;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz {
  public class Helpbox : ItemV2 {
    public HelpboxText helpboxText;

    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      // Todo: Pause game and show helpbox UI
      Debug.Log(helpboxText.text);
    }

    protected override IEnumerator OnTriggerEnter2D(Collider2D other) {
      if (!circleCollider2D.isTrigger) {
        yield break;
      }

      GruntV2 grunt = other.gameObject.GetComponent<GruntV2>();

      if (grunt == null) {
        yield break;
      }

      yield return Pickup(grunt);

      circleCollider2D.isTrigger = false;
    }

    private void OnTriggerExit2D(Collider2D other) {
      if (other.gameObject.GetComponent<GruntV2>() != null) {
        circleCollider2D.isTrigger = true;
      }
    }
  }
}
