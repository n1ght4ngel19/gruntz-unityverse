using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.AnimationPackz {
  public class CursorAnimationPack {
    public readonly List<Sprite> Pointer;
    public readonly List<Sprite> Gauntletz;
  
    public CursorAnimationPack() {
      // Pointer = Resources.LoadAll<Sprite>($"Animated Sprites/Cursorz/CursorPointer").ToList();
      // Gauntletz = Resources.LoadAll<Sprite>($"Animated Sprites/Cursorz/CursorGauntletz").ToList();
    }
  }
}
