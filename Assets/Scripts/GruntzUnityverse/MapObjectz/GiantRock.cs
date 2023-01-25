using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class GiantRock : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> blowUpFrames;
    public bool isToBeBroken;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    // private void Update() {
    //   if (isGauntletzGruntAdjacent() && isToBeBroken) {
    //     StartCoroutine(BlowUpRock());
    //   }
    // }

    // private IEnumerator BlowUpRock() {
    //   foreach (Sprite frame in blowUpFrames) {
    //     Renderer.sprite = frame;
    //     yield return null;
    //   }
    //
    //   yield return StartCoroutine(DestroyRock());
    // }

    // private IEnumerator DestroyRock() {
    //   MapManager.Instance.AddNavTileAt(transform.position);
    //   Destroy(gameObject);
    //
    //   yield return null;
    // }

    // TODO: Redo / Remove
    private bool isGauntletzGruntAdjacent() {
      return LevelManager.Instance.gruntz
        .Any(grunt => grunt.tool == ToolType.Gauntletz
                      && ((Vector2)grunt.transform.position + Vector2.up * 2 == (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.up * 2 + Vector2.left ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.up * 2 + Vector2.left * 2 ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.up * 2 + Vector2.right ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.up * 2 + Vector2.right * 2 ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.down * 2 == (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.down * 2 + Vector2.left ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.down * 2 + Vector2.left * 2 ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.down * 2 + Vector2.right ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.left * 2 + Vector2.up ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.left * 2 + Vector2.down ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.right * 2 + Vector2.up ==
                          (Vector2)transform.position
                          || (Vector2)grunt.transform.position + Vector2.right * 2 + Vector2.down ==
                          (Vector2)transform.position)
        );
    }
  }
}
