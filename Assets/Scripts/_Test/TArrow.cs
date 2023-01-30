using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utilitiez;
using UnityEngine;

namespace _Test {
    public class TArrow : MonoBehaviour {
        [field: SerializeField] public Vector2Int OwnLocation { get; set; }
        [field: SerializeField] public CompassDirection Direction { get; set; }


        private void Start() {
            OwnLocation = Vector2Int.FloorToInt(transform.position);
        }

        private void Update() {
            foreach (TGrunt grunt in LevelManager.Instance.testGruntz) {
                // Todo: Additional condition(s) for interrupting Arrow movement (e.g. pushing via Nerf Gun)
                if (grunt.NavComponent.OwnLocation.Equals(OwnLocation)) {
                    grunt.NavComponent.TargetLocation = OwnLocation + VectorOfDirection(Direction);

                    return;
                }
            }
        }

        public Vector2Int VectorOfDirection(CompassDirection direction) {
            return direction switch {
                CompassDirection.North => Vector2Int.up,
                CompassDirection.East => Vector2Int.right,
                CompassDirection.South => Vector2Int.down,
                CompassDirection.West => Vector2Int.left
            };
        }
    }
}
