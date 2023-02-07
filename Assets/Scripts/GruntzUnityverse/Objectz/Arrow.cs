using System;
using System.Linq;
using _Test;
using Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Objectz
{
  public class Arrow : MonoBehaviour
  {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public CompassDirection Direction { get; set; }


    private void Start()
    {
      OwnLocation = Vector2Int.FloorToInt
      (
        transform.position
      );
    }

    private void Update()
    {
      foreach (TGrunt grunt in LevelManager.Instance.PlayerGruntz
        .Where
        (
          grunt => grunt.NavComponent.OwnLocation.Equals
          (
            OwnLocation
          )
        )
      )
      {
        grunt.NavComponent.TargetLocation = OwnLocation
          + VectorOfDirection
          (
            Direction
          );

        return;
      }
    }

    public Vector2Int VectorOfDirection(CompassDirection direction)
    {
      return direction switch {
        CompassDirection.North => Vector2IntC.North,
        CompassDirection.East => Vector2IntC.East,
        CompassDirection.South => Vector2IntC.South,
        CompassDirection.West => Vector2Int.left,
        CompassDirection.NorthEast => Vector2IntC.NorthEast,
        CompassDirection.NorthWest => Vector2IntC.NorthWest,
        CompassDirection.SouthEast => Vector2IntC.SouthEast,
        CompassDirection.SouthWest => Vector2IntC.SouthWest,
        var _ => throw new ArgumentOutOfRangeException
        (
          nameof(direction),
          direction,
          "No Arrow direction specified!"
        ),
      };
    }
  }
}
