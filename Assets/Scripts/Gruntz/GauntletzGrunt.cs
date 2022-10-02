using UnityEngine;

namespace Gruntz {
  public class GauntletzGrunt : Grunt {
    
    private void Start() {
      targetPosition = transform.position;
    }

    private void Update() {
      PlaySouthIdleAnimationByDefault();
      
      SetTargetPosition();

      SetPath();
      
      Vector3 destination = new(
        Mathf.Floor(path[0].gridLocation.x) + 0.5f,
        Mathf.Floor(path[0].gridLocation.y) + 0.5f,
        -5
      );

      diffVector = destination - transform.position;
      transform.position = destination;

      path.RemoveAt(0);
    }
  }
}
