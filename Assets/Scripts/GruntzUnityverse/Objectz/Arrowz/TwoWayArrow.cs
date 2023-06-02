using System;
using Animancer;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class TwoWayArrow : Arrow {
    private Direction DefaultDirection { get; set; }
    private Direction AlternativeDirection { get; set; }
    private Sprite DefaultSprite { get; set; }
    [field: SerializeField] public Sprite AlternativeSprite { get; set; }


    protected override void Start() {
      base.Start();

      DefaultSprite = Renderer.sprite;
      DefaultDirection = Direction;

      AlternativeDirection = OppositeDirectionOf(DefaultDirection);
    }

    public void ChangeDirection() {
      Direction = Direction.Equals(DefaultDirection) ? AlternativeDirection : DefaultDirection;
      Renderer.sprite = Direction.Equals(DefaultDirection) ? DefaultSprite : AlternativeSprite;
    }

    private Direction OppositeDirectionOf(Direction direction) {
      return DefaultDirection switch {
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
