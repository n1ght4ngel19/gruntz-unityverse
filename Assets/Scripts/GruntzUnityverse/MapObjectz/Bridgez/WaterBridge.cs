using System.Collections;
using System.Collections.Generic;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class WaterBridge : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> animFrames;


    // TODO: Fix that you can only assign a BTS OR a BHS to a WaterBridge
    public BlueToggleSwitch blueToggleSwitch;
    public BlueHoldSwitch blueHoldSwitch;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      // TODO : WTF?
      if (blueToggleSwitch) {
        HandleBlueSwitch(blueToggleSwitch.isPressed);
      }
      else if (blueHoldSwitch) {
        HandleBlueSwitch(blueHoldSwitch.isPressed);
      }
    }

    private void HandleBlueSwitch(bool isPressed) {
      switch (isPressed) {
        case true: {
          StartCoroutine(RaiseBridge());
          LevelManager.Instance.UnblockNodeAt(GridLocation);

          break;
        }
        case false: {
          StartCoroutine(LowerBridge());
          LevelManager.Instance.BlockNodeAt(GridLocation);

          break;
        }
      }
    }

    private IEnumerator LowerBridge() {
      foreach (Sprite frame in animFrames) {
        spriteRenderer.sprite = frame;

        yield return null;
      }
    }

    private IEnumerator RaiseBridge() {
      for (int i = animFrames.Count - 1; i >= 0; i--) {
        spriteRenderer.sprite = animFrames[i];

        yield return null;
      }
    }
  }
}
