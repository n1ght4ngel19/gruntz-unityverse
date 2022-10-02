using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Singletonz;

using UnityEngine;

public class Rock : MonoBehaviour {
  public SpriteRenderer spriteRenderer;
  public List<Sprite> blowUpFrames;
  public bool isToBeBroken;
  public Vector2 twoDimPosition;

  private void Update() {
    twoDimPosition = transform.position;

    if (isToBeBroken && IsGauntletzGruntAdjacent()) {
      StartCoroutine(BlowUpRock());
    }
  }

  private IEnumerator BlowUpRock() {
    foreach (Sprite frame in blowUpFrames) {
      spriteRenderer.sprite = frame;
      yield return null;
    }
    
    yield return StartCoroutine(DestroyRock());
  }

  private IEnumerator DestroyRock() {
    MapManager.Instance.AddNavTileAt(transform.position);
    MapManager.Instance.rockz.RemoveAll(rock => rock.twoDimPosition == twoDimPosition);
    Destroy(gameObject);

    yield return null;
  }

  private bool IsGauntletzGruntAdjacent() {
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
