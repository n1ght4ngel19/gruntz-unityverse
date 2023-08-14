using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.MapItemz.Misc {
  public class Coin : MapItem {
    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredCoinz++;

        StartCoroutine(grunt.PickupMiscItem(nameof(Coin)));

        break;
      }
    }
  }
}
