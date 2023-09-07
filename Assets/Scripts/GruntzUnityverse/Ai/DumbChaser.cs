using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Ai {
  public class DumbChaser : MonoBehaviour {
    public int aggroRange;
    public Node startingNode;
    private Grunt _self;
    private bool _hasTarget;

    private void Start() {
      _self = gameObject.GetComponent<Grunt>();
    }

    private void Update() {
      startingNode ??= _self.navigator.ownNode;

      DefendAreaAroundSelf();
    }

    public void DefendAreaAroundSelf() {
      if (GameManager.Instance.currentLevelManager.playerGruntz.Any(IsWithinAggroRange)) {
        if (_hasTarget) {
          return;
        }

        _self.CleanState();

        Grunt targetGrunt = GameManager.Instance.currentLevelManager.playerGruntz.First(IsWithinAggroRange);
        _hasTarget = true;
        _self.targetGrunt = targetGrunt;
        _self.navigator.targetNode = targetGrunt.navigator.ownNode;
        _self.haveActionCommand = true;
      } else {
        _self.CleanState();

        _hasTarget = false;
        _self.navigator.targetNode = startingNode;
        _self.navigator.haveMoveCommand = true;
      }
    }

    private bool IsWithinAggroRange(Grunt grunt) {
      return grunt.navigator.ownNode.location.x <= startingNode.location.x + aggroRange &&
        grunt.navigator.ownNode.location.x >= startingNode.location.x - aggroRange &&
        grunt.navigator.ownNode.location.y <= startingNode.location.y + aggroRange &&
        grunt.navigator.ownNode.location.y >= startingNode.location.y - aggroRange;
    }

    public Vector2Int AbsVector2Int(int x, int y) {
      return new Vector2Int(Mathf.Abs(x), Mathf.Abs(y));
    }
  }
}
