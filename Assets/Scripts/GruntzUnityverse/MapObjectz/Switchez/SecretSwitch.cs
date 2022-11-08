using System.Collections;
using System.Linq;

using GruntzUnityverse.Singletonz;

using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class SecretSwitch : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public bool isUntouched;
    public Sprite[] animFrames;
    private const float TimeStep = 0.1f;

    private void Start() {
      isUntouched = true;
    }

    private void Update() {
      if (isUntouched) {
        if (
          MapManager.Instance.gruntz.Any(grunt1 => (Vector2)grunt1.transform.position == (Vector2)transform.position)
        ) {
          spriteRenderer.sprite = animFrames[1];
          isUntouched = false;

          foreach (SecretTile secretTile in MapManager.Instance.secretTilez) {
            StartCoroutine(HandleSecretTile(secretTile));
          }
        }
      }
    }

    private IEnumerator HandleSecretTile(SecretTile secretTile) {
      while (secretTile.delay > 0) {
        secretTile.delay -= TimeStep;
        yield return new WaitForSeconds(TimeStep);
      }
      
      secretTile.GetComponent<SpriteRenderer>().enabled = true;

      if (secretTile.isWalkable) {
        MapManager.Instance.AddNavTileAt(secretTile.transform.position);
      }

      while (secretTile.duration > 0) {
        secretTile.duration -= TimeStep;
        yield return new WaitForSeconds(TimeStep);
      }

      if (secretTile.isWalkable) {
        MapManager.Instance.RemoveNavTileAt(secretTile.transform.position);
      }

      Destroy(secretTile.gameObject);
    }
  }
}
