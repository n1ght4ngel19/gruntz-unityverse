using System.Linq;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utils.Extensionz;
using UnityEngine;

namespace GruntzUnityverse.Core {
public class Selector : MonoBehaviour {
	public Vector2Int location2D;

	public Node node => Level.instance.levelNodes.FirstOrDefault(n => n.location2D == location2D);

	public GridObject hoveredObject => GameManager.instance.gridObjectz.FirstOrDefault(go => go.enabled && go.location2D == location2D);

	public bool placingGrunt;

	private void FixedUpdate() {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition).RoundedToInt(z: 15f);
		location2D = Vector2Int.RoundToInt(transform.position);
	}
}
}
