using System.Collections.Generic;
using System.Linq;
using Animancer;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utils;
using HierarchyIcons;
using NaughtyAttributes;
using UnityEngine;


namespace GruntzUnityverse.Objectz {
public abstract class GridObject : MonoBehaviour {
    #region Fieldz

    public int objectID;

    public bool hideComponents;

    [Foldout("Flagz")]
    [DisableIf((nameof(isInstance)))]
    public bool isObstacle;

    [Foldout("Flagz")]
    [DisableIf((nameof(isInstance)))]
    public bool isWater;

    [Foldout("Flagz")]
    [DisableIf((nameof(isInstance)))]
    public bool isFire;

    [Foldout("Flagz")]
    [DisableIf((nameof(isInstance)))]
    public bool isVoid;

    [BoxGroup("GridObject Data")]
    [ReadOnly]
    public Vector2Int location2D;

    [BoxGroup("GridObject Data")]
    [ReadOnly]
    public Node node;

    [BoxGroup("GridObject Data")]
    [Required]
    [HideIf((nameof(isInstance)))]
    public SpriteRenderer spriteRenderer;

    [Required]
    [BoxGroup("GridObject Data")]
    [HideIf((nameof(isInstance)))]
    public CircleCollider2D circleCollider2D;

    protected GameManager gameManager;

    public bool isInstance => gameObject.scene.name != null;

    #endregion


    /// <summary>
    /// Checks whether this GridObject is diagonal to the given GridObject.
    /// </summary>
    protected bool IsDiagonalTo(GridObject other) {
        return Mathf.Abs(other.location2D.x - location2D.x) != 0 && Mathf.Abs(other.location2D.y - location2D.y) != 0;
    }

    public virtual void Activate() {
        circleCollider2D.isTrigger = true;
    }

    public virtual void Deactivate() {
        circleCollider2D.isTrigger = false;
    }

    protected virtual void AssignNodeValues() {
        node.isBlocked = isObstacle;
        node.isWater = isWater;
        node.isFire = isFire;
        node.isVoid = isVoid;
    }

    protected virtual void UnAssignNodeValues() {
        node.isBlocked = isObstacle;
        node.isWater = isWater;
        node.isFire = isFire;
        node.isVoid = isVoid;
    }

    protected List<T> GetSiblings<T>(bool includeInactive = false) where T : MonoBehaviour {
        return transform.parent.GetComponentsInChildren<T>(includeInactive).ToList();
    }

    // --------------------------------------------------
    // Lifecycle
    // --------------------------------------------------

    /// <summary>
    /// Initialization. Referencing other objects is NOT allowed.
    /// </summary>
    protected virtual void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        location2D = Vector2Int.RoundToInt(transform.position);
    }

    /// <summary>
    /// Initialization. Referencing other objects is NOT allowed.
    /// </summary>
    protected virtual void OnEnable() { }

    /// <summary>
    /// Initialization. Runs only in the editor
    /// </summary>
    protected virtual void Reset() { }

    /// <summary>
    /// Initialization. Referencing other objects is allowed.
    /// </summary>
    protected virtual void Start() {
        gameManager = FindFirstObjectByType<GameManager>();

        node = FindObjectsByType<Node>(FindObjectsSortMode.None).First(n => n.location2D.Equals(location2D));

        AssignNodeValues();
    }

    /// <summary>
    /// Physics-related or time-dependent updates.
    /// </summary>
    protected virtual void FixedUpdate() { }

    /// <summary>
    /// Main loop. Update the game state here.
    /// </summary>
    protected virtual void Update() { }

    /// <summary>
    /// Called when another object enters this object's collider.
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D other) { }

    /// <summary>
    /// Called when another object exits this object's collider.
    /// </summary>
    protected virtual void OnTriggerExit2D(Collider2D other) { }

    /// <summary>
    /// Called after all Update functions have been called.
    /// </summary>
    protected virtual void LateUpdate() { }

    /// <summary>
    /// Decommissioning. Called when the object is disabled during a frame.
    /// </summary>
    protected virtual void OnDisable() { }

    /// <summary>
    /// Decommissioning. Called when the object is destroyed.
    /// </summary>
    protected virtual void OnDestroy() {
        if (node == null) {
            return;
        }

        UnAssignNodeValues();
    }

    protected virtual void OnValidate() {
        #if UNITY_EDITOR
        HideFlags newHideFlags = hideComponents ? HideFlags.HideInInspector : HideFlags.None;

        GetComponent<SpriteRenderer>().hideFlags = newHideFlags;
        GetComponent<CircleCollider2D>().hideFlags = newHideFlags;

        if (TryGetComponent(out AnimancerComponent animancer)) {
            animancer.hideFlags = newHideFlags;
            animancer.Animator.hideFlags = newHideFlags;
        }

        if (TryGetComponent(out TrimName trimName)) {
            trimName.hideFlags = newHideFlags;
        }

        if (TryGetComponent(out HierarchyIcon hierarchyIcon)) {
            hierarchyIcon.hideFlags = isInstance ? HideFlags.None : HideFlags.HideInInspector;
        }
        #endif
    }

    private void OnDrawGizmosSelected() {
        #if UNITY_EDITOR
        location2D = Vector2Int.RoundToInt(transform.position);

        transform.hideFlags = HideFlags.HideInInspector;

        HideFlags newHideFlags = hideComponents ? HideFlags.HideInInspector : HideFlags.None;

        spriteRenderer.hideFlags = newHideFlags;
        circleCollider2D.hideFlags = newHideFlags;

        if (TryGetComponent(out AnimancerComponent animancer)) {
            animancer.hideFlags = newHideFlags;
            animancer.Animator.hideFlags = newHideFlags;
        }

        if (TryGetComponent(out TrimName trimName)) {
            trimName.hideFlags = newHideFlags;
        }

        if (TryGetComponent(out HierarchyIcon hierarchyIcon)) {
            hierarchyIcon.hideFlags = isInstance ? HideFlags.None : HideFlags.HideInInspector;
        }
        #endif
    }
}
}
