using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.PathFinding {
    public class NavComponent : MonoBehaviour {
        public Vector2Int OwnGridLocation { get; set; }
        public Vector2Int TargetGridLocation { get; set; }
        private Node StartNode { get; set; }
        private Node EndNode { get; set; }
        private List<Node> NodePath { get; set; }

        private void Start() {
            OwnGridLocation = Vector2Int.FloorToInt(gameObject.transform.position);
            TargetGridLocation = OwnGridLocation;
        }

        public void HandleMovement() {
            DeterminePath();
            
            MoveAlongPath();
        }

        public void DeterminePath() {
            if (TargetGridLocation.Equals(Vector2Int.zero)) {
                return;
            }

            EndNode = LevelManager.Instance.mapNodes.First(node =>
                node.GridLocation.Equals(TargetGridLocation));

            if (EndNode.isBlocked) {
                return;
            }

            // TODO: Move to the closest adjacent Node, if possible
            // if (EndNode.isBlocked && thereIsAFreeAdjacentPosition)

            StartNode = LevelManager.Instance.mapNodes.First(node =>
                node.GridLocation.Equals(OwnGridLocation));

            NodePath = PathFinder.PathBetween(StartNode, EndNode);
        }

        public void MoveAlongPath() {
            if (NodePath == null) {
                return;
            }
            
            if (NodePath.Count <= 1) {
                return;
            }

            // TODO: Rework
            // Setting the Grunt's GridLocation before moving to unblocked
            LevelManager.Instance.mapNodes.First(node =>
                node.GridLocation.Equals(OwnGridLocation)).isBlocked = false;

            Vector3 nextStop = new(
                NodePath[1].GridLocation.x + 0.5f,
                NodePath[1].GridLocation.y + 0.5f,
                -5
            );

            // Vector3 parentPosition = gameObject.transform.position;
                
            if (Vector2.Distance(nextStop, gameObject.transform.position) > 0.025f) {
                Vector3 moveDir = (nextStop - gameObject.transform.position).normalized;
                moveDir.x = Mathf.Round(moveDir.x);
                moveDir.y = Mathf.Round(moveDir.y);
                // moveDir.z = -5;

                // Increased speed for testing (Too fast is buggy!)
                gameObject.transform.position += moveDir * (Time.deltaTime / 0.3f);
                // Real speed => parentPosition += moveDir * (Time.deltaTime / TimePerTile); 
                
                // TODO: Rework
                OwnGridLocation = Vector2Int.FloorToInt(gameObject.transform.position);
                // Setting the Grunt's GridLocation before moving to blocked
                // LevelManager.Instance.mapNodes.ForEach(node => node.isBlocked = true);
                LevelManager.Instance.mapNodes.First(node =>
                    node.GridLocation.Equals(OwnGridLocation)).isBlocked = true;

            } else {
                // Moving on to the next Node
                NodePath.RemoveAt(1);
            }
        }
    }
}
