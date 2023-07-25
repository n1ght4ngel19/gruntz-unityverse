using System;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class Arrow : MapObject {
    [HideInInspector] public Direction direction;


    protected override void Start() {
      base.Start();

      SetDirection();
    }

    protected virtual void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt.AtLocation(location))) {
        grunt.navigator.targetLocation = location + DirectionAsVector(direction);
        grunt.navigator.isMoveForced = true;

        return;
      }
    }

    protected void SetDirection() {
      string spriteName = spriteRenderer.sprite.name;

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
        Direction.North => Vector2IntExtra.North(),
        Direction.East => Vector2IntExtra.East(),
        Direction.South => Vector2IntExtra.South(),
        Direction.West => Vector2IntExtra.West(),
        Direction.Northeast => Vector2IntExtra.NorthEast(),
        Direction.Northwest => Vector2IntExtra.NorthWest(),
        Direction.Southeast => Vector2IntExtra.SouthEast(),
        Direction.Southwest => Vector2IntExtra.SouthWest(),
        Direction.None => throw new ArgumentOutOfRangeException(nameof(dir), dir, "No Arrow direction specified!"),
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, "No Arrow direction specified!"),
      };
    }
  }
}
