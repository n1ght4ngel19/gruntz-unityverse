using System.Collections.Generic;
using System.Linq;

using GruntzUnityverse.Actorz;
using GruntzUnityverse.Singletonz;

using UnityEngine;

namespace GruntzUnityverse.Itemz {
  public class Tool : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    private List<Sprite> animFrames;
    public ToolType type;
    private const int FrameRate = 12;

    private void Start() {
      animFrames = Resources.LoadAll<Sprite>($"Animations/Itemz/Toolz/Tool{type}").ToList();
    }

    private void Update() {
      foreach (Grunt grunt in MapManager.Instance.gruntz
                 .Where(grunt => (Vector2)grunt.transform.position == (Vector2)transform.position)
      ) {
        grunt.tool = type;
        grunt.SwitchGruntAnimations(type);
        
        Destroy(gameObject);
      }
      
      int frame = (int)(Time.time * FrameRate % animFrames.Count);

      spriteRenderer.sprite = animFrames[frame];
    }
  }
}
