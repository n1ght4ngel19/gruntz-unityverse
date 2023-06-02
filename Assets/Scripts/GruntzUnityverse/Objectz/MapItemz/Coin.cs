using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class Coin : MapItem {
    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        SetEnabled(false);
        StatzManager.Instance.acquiredCoinz++;

        StartCoroutine(grunt.PickupItem("Misc", nameof(Coin)));

        break;
      }
    }
  }
}
