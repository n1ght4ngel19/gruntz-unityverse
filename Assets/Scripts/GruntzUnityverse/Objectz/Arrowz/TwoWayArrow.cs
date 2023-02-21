using System;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class TwoWayArrow : MapObject {
    [field: SerializeField] public CompassDirection DefaultDirection { get; set; }
    private CompassDirection AlternateDirection { get; set; }
    private CompassDirection Direction { get; set; }
    private Sprite DefaultSprite { get; set; }
    [field: SerializeField] public Sprite AlternateSprite { get; set; }


    protected override void Start() {
      base.Start();

      DefaultSprite = Renderer.sprite;
      Direction = DefaultDirection;

      SetAlternateDirection();
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        grunt.NavComponent.TargetLocation = OwnLocation + VectorOfDirection(Direction);

        return;
      }
    }

    public void ChangeDirection() {
      Direction = Direction.Equals(DefaultDirection)
        ? AlternateDirection
        : DefaultDirection;

      Renderer.sprite = Direction.Equals(DefaultDirection)
        ? DefaultSprite
        : AlternateSprite;
    }

    private void SetAlternateDirection() {
      AlternateDirection = DefaultDirection switch {
        CompassDirection.North => CompassDirection.South,
        CompassDirection.East => CompassDirection.West,
        CompassDirection.South => CompassDirection.North,
        CompassDirection.West => CompassDirection.East,
        CompassDirection.NorthEast => CompassDirection.SouthWest,
        CompassDirection.NorthWest => CompassDirection.SouthEast,
        CompassDirection.SouthEast => CompassDirection.NorthWest,
        CompassDirection.SouthWest => CompassDirection.NorthEast,
        var _ => throw new ArgumentOutOfRangeException(),
      };
    }

    public Vector2Int VectorOfDirection(CompassDirection direction) {
      return direction switch {
        CompassDirection.North => Vector2IntCustom.North,
        CompassDirection.East => Vector2IntCustom.East,
        CompassDirection.South => Vector2IntCustom.South,
        CompassDirection.West => Vector2Int.left,
        CompassDirection.NorthEast => Vector2IntCustom.NorthEast,
        CompassDirection.NorthWest => Vector2IntCustom.NorthWest,
        CompassDirection.SouthEast => Vector2IntCustom.SouthEast,
        CompassDirection.SouthWest => Vector2IntCustom.SouthWest,
        var _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "No Arrow direction specified!"),
      };
    }
  }
}
