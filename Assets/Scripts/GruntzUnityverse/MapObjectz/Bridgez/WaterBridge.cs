using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Bridgez
{
  public class WaterBridge : MonoBehaviour
  {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation { get; set; }

    public List<Sprite> animFrames;


    // TODO: Fix that you can only assign a BTS OR a BHS to a WaterBridge
    public GruntzUnityverse.MapObjectz.Switchez.BlueToggleSwitch blueToggleSwitch;
    public GruntzUnityverse.MapObjectz.Switchez.BlueHoldSwitch blueHoldSwitch;

    private void Start() { GridLocation = Vector2Int.FloorToInt(transform.position); }

    private void Update()
    {
      // TODO : WTF?
      if (blueToggleSwitch)
      {
        HandleBlueSwitch(blueToggleSwitch.IsPressed);
      }
      else if (blueHoldSwitch)
      {
        HandleBlueSwitch(blueHoldSwitch.IsPressed);
      }
    }

    private void HandleBlueSwitch(bool isPressed)
    {
      switch (isPressed)
      {
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

    private IEnumerator LowerBridge()
    {
      foreach (Sprite frame in animFrames)
      {
        spriteRenderer.sprite = frame;

        yield return null;
      }
    }

    private IEnumerator RaiseBridge()
    {
      foreach (Sprite frame in animFrames.AsEnumerable()
        .Reverse())
      {
        spriteRenderer.sprite = frame;

        yield return null;
      }
    }
  }
}
