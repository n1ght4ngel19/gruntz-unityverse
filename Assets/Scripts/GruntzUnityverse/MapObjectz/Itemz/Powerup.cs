using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz {
  // Todo: Inherit from Item
  public class Powerup : MonoBehaviour {
    [field: SerializeField] public PowerupName Name { get; set; }
  }
}
