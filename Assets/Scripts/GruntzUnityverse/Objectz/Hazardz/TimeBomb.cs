using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
public class TimeBomb : Hazard, IAnimatable, IExplodable {
	public int delay;

	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }

	public override async void OnRevealed() {
		Debug.Log("OnRevealed");

		Animancer.Play(AnimationManager.Instance.timeBombExplodeAnim);

		await UniTask.WaitForSeconds(delay);

		GameManager.Instance.allGruntz
			.Where(gr => node.neighbours.Contains(gr.node))
			.ToList()
			.ForEach(gr1 => gr1.Die(AnimationManager.Instance.explodeDeathAnimation, false, false));

		FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.Where(r => node.neighbours.Contains(r.node))
			.ToList()
			.ForEach(r1 => r1.Interact());

		Destroy(gameObject);
	}
}
}
