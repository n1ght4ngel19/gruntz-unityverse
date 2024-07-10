using System.Linq;
using Cysharp.Threading.Tasks;
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

	private void Start() {
		gameManager = FindFirstObjectByType<GameManager>();
	}

	private void Update() {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).RoundedToInt(z: 15f);
	}
}
}
