using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Barehandz : Tool {
    protected override void Start() {
      toolName = ToolName.Barehandz;
    }

    public override IEnumerator Use(Grunt grunt) {
      // Not applicable
      yield return null;
    }
  }
}
