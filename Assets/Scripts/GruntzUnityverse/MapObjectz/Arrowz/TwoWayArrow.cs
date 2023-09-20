using System;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Arrowz {
  public class TwoWayArrow : Arrow {
    private Sprite _initialSprite;
    public Sprite changedSprite;
    private Direction _initialDirection;
    private Direction _changedDirection;


    protected override void Start() {
      base.Start();

      _initialDirection = direction;
      _changedDirection = OppositeOf(_initialDirection);

      _initialSprite = spriteRenderer.sprite;
      Addressables.LoadAssetAsync<Sprite>($"Assets/Spritez/Objectz/Arrowz/Arrow_2W_{_changedDirection}.png")
        .Completed += handle => {
        changedSprite = handle.Result;
      };
    }
    // -------------------------------------------------------------------------------- //

    public void ChangeDirection() {
      direction = direction.Equals(_initialDirection) ? _changedDirection : _initialDirection;
      spriteRenderer.sprite = direction.Equals(_initialDirection) ? _initialSprite : changedSprite;
      nodeInDirection = GameManager.Instance.currentLevelManager.NodeAt(location + Vector2Direction.FromDirection(direction));
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
