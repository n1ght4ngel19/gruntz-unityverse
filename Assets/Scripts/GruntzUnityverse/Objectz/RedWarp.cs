using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class RedWarp : MapObject {
    public MapObject target;

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt.AtLocation(location))) {
        TeleportTo(target.location, grunt);
      }
    }

    private void TeleportTo(Vector2Int teleportLocation, Grunt grunt) {
      grunt.transform.position = new Vector3(teleportLocation.x, teleportLocation.y, grunt.transform.position.z);
      grunt.navigator.ownLocation = teleportLocation;
      grunt.navigator.ownNode = LevelManager.Instance.NodeAt(teleportLocation);
      grunt.navigator.targetLocation = teleportLocation;
      grunt.navigator.previousLocation = Vector2Int.zero;
      grunt.navigator.savedTargetLocation = Vector2Int.zero;
      grunt.navigator.pathStart = null;
      grunt.navigator.pathEnd = null;
    }
  }
}
