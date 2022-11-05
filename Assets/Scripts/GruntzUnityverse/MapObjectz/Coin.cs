using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Singletonz;
using GruntzUnityverse.Utilitiez;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class Coin : MonoBehaviour {
    public SpriteRenderer spriteRenderer;

    public List<Sprite> animFrames;
    private const int FrameRate = 12;

    private void Start() {
      animFrames = Resources.LoadAll<Sprite>("Animations/MapObjectz/Misc/Coin").ToList();
    }

    private void Update() {
      foreach (
        Grunt grunt in MapManager.Instance.gruntz
          .Where(grunt1 => Vector2Plus.AreEqual(grunt1.transform.position, transform.position))
      ) {
        RemoveFromGame();
      }

      int frame = (int)(Time.time * FrameRate % animFrames.Count);
      spriteRenderer.sprite = animFrames[frame];
    }

    private void RemoveFromGame() {
      StatzManager.Instance.acquiredCoinz++;
      Destroy(gameObject);
    }
  }
}
