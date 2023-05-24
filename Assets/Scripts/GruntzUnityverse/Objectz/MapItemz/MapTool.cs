using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapTool : MapObject {
    [field: SerializeField] public Tool PickupTool { get; set; }


    protected override void Start() {
      base.Start();
      PickupTool = gameObject.GetComponent<Tool>();
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        DeactivateSelf();
        StatzManager.Instance.acquiredToolz++;

        // Todo: Move into PickupItem function inside Grunt
        grunt.Equipment.Tool = PickupTool;
        grunt.Animator.runtimeAnimatorController = PickupTool.GruntAnimOverrider;
        // Todo: -------------------------------------------

        StartCoroutine(grunt.PickupItem(this));

        break;
      }
    }
  }
}
