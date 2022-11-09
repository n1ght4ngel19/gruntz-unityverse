using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Itemz;
using GruntzUnityverse.Singletonz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class Rock : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> blowUpFrames;
    public bool isToBeBroken;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);

      MapManager.Instance.mapNodes
        .First(node => node.GridLocation.Equals(GridLocation)).isBlocked = true;
    }

    private void Update() {
      // if (isToBeBroken && IsGauntletzGruntAdjacent()) {
      //   StartCoroutine(BlowUpRock());
      // }
    }

    // private IEnumerator BlowUpRock() {
    //   foreach (Sprite frame in blowUpFrames) {
    //     spriteRenderer.sprite = frame;
    //     yield return null;
    //   }
    //
    //   yield return StartCoroutine(DestroyRock());
    // }

    // private IEnumerator DestroyRock() {
    //   MapManager.Instance.AddNavTileAt(transform.position);
    //   MapManager.Instance.rockz.RemoveAll(rock => rock.twoDimPosition == twoDimPosition);
    //   Destroy(gameObject);
    //
    //   yield return null;
    // }

    // TODO: Redo / Remove
    private bool IsGauntletzGruntAdjacent() {
      return MapManager.Instance.gruntz
        .Any(grunt => grunt.tool == ToolType.Gauntletz
                      && ((Vector2)grunt.transform.position + Vector2.up == (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.down == (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.left == (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.right == (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.up + Vector2.left ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.up + Vector2.right ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.down + Vector2.left ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.down + Vector2.right ==
                          (Vector2)transform.position)
        );
    }
  }
}
