using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class Equipment : MonoBehaviour {
    [field: SerializeField] public ItemTool Tool { get; set; }
    [field: SerializeField] public ItemToy Toy { get; set; }
    [field: SerializeField] public ItemPowerup Powerup { get; set; }
  }
}
