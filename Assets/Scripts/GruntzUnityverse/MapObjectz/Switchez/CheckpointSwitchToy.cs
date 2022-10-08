using System.Collections.Generic;
using System.Linq;

using GruntzUnityverse.Itemz;
using GruntzUnityverse.Singletonz;

using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class CheckpointSwitchToy : MonoBehaviour {
    public ToyType requirement;
    public bool isChecked;
    public List<Sprite> animFrames;
    public SpriteRenderer spriteRenderer;

    private void Update() {
      if (isChecked) {
        return;
      }

      if (MapManager.Instance.gruntz
        .Any(grunt => (Vector2)grunt.transform.position == (Vector2)transform.position
                      && grunt.toy == requirement)
      ) {
        isChecked = true;
        spriteRenderer.sprite = animFrames[1];
      }
    }
  }
}
