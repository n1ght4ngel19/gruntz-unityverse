using Animancer;
using GruntzUnityverse.Utils.Extensionz;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GruntzUnityverse.Core {
public class GameCursor : MonoBehaviour {
    public GameManager gameManager;

    public SpriteRenderer spriteRenderer;

    public Material brownSkinColor;

    public Material defaultMaterial;

    public AnimancerComponent animancer;

    public AnimationClip toPlay;

    private Camera displayCamera => Camera.main;

    private Camera interfaceCamera => GameObject.Find("InterfaceCamera").GetComponent<Camera>();

    private Vector3 mousePosition => displayCamera.ScreenToWorldPoint(Input.mousePosition);

    private Vector4 cameraBounds => new(
        x: displayCamera.transform.position.y + displayCamera.orthographicSize * displayCamera.aspect,
        y: displayCamera.transform.position.y - displayCamera.orthographicSize * displayCamera.aspect,
        z: displayCamera.transform.position.x - displayCamera.orthographicSize * displayCamera.aspect,
        w: displayCamera.transform.position.x + displayCamera.orthographicSize * displayCamera.aspect
    );

    private bool doSwap => gameManager.firstSelected.tool.CompatibleWith(gameManager.selector.hoveredObject);

    public static GameCursor instance;

    private void Awake() {
        instance = this;
        defaultMaterial = spriteRenderer.material;
    }

    private void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animancer = GetComponent<AnimancerComponent>();
    }

    private void Update() {
        transform.localScale = new(interfaceCamera.orthographicSize / 10f, interfaceCamera.orthographicSize / 10f, 1f);

        transform.position = new(
            Mathf.Clamp(mousePosition.x, cameraBounds.z, (cameraBounds.w * 1.15f) - spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit / 2),
            Mathf.Clamp(mousePosition.y, cameraBounds.y, cameraBounds.x),
            transform.position.z
        );

        animancer.Play(toPlay);

        if (SceneManager.GetActiveScene().name == Namez.MainMenuName) {
            return;
        }

        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null) {
            return;
        }

        if (gameManager.firstSelected == null) {
            return;
        }

        SwapCursor(doSwap ? gameManager.firstSelected.tool.cursor : AnimationManager.instance.cursorDefault);
    }

    public void SwapCursor(AnimationClip newCursor) {
        toPlay = newCursor;
    }
}
}
