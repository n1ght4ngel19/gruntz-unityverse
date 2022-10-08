using System.Collections.Generic;
using System.Linq;

using GruntzUnityverse.Actorz;
using GruntzUnityverse.Singletonz;

using UnityEngine;

namespace GruntzUnityverse.Itemz {
  public class Tool : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public List<Sprite> animFrames;
    public ToolType toolType;
    private const int FrameRate = 12;
    
    private void Update() {
      foreach (Grunt grunt in MapManager.Instance.gruntz
                 .Where(grunt => (Vector2)grunt.transform.position == (Vector2)transform.position)
      ) {
        grunt.tool = toolType;
        grunt.SwitchGruntAnimations(toolType);
        
        Destroy(gameObject);
      }
      
      int frame = (int)(Time.time * FrameRate % animFrames.Count);

      spriteRenderer.sprite = animFrames[frame];
    }
  }
}
