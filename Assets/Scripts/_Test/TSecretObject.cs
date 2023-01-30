using GruntzUnityverse.Managerz;
using UnityEngine;

namespace _Test
{
  public class TSecretObject : MonoBehaviour
  {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public SpriteRenderer Renderer { get; set; }
    [field: SerializeField] public Behaviour Behaviour { get; set; }
    [field: SerializeField] public bool IsWalkable { get; set; }
    [field: SerializeField] public bool IsLocationOriginallyWalkable { get; set; }
    [field: SerializeField] public float Delay { get; set; }
    [field: SerializeField] public float Duration { get; set; }


    private void Start()
    {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Renderer = gameObject.GetComponent<SpriteRenderer>();
      Behaviour = gameObject.GetComponent<Behaviour>();
      SetSecretActive(false);
    }

    public void SetSecretActive(bool activeState)
    {
      Renderer.enabled = activeState;
      Behaviour.enabled = activeState;

      if (activeState && IsWalkable)
      {
        LevelManager.Instance.UnblockNodeAt(OwnLocation);
      }

      if (!activeState)
      {
        if (IsLocationOriginallyWalkable)
        {
          LevelManager.Instance.UnblockNodeAt(OwnLocation);
        }
        else
        {
          LevelManager.Instance.BlockNodeAt(OwnLocation);
        }
      }
    }
  }
}
