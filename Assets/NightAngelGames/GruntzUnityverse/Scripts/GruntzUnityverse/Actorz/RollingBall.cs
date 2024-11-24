using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Objectz.Arrowz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utils;
using HierarchyIcons;
using NaughtyAttributes;
using UnityEngine;


namespace GruntzUnityverse.Actorz {
public class RollingBall : MonoBehaviour {
    public bool hideComponents;

    [Foldout("Pathfinding")]
    [ReadOnly]
    public Node node;

    [Foldout("Pathfinding")]
    [ReadOnly]
    public Node next;

    [BoxGroup("Ball Data")]
    [Label("Seconds / Tile")]
    [Range(0, 5)]
    public float moveSpeed;

    [BoxGroup("Ball Data")]
    [ReadOnly]
    public Direction direction;

    [BoxGroup("Ball Data")]
    [ReadOnly]
    public Vector2Int location2D;

    [BoxGroup("Ball Data")]
    [ReadOnly]
    public SpriteRenderer spriteRenderer;

    [BoxGroup("Ball Data")]
    [ReadOnly]
    public CircleCollider2D circleCollider2D;

    [Foldout("Animation")]
    [DisableIf(nameof(isInstance))]
    public AnimationClip rollAnimUp;

    [Foldout("Animation")]
    [DisableIf(nameof(isInstance))]
    public AnimationClip rollAnimDown;

    [Foldout("Animation")]
    [DisableIf(nameof(isInstance))]
    public AnimationClip rollAnimLeft;

    [Foldout("Animation")]
    [DisableIf(nameof(isInstance))]
    public AnimationClip rollAnimRight;

    [Foldout("Animation")]
    [DisableIf(nameof(isInstance))]
    public AnimationClip breakAnim;

    [Foldout("Animation")]
    [DisableIf(nameof(isInstance))]
    public AnimationClip sinkAnim;

    private AnimationClip currentRollAnim => direction switch {
        Direction.Up or Direction.UpRight or Direction.UpLeft => rollAnimUp,
        Direction.Down or Direction.DownRight or Direction.DownLeft => rollAnimDown,
        Direction.Left => rollAnimLeft,
        Direction.Right => rollAnimRight,
        _ => currentRollAnim,
    };

    [Foldout("Animation")]
    [DisableIf(nameof(isInstance))]
    public AnimancerComponent animancer;

    public bool isInstance => gameObject.scene.name != null;

    private void ChangePosition() {
        if (next == null || node == null) {
            return;
        }

        Vector3 moveVector = (next.transform.position - node.transform.position);
        gameObject.transform.position += moveVector * (Time.fixedDeltaTime / moveSpeed);
    }

    private async void Break() {
        moveSpeed *= 5;

        animancer.Play(breakAnim);
        await UniTask.WaitForSeconds(breakAnim.length);

        animancer.enabled = false;
        enabled = false;

        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        GetComponent<SpriteRenderer>().sortingLayerName = "AlwaysBottom";
        GetComponent<SpriteRenderer>().sortingOrder = 6;
    }

    private void DirectByArrow() {
        Arrow belowArrow = FindObjectsByType<Arrow>(FindObjectsSortMode.None).FirstOrDefault(ar => ar.location2D == location2D);

        if (belowArrow != null) {
            direction = belowArrow.direction;
        }

        transform.rotation = Quaternion.identity;

        float rotateAngle = direction switch {
            Direction.UpRight => -45,
            Direction.UpLeft => 45,
            Direction.DownRight => -135,
            Direction.DownLeft => 135,
            _ => 0,
        };

        transform.Rotate(0, 0, rotateAngle);
    }

    // --------------------------------------------------
    // Lifecycle
    // --------------------------------------------------

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        animancer = GetComponent<AnimancerComponent>();
        location2D = Vector2Int.RoundToInt(transform.position);
    }

    private void Start() {
        node = FindObjectsByType<Node>(FindObjectsSortMode.None).First(n => n.location2D == location2D);

        DirectByArrow();

        animancer.Play(currentRollAnim);
    }

    private void FixedUpdate() {
        ChangePosition();
    }

    private void Update() {
        animancer.Play(currentRollAnim);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        bool hitBall = other.TryGetComponent(out RollingBall _);
        bool hitBrick = other.TryGetComponent(out Brick _);
        bool hitRock = other.TryGetComponent(out Rock _);
        bool hitGrunt = other.TryGetComponent(out Grunt _);

        if (!hitBall && !hitBrick && !hitRock) {
            return;
        }

        circleCollider2D.isTrigger = false;
        Break();
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        HideFlags newHideFlags = hideComponents ? HideFlags.HideInInspector : HideFlags.None;

        GetComponent<SpriteRenderer>().hideFlags = newHideFlags;
        GetComponent<CircleCollider2D>().hideFlags = newHideFlags;
        GetComponent<Rigidbody2D>().hideFlags = newHideFlags;

        animancer.hideFlags = newHideFlags;
        animancer.Animator.hideFlags = newHideFlags;

        if (TryGetComponent(out TrimName trimName)) {
            trimName.hideFlags = newHideFlags;
        }

        if (TryGetComponent(out HierarchyIcon hierarchyIcon)) {
            hierarchyIcon.hideFlags = isInstance ? HideFlags.None : HideFlags.HideInInspector;
        }
    }

    private void OnDrawGizmosSelected() {
        location2D = Vector2Int.RoundToInt(transform.position);

        transform.hideFlags = HideFlags.HideInInspector;

        HideFlags newHideFlags = hideComponents ? HideFlags.HideInInspector : HideFlags.None;

        GetComponent<SpriteRenderer>().hideFlags = newHideFlags;
        GetComponent<CircleCollider2D>().hideFlags = newHideFlags;
        GetComponent<Rigidbody2D>().hideFlags = newHideFlags;

        animancer.hideFlags = newHideFlags;
        animancer.Animator.hideFlags = newHideFlags;

        if (TryGetComponent(out TrimName trimName)) {
            trimName.hideFlags = newHideFlags;
        }

        if (TryGetComponent(out HierarchyIcon hierarchyIcon)) {
            hierarchyIcon.hideFlags = isInstance ? HideFlags.None : HideFlags.HideInInspector;
        }
    }
    #endif
}
}
