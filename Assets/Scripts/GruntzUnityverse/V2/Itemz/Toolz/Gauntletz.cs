using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Toolz {
  public class Gauntletz : Tool {
    public override IEnumerator Use() {
      Debug.Log("Breaking stuff!");

      yield return new WaitForSeconds(useTime);
    }
  }
}
