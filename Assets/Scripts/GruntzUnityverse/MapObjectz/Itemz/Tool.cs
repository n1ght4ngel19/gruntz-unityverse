using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz {
  public abstract class Tool : Item {
    public ToolName toolName;
    public RangeType rangeType;
    [Range(0, 40)] public int damage;
    [Range(0, 20)] public int damageReduction;


    public virtual IEnumerator UseItem() {
      yield return null;
    }

    public virtual IEnumerator Attack(Grunt attackTarget) {
      yield return null;
    }
  }
}
