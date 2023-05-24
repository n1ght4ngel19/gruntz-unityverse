using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Itemz.Toolz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Rock : MapObject {
    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(OwnLocation, true);
    }

    private void Update() {
      // If a Grunt that is beside the Rock and has Gauntletz equipped and is targeting the Rock, break the Rock
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(
        grunt => grunt.Navigator.OwnNode.Neighbours.Contains(OwnNode)
          && grunt.HasTool(ToolType.Gauntletz)
          && grunt.TargetObject.Equals(this)
      )) {
        enabled = false;
        StartCoroutine(((Gauntletz)grunt.Equipment.Tool).BreakRock(grunt));
      }
    }

    /// <summary>
    /// Destroys the Rock.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> since this is a <see cref="Coroutine"/></returns>
    public IEnumerator Break() {
      OwnAnimator.Play("RockBreak");

      yield return new WaitForSeconds(1);

      LevelManager.Instance.Rockz.Remove(this);
      LevelManager.Instance.SetBlockedAt(OwnLocation, false);
      Destroy(gameObject);
    }
  }
}
