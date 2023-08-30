using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Ai {
  public class DumbChaser : MonoBehaviour {
    public int aggroRange;
    private Grunt _self;
    private bool _hasTarget;
    public Node startingNode;

    private void Start() {
      _self = gameObject.GetComponent<Grunt>();
    }


    private void Update() {
      startingNode ??= _self.navigator.ownNode;

      DefendAreaAroundSelf();
    }

    public void DefendAreaAroundSelf() {
      foreach (Grunt grunt in LevelManager.Instance.playerGruntz) {
        Node gruntNode = grunt.navigator.ownNode;

        if (IsWithinAggroRange(gruntNode)) {
          // Todo: Look for weaker opponent if possible, don't try to attack stronger opponents
          if (!_hasTarget) {
            _self.CleanState();

            _hasTarget = true;
            _self.targetGrunt = grunt;
            _self.navigator.targetNode = grunt.navigator.ownNode;
            _self.haveActionCommand = true;
          }
        } else {
          _hasTarget = false;
          _self.haveActionCommand = false;
          _self.navigator.targetNode = startingNode;
          _self.navigator.haveMoveCommand = true;
        }


        Vector2Int absGruntLocation = AbsVector2Int(grunt.navigator.ownLocation.x, grunt.navigator.ownLocation.y);

        Vector2Int absSelfStartingLocation = AbsVector2Int(_self.navigator.startingLocation.x,
          _self.navigator.startingLocation.y);

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

    private bool IsWithinAggroRange(Node gruntNode) {
      return gruntNode.location.x <= startingNode.location.x + aggroRange &&
        gruntNode.location.x >= startingNode.location.x - aggroRange &&
        gruntNode.location.y <= startingNode.location.y + aggroRange &&
        gruntNode.location.y >= startingNode.location.y - aggroRange;
    }

    public Vector2Int AbsVector2Int(int x, int y) {
      return new Vector2Int(Mathf.Abs(x), Mathf.Abs(y));
    }
  }
}
