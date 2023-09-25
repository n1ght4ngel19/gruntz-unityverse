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
      _changedDirection = DirectionUtility.OppositeOf(_initialDirection);

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
  }
}
