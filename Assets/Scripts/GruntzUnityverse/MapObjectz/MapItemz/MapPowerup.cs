using GruntzUnityverse.MapObjectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.MapItemz {
  public class MapPowerup : MapItem {
    [field: SerializeField] public Powerup PickupPowerup { get; set; }
  }
}
