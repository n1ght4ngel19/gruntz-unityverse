using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapTool : MapItem {
    [field: SerializeField] public Tool PickupTool { get; set; }


    protected override void Start() {
      base.Start();
      PickupTool = gameObject.GetComponent<Tool>();
      RotationAnimation = Resources.Load<AnimationClip>($"Animationz/MapItemz/Tool/Clipz/{PickupTool.GetType().Name}");
      animancer.Play(RotationAnimation);
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt.AtLocation(location))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredToolz++;
        grunt.equipment.tool = PickupTool;

        StartCoroutine(grunt.PickupItem("Tool", PickupTool.GetType().Name));

        break;
      }
    }
  }
}
