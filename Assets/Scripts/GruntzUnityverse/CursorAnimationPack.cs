using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace GruntzUnityverse {
  public class CursorAnimationPack {
    public readonly List<Sprite> Pointer;
    public readonly List<Sprite> Gauntletz;
  
    public CursorAnimationPack() {
      Pointer = Resources.LoadAll<Sprite>($"Animations/Cursorz/CursorPointer").ToList();
      Gauntletz = Resources.LoadAll<Sprite>($"Animations/Cursorz/CursorGauntletz").ToList();
    }
  }
}
