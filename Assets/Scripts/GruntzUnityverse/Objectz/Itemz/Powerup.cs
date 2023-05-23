using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Powerup : MonoBehaviour {
    [field: SerializeField] public PowerupType Type { get; set; }
  }
}
