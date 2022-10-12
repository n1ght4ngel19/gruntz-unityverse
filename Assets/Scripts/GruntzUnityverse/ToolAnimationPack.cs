using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace GruntzUnityverse {
  public class ToolAnimationPack {
    public readonly List<Sprite> Club;
    public readonly List<Sprite> Gauntletz;
  
    public ToolAnimationPack() {
      Club = Resources.LoadAll<Sprite>("Animations/Itemz/Toolz/ToolClub").ToList();
      Gauntletz = Resources.LoadAll<Sprite>("Animations/Itemz/Toolz/ToolGauntletz").ToList();
    }
  }
}
