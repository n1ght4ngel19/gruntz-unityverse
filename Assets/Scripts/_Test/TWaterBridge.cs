using GruntzUnityverse.Managerz;
using UnityEngine;

namespace _Test
{
  public class TWaterBridge : MonoBehaviour
  {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsDown { get; set; }
    [field: SerializeField] public bool ChangeState { get; set; }

    private void Start()
    {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
      if (ChangeState)
      {
        ChangeState = false;
        Animator.Play(IsDown ? "WaterBridge_Up" : "WaterBridge_Down");
        IsDown = !IsDown;
        LevelManager.Instance.SetBlockedAt(OwnLocation, IsDown);
      }
    }
  }
}
