using System.Collections.Generic;

using Singletonz;

using Switchez;

using UnityEngine;

namespace Bridgez {
  public class WaterBridge : MonoBehaviour {
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
          MapManager.Instance.AddNavTileAt(transform.position);

          for (int i = animFrames.Count - 1; i >= 0; i--) {
            gameObject.GetComponent<SpriteRenderer>()
              .sprite = animFrames[i];
          }

          break;
        }
        case false: {
          MapManager.Instance.RemoveNavTileAt(transform.position);

          foreach (Sprite frame in animFrames) {
            gameObject.GetComponent<SpriteRenderer>()
              .sprite = frame;
          }

          break;
        }
      }
    }
  }
}
