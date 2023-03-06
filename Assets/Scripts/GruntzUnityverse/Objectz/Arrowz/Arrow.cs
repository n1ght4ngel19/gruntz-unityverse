using System;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class Arrow : MapObject {
    [field: SerializeField] public CompassDirection Direction { get; set; }

    protected virtual void Update() {
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        grunt.NavComponent.TargetLocation = OwnLocation + VectorOfDirection(Direction);
        grunt.NavComponent.IsMovementForced = true;

        return;
      }
    }

    protected Vector2Int VectorOfDirection(CompassDirection direction) {
      return direction switch {
        CompassDirection.North => Vector2IntCustom.North,
        CompassDirection.East => Vector2IntCustom.East,
        CompassDirection.South => Vector2IntCustom.South,
        CompassDirection.West => Vector2Int.left,
        CompassDirection.NorthEast => Vector2IntCustom.NorthEast,
        CompassDirection.NorthWest => Vector2IntCustom.NorthWest,
        CompassDirection.SouthEast => Vector2IntCustom.SouthEast,
        CompassDirection.SouthWest => Vector2IntCustom.SouthWest,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "No Arrow direction specified!"),
      };
    }
  }
}
