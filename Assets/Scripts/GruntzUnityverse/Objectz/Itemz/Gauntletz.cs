using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Gauntletz : ItemTool {
    [field: SerializeField] public Grunt TargetGrunt { get; set; }
    [field: SerializeField] public Rock TargetRock { get; set; }
    // [field: SerializeField] public Brick TargetBrick { get; set; }
  }
}
