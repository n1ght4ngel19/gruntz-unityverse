using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
public class GruntPuddle : MonoBehaviour {
	public Vector2Int location2D;
	public Node node;
	public AnimancerComponent animancer;
	public AnimationClip appearAnim;
	public AnimationClip bubblingAnim;
	public AnimationClip disappearAnim;

	private void Start() {
		location2D = Vector2Int.RoundToInt(transform.position);
		node = Level.Instance.levelNodes.First(n => n.location2D == location2D);
		
		Appear();
	}

	public async void Appear() {
		await animancer.Play(appearAnim);

		animancer.Play(bubblingAnim);
	}

	public async void Disappear() {
		await animancer.Play(disappearAnim);
		Destroy(gameObject);
	}
}
}
