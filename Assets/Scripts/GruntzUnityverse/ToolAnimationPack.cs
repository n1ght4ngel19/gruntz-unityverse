using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace GruntzUnityverse {
  public class ToolAnimationPack {
    public readonly List<Sprite> Club;
    public readonly List<Sprite> Gauntletz;
    public readonly List<Sprite> Warpstone1;
  
    public ToolAnimationPack() {
      Club = Resources.LoadAll<Sprite>("Animations/MapObjectz/Itemz/Toolz/ToolClub").ToList();
      Gauntletz = Resources.LoadAll<Sprite>("Animations/MapObjectz/Itemz/Toolz/ToolGauntletz").ToList();
      Warpstone1 = Resources.LoadAll<Sprite>("Animations/MapObjectz/Itemz/Toolz/ToolWarpstone1").ToList();
    }
  }
}
