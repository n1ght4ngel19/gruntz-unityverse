using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class ItemPowerup : MonoBehaviour {
    [field: SerializeField] public PowerupType Type { get; set; }
  }
}
