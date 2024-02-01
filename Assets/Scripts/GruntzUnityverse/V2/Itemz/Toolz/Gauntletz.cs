using System.Collections;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Toolz {
  public class Gauntletz : Tool {
    public override IEnumerator Use(GruntV2 target) {
      // Todo: Play use or attack animation
      Debug.Log("Attacking!");

      yield return new WaitForSeconds(useTime);
    }

    public override IEnumerator Use(GameObject target) {
      // Todo: Play use or attack animation
      Debug.Log("Breaking stuff!");

      yield return new WaitForSeconds(useTime);
    }
  }
}
