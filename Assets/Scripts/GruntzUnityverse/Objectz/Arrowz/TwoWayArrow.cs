using System;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class TwoWayArrow : MonoBehaviour {
    private Vector2Int OwnLocation { get; set; }
    private SpriteRenderer Renderer { get; set; }
    [field: SerializeField] public CompassDirection DefaultDirection { get; set; }
    private CompassDirection AlternateDirection { get; set; }
    private CompassDirection Direction { get; set; }
    private Sprite DefaultSprite { get; set; }
    [field: SerializeField] public Sprite AlternateSprite { get; set; }


    private void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Renderer = gameObject.GetComponent<SpriteRenderer>();
      DefaultSprite = Renderer.sprite;
      Direction = DefaultDirection;

      SetAlternateDirection();
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(
        grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation)
      )) {
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
        CompassDirection.North => Vector2IntC.North,
        CompassDirection.East => Vector2IntC.East,
        CompassDirection.South => Vector2IntC.South,
        CompassDirection.West => Vector2Int.left,
        CompassDirection.NorthEast => Vector2IntC.NorthEast,
        CompassDirection.NorthWest => Vector2IntC.NorthWest,
        CompassDirection.SouthEast => Vector2IntC.SouthEast,
        CompassDirection.SouthWest => Vector2IntC.SouthWest,
        var _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "No Arrow direction specified!"),
      };
    }
  }
}
