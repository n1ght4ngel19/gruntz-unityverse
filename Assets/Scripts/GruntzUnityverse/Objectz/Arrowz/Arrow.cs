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
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        grunt.Navigator.TargetLocation = OwnLocation + VectorOfDirection(Direction);
        grunt.Navigator.IsMovementForced = true;

        return;
      }
    }

    protected Vector2Int VectorOfDirection(Direction direction) {
      return direction switch {
        Direction.North => Vector2IntCustom.North(),
        Direction.East => Vector2IntCustom.East(),
        Direction.South => Vector2IntCustom.South(),
        Direction.West => Vector2IntCustom.West(),
        Direction.Northeast => Vector2IntCustom.NorthEast(),
        Direction.Northwest => Vector2IntCustom.NorthWest(),
        Direction.Southeast => Vector2IntCustom.SouthEast(),
        Direction.Southwest => Vector2IntCustom.SouthWest(),
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "No Arrow direction specified!"),
      };
    }
  }
}
