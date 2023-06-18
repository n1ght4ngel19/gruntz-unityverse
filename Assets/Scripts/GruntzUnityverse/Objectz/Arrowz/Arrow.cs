using System;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class Arrow : MapObject {
    [field: SerializeField] public Direction Direction { get; set; }

    protected virtual void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt.AtLocation(Location))) {
        grunt.navigator.TargetLocation = Location + VectorOfDirection(Direction);
        grunt.navigator.IsMovementForced = true;

        return;
      }
    }

    protected Vector2Int VectorOfDirection(Direction direction) {
      return direction switch {
        Direction.North => Vector2IntExtra.North(),
        Direction.East => Vector2IntExtra.East(),
        Direction.South => Vector2IntExtra.South(),
        Direction.West => Vector2IntExtra.West(),
        Direction.Northeast => Vector2IntExtra.NorthEast(),
        Direction.Northwest => Vector2IntExtra.NorthWest(),
        Direction.Southeast => Vector2IntExtra.SouthEast(),
        Direction.Southwest => Vector2IntExtra.SouthWest(),
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "No Arrow direction specified!"),
      };
    }
  }
}
