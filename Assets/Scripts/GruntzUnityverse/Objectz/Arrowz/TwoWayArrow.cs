using System;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class TwoWayArrow : Arrow {
    [field: SerializeField] public Sprite ChangedSprite { get; set; }
    private Direction InitialDirection { get; set; }
    private Direction ChangedDirection { get; set; }
    private Sprite InitialSprite { get; set; }


    protected override void Start() {
      base.Start();

      InitialSprite = Renderer.sprite;
      InitialDirection = Direction;

      ChangedDirection = OppositeDirectionOf(InitialDirection);
    }

    public void ChangeDirection() {
      Direction = Direction.Equals(InitialDirection) ? ChangedDirection : InitialDirection;
      Renderer.sprite = Direction.Equals(InitialDirection) ? InitialSprite : ChangedSprite;
    }

    private Direction OppositeDirectionOf(Direction direction) {
      return InitialDirection switch {
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
