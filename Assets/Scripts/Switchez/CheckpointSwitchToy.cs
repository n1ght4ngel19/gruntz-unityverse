using System.Collections.Generic;
using System.Linq;

using Singletonz;

using UnityEngine;

namespace Switchez {
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
