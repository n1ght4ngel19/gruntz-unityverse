using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapPowerup : MapItem {
    [field: SerializeField] public PowerupType Type { get; set; }
  }
}
