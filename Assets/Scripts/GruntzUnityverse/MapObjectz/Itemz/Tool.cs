using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.MapObjectz.Itemz {
  public abstract class Tool : Item {
    public ToolName toolName;
    public RangeType rangeType;


    public virtual IEnumerator UseItem() {
      yield return null;
    }

    public virtual IEnumerator Attack(Grunt grunt) {
      yield return null;
    }
  }
}
