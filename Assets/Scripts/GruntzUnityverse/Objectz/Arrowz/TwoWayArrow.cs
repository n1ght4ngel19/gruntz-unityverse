using System;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class TwoWayArrow : Arrow {
    [field: SerializeField] public Direction DefaultDirection { get; set; }
    private Direction AlternateDirection { get; set; }
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
        Direction.North => Direction.South,
        Direction.East => Direction.West,
        Direction.South => Direction.North,
        Direction.West => Direction.East,
        Direction.Northeast => Direction.Southwest,
        Direction.Northwest => Direction.Southeast,
        Direction.Southeast => Direction.Northwest,
        Direction.Southwest => Direction.Northeast,
        var _ => throw new ArgumentOutOfRangeException(),
      };
    }
  }
}
