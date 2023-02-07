using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class CheckpointSwitch : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public List<CheckpointPyramid> Pyramidz { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }


    private void Start() {
      if (Pyramidz.Count.Equals(0)) {
        Debug.LogError("There is no Pyramid assigned to this Switch, this way the Checkpoint won't work properly!");
      }

      OwnLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      if (Pyramidz.All(pyramid => pyramid.HasChanged)) {
        enabled = false;
      }

      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        IsPressed = true;
      } else {
        IsPressed = false;
      }
    }
  }
}
