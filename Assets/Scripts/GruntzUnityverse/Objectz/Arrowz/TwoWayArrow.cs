using System;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class TwoWayArrow : Arrow {
    private Sprite _initialSprite;
    public Sprite changedSprite;
    private Direction _initialDirection;
    private Direction _changedDirection;


    protected override void Start() {
      base.Start();

      _initialSprite = spriteRenderer.sprite;
      _initialDirection = direction;
      _changedDirection = OppositeOf(_initialDirection);
    }

    public void ChangeDirection() {
      direction = direction.Equals(_initialDirection) ? _changedDirection : _initialDirection;
      spriteRenderer.sprite = direction.Equals(_initialDirection) ? _initialSprite : changedSprite;
    }

    private static Direction OppositeOf(Direction dir) {
      return dir switch {
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
