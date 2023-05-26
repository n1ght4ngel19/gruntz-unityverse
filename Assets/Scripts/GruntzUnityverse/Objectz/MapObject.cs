using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using JetBrains.Annotations;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  /// <summary>
  /// The base for all Objects that can be interacted with on a Level.
  /// </summary>
  public class MapObject : MonoBehaviour {
    public Vector2Int OwnLocation { get; set; }
    [CanBeNull] public Node OwnNode { get; set; }
    public SpriteRenderer Renderer { get; set; }
    public Animator OwnAnimator { get; set; }
    protected Transform OwnTransform { get; set; }
    protected Camera MainCamera { get; set; }


    protected virtual void Awake() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      OwnAnimator = gameObject.GetComponent<Animator>();
      OwnTransform = gameObject.GetComponent<Transform>();
      Renderer = gameObject.GetComponent<SpriteRenderer>();
      MainCamera = Camera.main;
    }

    protected virtual void Start() {
      OwnNode = LevelManager.Instance.NodeAt(OwnLocation);
    }

    protected void DeactivateSelf() {
      enabled = false;
      Renderer.enabled = false;
    }

    protected void ActivateSelf() {
      enabled = true;
      Renderer.enabled = true;
    }
  }
}
