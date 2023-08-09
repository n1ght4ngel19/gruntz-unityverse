using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using System.Collections;

namespace GruntzUnityverse.Objectz.Itemz.Toolz {
  public class Warpstone : Tool {
    protected override void Start() {
      base.Start();

      toolName = ToolName.Warpstone;
      rangeType = RangeType.None;
    }

    public override IEnumerator Use(Grunt grunt) {
      yield return null;
    }
  }
}
