using System.Collections.Generic;
using System.Linq;

using Singletonz;

using UnityEngine;

public class Tool : MonoBehaviour {
  public SpriteRenderer spriteRenderer;
  public List<Sprite> animFrames;
  public ToolType type;
  private const int FrameRate = 12;
  
  private void Update() {
    foreach (Grunt grunt in MapManager.Instance.gruntz
               .Where(grunt => (Vector2)grunt.transform.position == (Vector2)transform.position)
    ) {
      grunt.tool = type;
      
      Destroy(gameObject);
    }
    
    
    int frame = (int)(Time.time * FrameRate % animFrames.Count);

    spriteRenderer.sprite = animFrames[frame];
  }
}
