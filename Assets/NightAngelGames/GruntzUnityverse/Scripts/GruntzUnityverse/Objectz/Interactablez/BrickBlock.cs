using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace GruntzUnityverse.Objectz.Interactablez {
public class BrickBlock : GridObject {
	public bool cloaked;

	public Brick bottomBrick =>
		transform.GetComponentsInChildren<Brick>()
			.FirstOrDefault(br => br.CompareTag("BottomBrick"));

	public Brick middleBrick =>
		transform.GetComponentsInChildren<Brick>()
			.FirstOrDefault(br => br.CompareTag("MiddleBrick"));

	public Brick topBrick =>
		transform.GetComponentsInChildren<Brick>()
			.FirstOrDefault(br => br.CompareTag("TopBrick"));

	public Brick topMostBrick => topBrick != null ? topBrick : middleBrick != null ? middleBrick : bottomBrick != null ? bottomBrick : null;

	public async void Break(bool explode = false, bool immediate = false) {
		Brick toBreak = topMostBrick;

		if (toBreak == bottomBrick) {
			Reveal();
		}

		if (toBreak.type != BrickType.Gold || explode) {
			toBreak.animancer.Play(toBreak.breakAnim);

			toBreak.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
			toBreak.spriteRenderer.sortingLayerName = "Default";
			toBreak.spriteRenderer.sortingOrder = 4;
		}

		await UniTask.WaitForSeconds(toBreak.breakAnim.length * 0.05f);

		if (toBreak.type == BrickType.Black) {
			gameManager.gruntz
				.Where(gr => node.neighbours.Contains(gr.node))
				.ToList()
				.ForEach(gr1 => gr1.Die(AnimationManager.instance.explodeDeathAnimation, false, false));

			FindObjectsByType<Rock>(FindObjectsSortMode.None)
				.Where(r => node.neighbours.Contains(r.node))
				.ToList()
				.ForEach(r1 => r1.Break());

			FindObjectsByType<BrickBlock>(FindObjectsSortMode.None)
				.Where(bb => node.neighbours.Contains(bb.node))
				.ToList()
				.ForEach(bb1 => bb1.Break(explode: true));
		}

		await UniTask.WaitForSeconds(immediate ? 0 : toBreak.breakAnim.length * 0.7f);

		switch (toBreak.type) {
			case BrickType.Black:
				Break(explode: true, immediate: true);

				break;
			case BrickType.Gold when !explode:
				return;
			case BrickType.Brown:
				break;
			case BrickType.Red:
				break;
			case BrickType.Blue:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		toBreak.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		toBreak.spriteRenderer.sortingLayerName = "AlwaysBottom";
		toBreak.spriteRenderer.sortingOrder = 4;

		node.isBlocked = !toBreak.CompareTag("BottomBrick");
		node.hardCorner = !toBreak.CompareTag("BottomBrick");

		gameManager.gridObjectz.Remove(toBreak);

		Destroy(toBreak.gameObject, toBreak.breakAnim.length * 0.25f);
	}

	public void Reveal() {
		GetComponent<SpriteRenderer>().enabled = false;
	}

	public void BuildBrick(BrickType typeToBuild) {
		string layer = bottomBrick == null ? "B" : middleBrick == null ? "M" : "T";
		string buildKey = $"Brick_{(int)typeToBuild}{layer}";

		Addressables.LoadAssetAsync<GameObject>(buildKey).Completed += handle => {
			Reveal();

			GameObject brick = Instantiate(handle.Result, transform);

			brick.name.Remove(brick.name.Length - 7);
			// brick.GetComponent<Brick>().Setup();

			gameManager.gridObjectz.Add(brick.GetComponent<Brick>());
		};
	}

	protected override void AssignNodeValues() {
		node.isBlocked = bottomBrick != null;
		node.hardCorner = bottomBrick != null;
	}

	protected override void Start() {
		base.Start();

		string cloakKey = "BrickFoundation";

		if (topBrick != null) {
			cloakKey = "Brick_111";
		} else if (middleBrick != null) {
			cloakKey = "Brick_110";
		} else if (bottomBrick != null) {
			cloakKey = "Brick_100";
		}

		Addressables.LoadAssetAsync<Sprite>(cloakKey).Completed += handle => {
			spriteRenderer.enabled = cloaked;
			spriteRenderer.sprite = handle.Result;

			if (!cloaked) {
				return;
			}

			spriteRenderer.sortingLayerName = "HighObjectz";
			spriteRenderer.sortingOrder = 3;
		};
	}
}
}
