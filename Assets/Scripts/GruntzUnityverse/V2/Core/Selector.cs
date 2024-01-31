using GruntzUnityverse.V2.Grunt;
using GruntzUnityverse.V2.Utils;
using UnityEngine;

namespace GruntzUnityverse.V2.Core {
  public class Selector : MonoBehaviour {
    public Vector2Int location2D;

    // Temporary for testing
    public GruntV2 baseGrunt;
    public Transform gruntzTransform;

    private void Update() {
      transform.position = Input.mousePosition.FromCamera().RoundedToIntWithCustomZ(15f);
      location2D = Vector2Int.RoundToInt(transform.position);
    }

    private void OnMove() {
      GruntV2 gruntV2 = Instantiate(baseGrunt, transform.position, Quaternion.identity, gruntzTransform);
      gruntV2.GenerateGuid();
    }
  }
}
