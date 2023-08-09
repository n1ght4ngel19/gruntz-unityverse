using System;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class Arrow : MapObject {
    public Direction direction;


    protected override void Start() {
      base.Start();

      SetDirection();
    }

    protected virtual void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        Node targetNode = LevelManager.Instance.NodeAt(location + DirectionAsVector(direction));
        grunt.navigator.targetNode = targetNode;
        grunt.navigator.haveMoveCommand = true;
        grunt.navigator.isMoveForced = true;

        return;
      }
    }

    protected void SetDirection() {
      string spriteName = spriteRenderer.sprite.name;

      //direction = spriteName.Contains(StringDirection.Northeast)
      //  ? Direction.Northeast
      //  : spriteName.Contains(StringDirection.Northwest)
      //  ? Direction.Northwest
      //  : spriteName.Contains(StringDirection.North)
      //  ? Direction.North
      //  : spriteName.Contains(StringDirection.Southeast)
      //  ? Direction.Southeast
      //  : spriteName.Contains(StringDirection.Southwest)
      //  ? Direction.Southwest
      //  : spriteName.Contains(StringDirection.South)
      //  ? Direction.South
      //  : spriteName.Contains(StringDirection.East)
      //  ? Direction.East
      //  : spriteName.Contains(StringDirection.West)
      //  ? Direction.West
      //  : Direction.None;
      if (spriteName.Contains("Northeast")) {
        direction = Direction.Northeast;
      } else if (spriteName.Contains("Northwest")) {
        direction = Direction.Northwest;
      } else if (spriteName.Contains("North")) {
        direction = Direction.North;
      } else if (spriteName.Contains("Southeast")) {
        direction = Direction.Southeast;
      } else if (spriteName.Contains("Southwest")) {
        direction = Direction.Southwest;
      } else if (spriteName.Contains("South")) {
        direction = Direction.South;
      } else if (spriteName.Contains("East")) {
        direction = Direction.East;
      } else if (spriteName.Contains("West")) {
        direction = Direction.West;
      } else {
        Debug.LogError("The direction of the Arrow could not be determined! The Arrow will not function!");
        direction = Direction.None;
      }
    }

    protected Vector2Int DirectionAsVector(Direction dir) {
      return dir switch {
        Direction.North => Vector2Direction.north,
        Direction.East => Vector2Direction.east,
        Direction.South => Vector2Direction.south,
        Direction.West => Vector2Direction.west,
        Direction.Northeast => Vector2Direction.northeast,
        Direction.Northwest => Vector2Direction.northwest,
        Direction.Southeast => Vector2Direction.southeast,
        Direction.Southwest => Vector2Direction.southwest,
        Direction.None => throw new ArgumentOutOfRangeException(nameof(dir), dir, "No Arrow direction specified!"),
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, "No Arrow direction specified!"),
      };
    }
  }
}
