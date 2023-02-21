using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class Equipment : MonoBehaviour {
    [field: SerializeField] public Tool Tool { get; set; }
    [field: SerializeField] public Toy Toy { get; set; }
    [field: SerializeField] public Powerup Powerup { get; set; }

    private void Start() {
      Tool = gameObject.GetComponent<Tool>();
      Toy = gameObject.GetComponent<Toy>();
      Powerup = gameObject.GetComponent<Powerup>();
    }
  }
}
