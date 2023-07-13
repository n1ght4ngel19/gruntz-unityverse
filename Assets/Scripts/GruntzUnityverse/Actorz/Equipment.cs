using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// Stores which Items a Grunt has equipped. 
  /// </summary>
  public class Equipment : MonoBehaviour {
    [field: SerializeField] public Tool tool;
    [field: SerializeField] public Toy toy;
    [field: SerializeField] public Powerup powerup;
  }
}
