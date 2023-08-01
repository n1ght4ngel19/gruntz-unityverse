using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.Objectz.Itemz {
  public abstract class Tool : Item {
    public ToolName toolName;

    public virtual IEnumerator Attack(Grunt grunt) {
      yield return null;
    }
  }
}
