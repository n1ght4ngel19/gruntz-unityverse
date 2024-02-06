using GruntzUnityverse.V2.Grunt;
using GruntzUnityverse.V2.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public class Arrow : GridObject {
    public DirectionV2 direction;
    public NodeV2 targetNode;

    protected override void Start() {
      base.Start();

      targetNode = direction switch {
        DirectionV2.Up => node.neighbourSet.up,
        DirectionV2.UpRight => node.neighbourSet.upRight,
        DirectionV2.Right => node.neighbourSet.right,
        DirectionV2.DownRight => node.neighbourSet.downRight,
        DirectionV2.Down => node.neighbourSet.down,
        DirectionV2.DownLeft => node.neighbourSet.downLeft,
        DirectionV2.Left => node.neighbourSet.left,
        DirectionV2.UpLeft => node.neighbourSet.upLeft,
        _ => null
      };
    }

    private void OnTriggerEnter2D(Collider2D other) {
      GruntV2 grunt = other.GetComponent<GruntV2>();

      Debug.Log("Colliding with something");

      if (grunt != null) {
        Debug.Log("Colliding with Gruntz");
        // StopCoroutine(grunt.Move());
        // grunt.flagz.interrupted = true;
        StartCoroutine(grunt.MoveTo(targetNode));
      }
    }
  }
}
