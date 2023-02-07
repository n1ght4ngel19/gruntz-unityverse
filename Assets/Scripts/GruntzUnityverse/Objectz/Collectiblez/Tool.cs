using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Collectiblez {
  public class Tool : MonoBehaviour {
    public Vector2Int OwnLocation {get; set;}
    public ToolType type;

    
    private void Start() {
      // Todo: Set map animation
      OwnLocation = Vector2Int.FloorToInt(transform.position);
    }
  }
}
