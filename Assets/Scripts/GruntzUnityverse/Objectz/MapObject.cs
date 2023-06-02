using System.Collections;
using Animancer;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace GruntzUnityverse.Objectz {
  /// <summary>
  /// The base for all Objects that can be interacted with on a Level.
  /// </summary>
  public class MapObject : MonoBehaviour {
    public Vector2Int OwnLocation { get; set; }
    [CanBeNull] public Node OwnNode { get; set; }
    public SpriteRenderer Renderer { get; set; }
    protected Transform OwnTransform { get; set; }
    protected Camera MainCamera { get; set; }
    public Animator Animator { get; set; }
    public AnimancerComponent Animancer { get; set; }


    protected virtual void Awake() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Animator = gameObject.AddComponent<Animator>();
      OwnTransform = gameObject.GetComponent<Transform>();
      Renderer = gameObject.GetComponent<SpriteRenderer>();
      MainCamera = Camera.main;
      Animancer = gameObject.AddComponent<AnimancerComponent>();
    }

    protected virtual void Start() {
      OwnNode = LevelManager.Instance.NodeAt(OwnLocation);
    }

    // Todo: Remove
    protected void DeactivateSelf() {
      enabled = false;
      Renderer.enabled = false;
    }

    // Todo: Remove
    protected void ActivateSelf() {
      enabled = true;
      Renderer.enabled = true;
    }

    protected void SetEnabled(bool value) {
      enabled = value;
      Renderer.enabled = value;
    }

    public virtual IEnumerator BeUsed() {
      yield return null;
    }
  }
}
