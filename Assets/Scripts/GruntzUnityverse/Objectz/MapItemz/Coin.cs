using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class Coin : MapObject {
    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        enabled = false;
        Renderer.enabled = false;
        StatzManager.Instance.acquiredCoinz++;

        StartCoroutine(grunt.PickupItem(nameof(Coin)));

        break;
      }
    }
  }
}
