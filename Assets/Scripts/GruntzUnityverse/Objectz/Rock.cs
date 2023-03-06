using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Rock : MapObject {
    [field: SerializeField] public bool IsInitialized { get; set; }
    public bool isBlockedHere;

    private void Update() {
      if (!IsInitialized) {
        InitializeNodeAtOwnLocation();

        return;
      }

      Node ownNode = LevelManager.Instance.nodeList.First(node => node.GridLocation.Equals(OwnLocation));

      // If a Grunt that is beside the Rock and has Gauntletz equipped and is targeting the Rock, break the Rock
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz) {
        if (LevelManager.Instance.nodeList.First(node => node.GridLocation.Equals(grunt.NavComponent.OwnLocation))
            .Neighbours.Contains(ownNode)
          && grunt.HasTool("Gauntletz")
          && grunt.TargetObject.Equals(this)) {
          enabled = false;
          StartCoroutine(grunt.BreakRock());
        }
      }
    }

    public IEnumerator Break() {
      Animator.Play("RockBreak");

      yield return new WaitForSeconds(1);

      LevelManager.Instance.Rockz.Remove(this);
      LevelManager.Instance.SetBlockedAt(OwnLocation, false);
      Destroy(gameObject);
    }

    protected void InitializeNodeAtOwnLocation() {
      IsInitialized = true;
      LevelManager.Instance.SetBlockedAt(OwnLocation, true);
    }
  }
}
