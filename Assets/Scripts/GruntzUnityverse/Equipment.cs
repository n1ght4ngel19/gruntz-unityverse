using UnityEngine;

namespace GruntzUnityverse {
  public class Equipment : MonoBehaviour {
    [field: SerializeField] public Tool Tool { get; set; }
    [field: SerializeField] public Toy Toy { get; set; }
    [field: SerializeField] public Powerup Powerup { get; set; }
  }
}
