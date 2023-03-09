using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Hole : MapObject {
    [field: SerializeField] public bool IsOpen { get; set; }


    private void Update() {
      if (!IsOpen) {
        return;
      }

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(
        grunt => grunt.IsOnLocation(OwnLocation) && grunt.enabled
      )) {
        StartCoroutine(grunt.FallInHole());

        break;
      }
    }
  }
}
