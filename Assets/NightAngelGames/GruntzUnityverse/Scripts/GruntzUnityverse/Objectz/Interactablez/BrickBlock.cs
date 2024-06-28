using System.Linq;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Objectz.Interactablez {
public class BrickBlock : GridObject {
	public bool cloaked;
	public Brick bottomBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.CompareTag("BottomBrick"));
	public Brick middleBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.CompareTag("MiddleBrick"));
	public Brick topBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.CompareTag("TopBrick"));

	public Brick topMostBrick => topBrick != null ? topBrick : middleBrick != null ? middleBrick : bottomBrick != null ? bottomBrick : null;

	public override void Setup() {
		base.Setup();

		string cloakKey;

		if (topBrick != null) {
			cloakKey = "Brick_111";
		} else if (middleBrick != null) {
			cloakKey = "Brick_110";
		} else if (bottomBrick != null) {
			cloakKey = "Brick_100";
		} else {
			cloakKey = "BrickFoundation";
		}

		Addressables.LoadAssetAsync<Sprite>(cloakKey).Completed += handle => {
			GetComponent<SpriteRenderer>().enabled = cloaked;
			GetComponent<SpriteRenderer>().sprite = handle.Result;

			if (cloaked) {
				GetComponent<SpriteRenderer>().sortingLayerName = "HighObjectz";
				GetComponent<SpriteRenderer>().sortingOrder = 3;
			}
		};

		node.isBlocked = bottomBrick != null;
		node.hardCorner = bottomBrick != null;
	}

	public async void Break(bool explode = false, bool immediate = false) {
		Brick toBreak = topMostBrick;

		if (toBreak == bottomBrick) {
			Reveal();
		}

		if (toBreak.type != BrickType.Gold || explode) {
			toBreak.animancer.Play(toBreak.breakAnim);

			toBreak.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
			toBreak.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
			toBreak.GetComponent<SpriteRenderer>().sortingOrder = 4;
		}

		await UniTask.WaitForSeconds(toBreak.breakAnim.length * 0.05f);

		if (toBreak.type == BrickType.Black) {
			GameManager.instance.allGruntz
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

		if (toBreak.type == BrickType.Black) {
			Break(explode: true, immediate: true);
		}

		if (toBreak.type != BrickType.Gold || explode) {
			toBreak.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			toBreak.GetComponent<SpriteRenderer>().sortingLayerName = "AlwaysBottom";
			toBreak.GetComponent<SpriteRenderer>().sortingOrder = 4;

			node.isBlocked = !toBreak.CompareTag("BottomBrick");
			node.hardCorner = !toBreak.CompareTag("BottomBrick");

			GameManager.instance.gridObjectz.Remove(toBreak);
			Destroy(toBreak.gameObject, toBreak.breakAnim.length * 0.25f);
		}
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
			brick.GetComponent<Brick>().Setup();
			GameManager.instance.gridObjectz.Add(brick.GetComponent<Brick>());
		};
	}
}
}
