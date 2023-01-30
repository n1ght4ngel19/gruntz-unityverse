using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace _Test
{
  public class TCheckpointPyramid : MonoBehaviour
  {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public List<TCheckpointSwitch> Switches { get; set; }
    [field: SerializeField] public bool IsDown { get; set; }
    [field: SerializeField] public bool HasChanged { get; set; }

    private void Start()
    {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
      if (Switches.Any(checkpointSwitch => !checkpointSwitch.IsPressed || !checkpointSwitch.CanBePressed))
        return;

      HasChanged = true;
      Animator.Play(IsDown ? "Pyramid_Up" : "Pyramid_Down");
      IsDown = !IsDown;
      LevelManager.Instance.SetBlockedAt(OwnLocation, !IsDown);
    }
  }
}
