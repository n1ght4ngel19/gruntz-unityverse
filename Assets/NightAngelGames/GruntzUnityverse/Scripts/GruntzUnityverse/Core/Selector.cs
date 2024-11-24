using System.Linq;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utils.Extensionz;
using UnityEngine;


namespace GruntzUnityverse.Core {
public class Selector : MonoBehaviour {
	public GameManager gameManager;

	public Vector2Int location2D => Vector2Int.RoundToInt(transform.position);

	public Node node => Level.instance.levelNodes.FirstOrDefault(n => n.location2D == location2D);

	public GridObject hoveredObject => gameManager.gridObjectz.FirstOrDefault(go => go.enabled && go.location2D == location2D);

	public bool placingGrunt;

	public new Camera camera;

	private void Start() {
		gameManager = FindFirstObjectByType<GameManager>();
	}

	private void Update() {
		transform.position = Vector3Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).RoundedToInt(z: 15f));
	}
}
}
