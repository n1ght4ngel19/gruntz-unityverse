using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz;
using UnityEngine;

namespace _Test {
    public class TestGrunt : MonoBehaviour {
        [field: SerializeField] public NavComponent NavComponent { get; set; }
        [field: SerializeField] public bool IsSelected { get; set; }


        private void Start() {
            NavComponent = gameObject.AddComponent<NavComponent>();
            NavComponent.OwnLocation = Vector2Int.FloorToInt(transform.position);
            NavComponent.TargetLocation = NavComponent.OwnLocation;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(1)
                && IsSelected
                && SelectorCircle.Instance.GridLocation != NavComponent.OwnLocation
            ) {
                NavComponent.TargetLocation = SelectorCircle.Instance.GridLocation;
            }

            // This gets called every time, since it only needs the target, which is always provided
            NavComponent.MoveTowardsTarget();
        }

        protected void OnMouseDown() {
            IsSelected = true;

            foreach (TestGrunt grunt in LevelManager.Instance.testGruntz.Where(grunt => grunt != this)) {
                grunt.IsSelected = false;
            }
        }
    }
}
