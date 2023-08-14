using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Itemz;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapTool : MapItem {
    public Tool pickupTool;


    protected override void Start() {
      base.Start();

      pickupTool = gameObject.GetComponent<Tool>();
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredToolz++;
        grunt.equipment.tool = pickupTool;

        StartCoroutine(grunt.PickupItem(pickupTool, nameof(Tool), pickupTool.GetType().Name));

        break;
      }
    }
  }
}
