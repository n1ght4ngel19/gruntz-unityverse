using System.Collections.Generic;
using System.Linq;

using Singletonz;

using UnityEngine;

public class Item : MonoBehaviour {
  public SpriteRenderer spriteRenderer;
  public List<Sprite> animFrames;
  public ItemType type;
  private int frameRate = 12;
  
  private void Update() {
    foreach (Grunt grunt in MapManager.Instance.gruntz
               .Where(grunt => (Vector2)grunt.transform.position == (Vector2)transform.position)
    ) {
      grunt.tool = type;
      
      Destroy(gameObject);
    }
    
    
    int frame = (int)(Time.time * frameRate % animFrames.Count);

    spriteRenderer.sprite = animFrames[frame];
  }
}
