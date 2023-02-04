using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace _Test
{
  public class TBlueToggleSwitch : MonoBehaviour
  {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public List<TWaterBridge> Bridges { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }


    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }

    private void Update()
    {
      if (LevelManager.Instance.testGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation)))
      {
        if (!HasBeenPressed)
        {
          IsPressed = true;
          HasBeenPressed = true;

          foreach (TWaterBridge bridge in Bridges)
          {
            bridge.ChangeState = true;
          }
        }
      }
      else
      {
        IsPressed = false;
        HasBeenPressed = false;
      }
    }
  }
}
