using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapTool : MapObject {
    [field: SerializeField] public ItemTool Tool { get; set; }

    protected override void Start() {
      base.Start();
      Animator.Play("MapItem_Spinning");
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        enabled = false;
        Renderer.enabled = false;
        StatzManager.Instance.acquiredToolz++;
        grunt.Equipment.Tool = Tool;
        grunt.Animator.runtimeAnimatorController = Tool.GruntAnimOverrider;

        StartCoroutine(grunt.PickupItem(this));

        break;
      }
    }
  }
}
