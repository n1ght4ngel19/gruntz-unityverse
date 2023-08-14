using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Ai {
  public class DumbChaser : MonoBehaviour {
    public int aggroRange;
    private Grunt _self;
    private bool _hasTarget;

    private void Start() {
      _self = gameObject.GetComponent<Grunt>();
    }


    private void Update() {
      DefendAreaAroundSelf();
    }

    public void DefendAreaAroundSelf() {
      foreach (Grunt grunt in LevelManager.Instance.playerGruntz) {
        Vector2Int absGruntLocation = AbsVector2Int(grunt.navigator.ownLocation.x, grunt.navigator.ownLocation.y);

        Vector2Int absSelfStartingLocation = AbsVector2Int(
          _self.navigator.startingLocation.x,
          _self.navigator.startingLocation.y
        );

        Vector2Int absDistance = absGruntLocation - absSelfStartingLocation;
        absDistance = AbsVector2Int(absDistance.x, absDistance.y);

        if (absDistance.x <= aggroRange && absDistance.y <= aggroRange) {
          if (!_hasTarget) {
            _hasTarget = true;
            _self.haveActionCommand = true;
          }
        } else {
          _hasTarget = false;
          _self.haveActionCommand = true;
        }
      }
    }

    public Vector2Int AbsVector2Int(int x, int y) {
      return new Vector2Int(Mathf.Abs(x), Mathf.Abs(y));
    }
  }
}
