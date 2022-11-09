using System.Collections;
using System.Linq;
using GruntzUnityverse.Singletonz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class SecretSwitch : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public Sprite[] animFrames;

    public bool isUntouched;
    private const float TimeStep = 0.1f;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);

      isUntouched = true;
    }

    private void Update() {
      if (!isUntouched) {
        return;
      }

      if (MapManager.Instance.gruntz.Any(grunt => grunt.ownGridLocation.Equals(GridLocation))) {
        spriteRenderer.sprite = animFrames[1];
        isUntouched = false;

        foreach (SecretTile secretTile in MapManager.Instance.secretTilez) {
          StartCoroutine(HandleSecretTile(secretTile));
        }
      }
    }

    private IEnumerator HandleSecretTile(SecretTile secretTile) {
      while (secretTile.delay > 0) {
        secretTile.delay -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      secretTile.GetComponent<SpriteRenderer>().enabled = true;

      // if (secretTile.isWalkable) {
      //   MapManager.Instance.AddNavTileAt(secretTile.transform.position);
      // }

      while (secretTile.duration > 0) {
        secretTile.duration -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      // if (secretTile.isWalkable) {
      //   MapManager.Instance.RemoveNavTileAt(secretTile.transform.position);
      // }

      Destroy(secretTile.gameObject);
    }
  }
}
