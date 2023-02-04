using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace _Test
{
  public class TCheckpointSwitch : MonoBehaviour
  {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public List<TCheckpointPyramid> Pyramids { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }


    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }

    private void Update()
    {
      if (Pyramids.All(pyramid => pyramid.HasChanged))
      {
        enabled = false;
      }

      if (LevelManager.Instance.testGruntz
        .Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))
      )
      {
        IsPressed = true;
      }
      else
      {
        IsPressed = false;
      }
    }
  }
}
