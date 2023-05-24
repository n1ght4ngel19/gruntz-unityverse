using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// Stores which Items a Grunt has equipped. 
  /// </summary>
  public class Equipment : MonoBehaviour {
    [field: SerializeField] public Tool Tool { get; set; }
    [field: SerializeField] public Toy Toy { get; set; }
    [field: SerializeField] public Powerup Powerup { get; set; }
  }
}
