using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Ai {
  public class DumbChaser : MonoBehaviour {
    public int aggroRange;
    public Node startingNode;

    public bool hasAggro;
    public bool hasCleaned;
    public bool hasAlready;

    private Grunt _self;
    private Grunt _targetGrunt;

    private void Start() {
      _self = gameObject.GetComponent<Grunt>();
    }

    private void Update() {
      startingNode ??= _self.navigator.ownNode;
      hasAggro = GameManager.Instance.currentLevelManager.playerGruntz.Any(IsWithinAggroRange);

      if (hasAggro) {
        hasCleaned = false;

        if (!hasAlready) {
          hasAlready = true;
          _self.targetGrunt = GameManager.Instance.currentLevelManager.playerGruntz.First(IsWithinAggroRange);
          _self.navigator.targetNode = _self.targetGrunt.navigator.ownNode;
          _self.haveActionCommand = true;
        }
      } else if (!hasCleaned) {
        hasCleaned = true;
        hasAlready = false;

        _self.CleanState();
        _self.navigator.targetNode = startingNode;
        _self.haveMoveCommand = true;
      }

      // DefendArea();
    }

    public void DefendArea() {
      if (GameManager.Instance.currentLevelManager.playerGruntz.Any(IsWithinAggroRange)) {
        Grunt potentialTarget = GameManager.Instance.currentLevelManager.playerGruntz.First(IsWithinAggroRange);

        _self.targetGrunt = potentialTarget;

        _self.navigator.targetNode = _self.targetGrunt.navigator.ownNode;
        _self.haveActionCommand = true;

        // return;

        _self.CleanState();

        Grunt targetGrunt = GameManager.Instance.currentLevelManager.playerGruntz.First(IsWithinAggroRange);
        _self.targetGrunt = targetGrunt;
        _self.navigator.targetNode = targetGrunt.navigator.ownNode;
        _self.haveActionCommand = true;
      } else {
        _self.CleanState();

        _self.navigator.targetNode = startingNode;
        _self.haveMoveCommand = true;
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
