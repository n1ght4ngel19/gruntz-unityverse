using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapPowerup : MapItem {
    [field: SerializeField] public Powerup PickupPowerup { get; set; }
  }
}
