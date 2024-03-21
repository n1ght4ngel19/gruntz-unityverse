using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
public class GruntPuddle : GridObject {
	public int gooAmount;

	public AnimancerComponent animancer;
	public AnimationClip appearAnim;
	public AnimationClip bubblingAnim;
	public AnimationClip disappearAnim;

	private void Start() {
		location2D = Vector2Int.RoundToInt(transform.position);
		node = Level.instance.levelNodes.First(n => n.location2D == location2D);

		animancer.Play(bubblingAnim);
	}

	public async void Appear() {
		await animancer.Play(appearAnim);

		animancer.Play(bubblingAnim);
	}

	public async void Disappear() {
		await animancer.Play(disappearAnim);

		Destroy(gameObject, 0.1f);
	}
}
}
