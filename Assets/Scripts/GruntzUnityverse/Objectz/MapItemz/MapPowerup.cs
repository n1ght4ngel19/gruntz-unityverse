using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapPowerup : MapObject {
    [field: SerializeField] public PowerupType Type { get; set; }
  }
}
