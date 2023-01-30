using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.PathFinding;
using UnityEngine;

namespace _Test {
    public class TNavComponent : MonoBehaviour {
        [field: SerializeField] public Vector2Int OwnLocation { get; set; }
        [field: SerializeField] public Vector2Int TargetLocation { get; set; }
        [field: SerializeField] public Vector2Int SavedTargetLocation { get; set; }

        #region Pathfinding
            [field: SerializeField] public Node PathStart { get; set; }
            [field: SerializeField] public Node PathEnd { get; set; }
            [field: SerializeField] public List<Node> Path { get; set; }
        #endregion
        
        [field: SerializeField] public bool IsMoving { get; set; }
        [field: SerializeField] public bool HasSavedTarget { get; set; }

        public void MoveTowardsTarget() {
            PathStart = LevelManager.Instance.mapNodes.First(node =>
                node.GridLocation.Equals(OwnLocation));

            PathEnd = LevelManager.Instance.mapNodes.First(node =>
                node.GridLocation.Equals(TargetLocation)
                && !node.isBlocked);

            Path = PathFinder.PathBetween(PathStart, PathEnd);

            if (Path == null) {
                return;
            }
    
            if (Path.Count <= 1) {
                return;
            }

            Vector3 nextPosition = LocationAsPosition(Path[1].GridLocation);

            if (Vector2.Distance(nextPosition, gameObject.transform.position) > 0.025) {
                IsMoving = true;

                Vector3 moveVector = (nextPosition - gameObject.transform.position).normalized;

                gameObject.transform.position += moveVector * (Time.deltaTime / 0.6f);
            } else {
                // Make necessary changes when arriving
                IsMoving = false;

                LevelManager.Instance.mapNodes.First(node =>
                    node.GridLocation.Equals(OwnLocation)).isBlocked = false;

                OwnLocation = Path[1].GridLocation;

                LevelManager.Instance.mapNodes.First(node =>
                    node.GridLocation.Equals(OwnLocation)).isBlocked = true;

                Path.RemoveAt(1);
            }
        }
        
        public Vector3 LocationAsPosition(Vector2Int location) {
            return new Vector3(
                location.x + 0.5f,
                location.y + 0.5f,
                -5f
            );
        }
    }
}
