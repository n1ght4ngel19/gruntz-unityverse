using GruntzUnityverse.Enumz;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using System.Linq;

namespace GruntzUnityverse.Objectz.MapItemz.Misc {
  public class Warpletter : MapItem {
    public WarpletterType warpletterType;

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredWarpletterz++;

        StartCoroutine(grunt.PickupMiscItem($"{nameof(Warpletter)}{warpletterType}"));

        break;
      }
    }
  }
}
