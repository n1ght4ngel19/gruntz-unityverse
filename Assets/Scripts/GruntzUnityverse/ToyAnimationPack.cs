using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace GruntzUnityverse {
  public class ToyAnimationPack {
    public readonly List<Sprite> Beachball;
  
    public ToyAnimationPack() {
      Beachball = Resources.LoadAll<Sprite>("Animations/Itemz/Toyz/ToyBeachball").ToList();
    }
  }
}
