using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.AnimationPackz {
  public class ToyAnimationPack {
    public readonly List<Sprite> Beachball;
  
    public ToyAnimationPack() {
      Beachball = Resources.LoadAll<Sprite>("Animated Sprites/MapObjectz/Itemz/Toyz/ToyBeachball").ToList();
    }
  }
}
