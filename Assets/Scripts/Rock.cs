using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Singletonz;

using UnityEngine;

public class Rock : MonoBehaviour {
  public SpriteRenderer spriteRenderer;
  public List<Sprite> blowUpFrames;
  public bool isToBeBroken;

  private void Update() {
    if (isToBeBroken && isGauntletzGruntAdjacent()) {
      StartCoroutine(BlowUpRock());
    }
  }

  private IEnumerator BlowUpRock() {
    foreach (Sprite frame in blowUpFrames) {
      spriteRenderer.sprite = frame;
      yield return null;
    }
    
    yield return StartCoroutine(destroyRock());
  }

  IEnumerator destroyRock() {
    MapManager.Instance.AddNavTileAt(transform.position);
    Destroy(gameObject);

    yield return null;
  }

  private bool isGauntletzGruntAdjacent() {
    return MapManager.Instance.gruntz
      .Any(grunt => grunt.tool == ToolType.Gauntletz
                    && ((Vector2)grunt.transform.position + Vector2.up == (Vector2)transform.position
                        || (Vector2)grunt.transform.position + Vector2.down == (Vector2)transform.position
                        || (Vector2)grunt.transform.position + Vector2.left == (Vector2)transform.position
                        || (Vector2)grunt.transform.position + Vector2.right == (Vector2)transform.position
                        || (Vector2)grunt.transform.position + Vector2.up + Vector2.left == (Vector2)transform.position
                        || (Vector2)grunt.transform.position + Vector2.up + Vector2.right == (Vector2)transform.position
                        || (Vector2)grunt.transform.position + Vector2.down + Vector2.left == (Vector2)transform.position
                        || (Vector2)grunt.transform.position + Vector2.down + Vector2.right == (Vector2)transform.position)
      );
  }
}
