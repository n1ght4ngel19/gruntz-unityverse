using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace GruntzUnityverse.Objectz {
public class CreatorPad : GridObject {
    [DisableIf(nameof(isInstance))]
    public AnimationClip padAnim;

    [ReadOnly]
    public AnimancerComponent animancer;

    private void OnTryPlaceGrunt() {
        if (gameManager == null) {
            return;
        }

        if (!gameManager.selector.placingGrunt || gameManager.selector.node != node) {
            return;
        }

        gameManager.selector.placingGrunt = false;
        GameCursor.instance.SwapCursor(AnimationManager.instance.cursorDefault);
        GameCursor.instance.spriteRenderer.material = GameCursor.instance.defaultMaterial;

        Addressables.InstantiateAsync("PG_BareHandz").Completed += handle => {
            handle.Result.transform.position = new Vector3(location2D.x, location2D.y, 0);

            gameManager.gruntz.Add(handle.Result.GetComponent<Grunt>());
            gameManager.playerGruntz.Add(handle.Result.GetComponent<Grunt>());
        };
    }

    // --------------------------------------------------
    // Lifecycle
    // --------------------------------------------------

    protected override void Awake() {
        base.Awake();

        animancer = GetComponent<AnimancerComponent>();
    }

    protected override void Start() {
        animancer.Play(padAnim);
    }
}
}
