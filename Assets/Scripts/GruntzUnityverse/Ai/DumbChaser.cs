using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Ai {
  public class DumbChaser : MonoBehaviour {
    public int aggroRange;
    [HideInInspector] public Node startingNode;
    [HideInInspector] public Grunt self;
    [HideInInspector] public List<Grunt> targetz;
    [HideInInspector] public Node targetCurrentNode;

    private void OnValidate() {
      gameObject.GetComponentInChildren<RangeRect>().transform.localScale = new Vector3(aggroRange * 2 + 1, aggroRange * 2 + 1, 1);
    }

    private void Start() {
      self = gameObject.GetComponent<Grunt>();
      targetz = new List<Grunt>();
    }

    private void Update() {
      if (self.state == GruntState.Playing) {
        return;
      }

      startingNode ??= self.navigator.ownNode;

      targetz = GameManager.Instance.currentLevelManager.playerGruntz.Where(IsWithinAggroRange).ToList();

      if (targetz.Count > 0) {
        // Todo?: Select weakest Grunt
        if (self.targetGrunt == null) {
          self.haveMovingToAttackingCommand = true;
          self.targetGrunt = targetz[0];
          self.navigator.targetNode = self.targetGrunt.navigator.ownNode;
          targetCurrentNode = self.targetGrunt.navigator.ownNode;
          // grunt.hasPlayedDizgruntledAttackSound = false;
        } else if (self.targetGrunt.navigator.ownNode != targetCurrentNode) {
          targetCurrentNode = self.targetGrunt.navigator.ownNode;

          if (!self.IsNeighbourOf(self.targetGrunt)) {
            self.haveMovingToAttackingCommand = true;
            self.navigator.targetNode = self.targetGrunt.navigator.ownNode;
          }
        }
      } else {
        self.CleanState();
        self.navigator.targetNode = startingNode;
        self.haveMoveCommand = true;
      }
    }

    private bool IsWithinAggroRange(Grunt grunt) {
      return grunt.team == Team.Player &&
        grunt.navigator.ownNode.location.x <= startingNode.location.x + aggroRange &&
        grunt.navigator.ownNode.location.x >= startingNode.location.x - aggroRange &&
        grunt.navigator.ownNode.location.y <= startingNode.location.y + aggroRange &&
        grunt.navigator.ownNode.location.y >= startingNode.location.y - aggroRange;
    }

    public Vector2Int AbsVector2Int(int x, int y) {
      return new Vector2Int(Mathf.Abs(x), Mathf.Abs(y));
    }
  }
}
