using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class CheckpointSwitch : ObjectSwitch {
    [field: SerializeField] public List<CheckpointPyramid> Pyramidz { get; set; }
    [field: SerializeField] public ItemType Requirement { get; set; }


    private void Update() {
      if (Pyramidz.Count.Equals(0)) {
        Debug.LogError("There is no Pyramid assigned to this Switch, this way the Checkpoint won't work properly!");

        enabled = false;
      }

      if (Pyramidz.All(pyramid => pyramid.HasChanged)) {
        enabled = false;
      }

      if (LevelManager.Instance.AllGruntz.Any(
        grunt => grunt.IsOnLocation(OwnLocation) && grunt.HasItem(Requirement.ToString())
      )) {
        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }
  }
}
