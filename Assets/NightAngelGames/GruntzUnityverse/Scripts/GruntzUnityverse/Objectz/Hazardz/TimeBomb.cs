﻿using System.Linq;
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

		Animancer.Play(AnimationManager.instance.timeBombTickingAnim);

		await UniTask.WaitForSeconds(delay);

		GameManager.instance.allGruntz
			.Where(gr => node.neighbours.Contains(gr.node) || node == gr.node)
			.ToList()
			.ForEach(gr1 => gr1.Die(AnimationManager.instance.explodeDeathAnimation, false, false));

		FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.Where(r => node.neighbours.Contains(r.node))
			.ToList()
			.ForEach(r1 => r1.Break());

		spriteRenderer.sortingLayerName = "Default";
		spriteRenderer.sortingOrder = 6;
		
		await Animancer.Play(AnimationManager.instance.explosionAnim1);

		GameManager.instance.gridObjectz.Remove(this);
		Destroy(gameObject);
	}
}
}