using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapTool : MapObject {
    [field: SerializeField] public Tool PickupTool { get; set; }

    // Todo: Redo?
    private string Type { get; set; }


    protected override void Start() {
      base.Start();
      PickupTool = gameObject.GetComponent<Tool>();
      Type = PickupTool.Type.ToString();
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        DeactivateSelf();

        StatzManager.Instance.acquiredToolz++;
        grunt.Equipment.Tool = PickupTool;

        StartCoroutine(grunt.PickupItem(Type));

        break;
      }
    }
  }
}
