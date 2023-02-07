using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Collectiblez {
  public class Toy : MonoBehaviour {
    public Vector2Int OwnLocation { get; set; }
    public ToyType type;

    
    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }
  }
}
