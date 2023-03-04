using System;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class TwoWayArrow : Arrow {
    [field: SerializeField] public CompassDirection DefaultDirection { get; set; }
    private CompassDirection AlternateDirection { get; set; }
    private Sprite DefaultSprite { get; set; }
    [field: SerializeField] public Sprite AlternateSprite { get; set; }


    protected override void Start() {
      base.Start();

      DefaultSprite = Renderer.sprite;
      Direction = DefaultDirection;

      SetAlternateDirection();
    }

    public void ChangeDirection() {
      Direction = Direction.Equals(DefaultDirection) ? AlternateDirection : DefaultDirection;

      Renderer.sprite = Direction.Equals(DefaultDirection) ? DefaultSprite : AlternateSprite;
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
  }
}
