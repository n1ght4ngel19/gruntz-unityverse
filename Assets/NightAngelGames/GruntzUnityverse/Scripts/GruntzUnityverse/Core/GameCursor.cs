using Animancer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.Core {
public class GameCursor : MonoBehaviour {
	public GameManager gameManager;

	public SpriteRenderer spriteRenderer;

	public AnimationClip toPlay;

	public Material brownSkinColor;

	public Material defaultMaterial;

	public AnimancerComponent animancer;

	public static GameCursor instance;

	private bool doSwap => gameManager.firstSelected.tool.CompatibleWith(gameManager.selector.hoveredObject);

	private void Awake() {
		instance = this;

		defaultMaterial = spriteRenderer.material;
	}

	private void Start() {
		gameManager = FindFirstObjectByType<GameManager>();

		animancer = GetComponent<AnimancerComponent>();
	}

	private void Update() {
		transform.localScale = new Vector3(
			Camera.main.orthographicSize / 10f,
			Camera.main.orthographicSize / 10f,
			1f
		);

		transform.position = new Vector3(
			Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
			Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
			transform.position.z
		);

		animancer.Play(toPlay);

		if (SceneManager.GetActiveScene().name == "MainMenu") {
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
