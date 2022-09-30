using System.Collections;
using System.Collections.Generic;

using Singletonz;

using Switchez;

using UnityEngine;

namespace Bridgez {
  public class WaterBridge : MonoBehaviour {
    public SpriteRenderer spriteRenderer;

    // You can only assign a BTS or a BHS to a WaterBridge
    public BlueToggleSwitch blueToggleSwitch;

    // You can only assign a BTS or a BHS to a WaterBridge
    public BlueHoldSwitch blueHoldSwitch;
    public List<Sprite> animFrames;

    private void Update() {
      if (blueToggleSwitch) {
        HandleBlueSwitch(blueToggleSwitch.isPressed);
      } else if (blueHoldSwitch) {
        HandleBlueSwitch(blueHoldSwitch.isPressed);
      }
    }

    private void HandleBlueSwitch(bool isPressed) {
      switch (isPressed) {
        case true: {
          StartCoroutine(RaiseBridge());
          MapManager.Instance.AddNavTileAt(transform.position);

          break;
        }
        case false: {
          StartCoroutine(LowerBridge());
          MapManager.Instance.RemoveNavTileAt(transform.position);

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
