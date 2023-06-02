using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapTool : MapItem {
    [field: SerializeField] public Tool PickupTool { get; set; }

    // Todo: Redo
    private string Type { get; set; }


    protected override void Start() {
      base.Start();
      PickupTool = gameObject.GetComponent<Tool>();
      Type = PickupTool.Type.ToString();
      RotationAnimation = Resources.Load<AnimationClip>($"Animationz/MapItemz/Tool/Clipz/{Type}");
      Animancer.Play(RotationAnimation);
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        SetEnabled(false);

        StatzManager.Instance.acquiredToolz++;
        grunt.Equipment.Tool = PickupTool;

        StartCoroutine(grunt.PickupItem("Tool", Type));

        break;
      }
    }
  }
}
