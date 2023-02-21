using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Powerup : MapObject {
    [field: SerializeField] public AnimatorOverrideController OverrideController { get; set; }
    [field: SerializeField] public PowerupType Type { get; set; }
  }
}
