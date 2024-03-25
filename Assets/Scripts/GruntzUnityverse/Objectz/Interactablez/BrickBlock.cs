using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
public class BrickBlock : GridObject {
	public Brick bottomBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.name.EndsWith("B"));
	public Brick middleBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.name.EndsWith("M"));
	public Brick topBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.name.EndsWith("T"));

	public Brick topMostBrick => topBrick != null ? topBrick : middleBrick != null ? middleBrick : bottomBrick != null ? bottomBrick : null;

	public override void Setup() {
		base.Setup();

		node.isBlocked = bottomBrick != null;
		node.hardCorner = bottomBrick != null;
	}

	public async void Break() {
		Brick toBreak = topMostBrick;

		if (toBreak.type != BrickType.Gold) {
			toBreak.animancer.Play(toBreak.breakAnim);

			toBreak.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
			toBreak.spriteRenderer.sortingLayerName = "Default";
			toBreak.spriteRenderer.sortingOrder = 4;
		}

		await UniTask.WaitForSeconds(toBreak.breakAnim.length * 0.75f);

		if (toBreak.type != BrickType.Gold) {
			toBreak.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			toBreak.spriteRenderer.sortingLayerName = "AlwaysBottom";
			toBreak.spriteRenderer.sortingOrder = 4;
		}

		node.isBlocked = toBreak != bottomBrick;
		node.hardCorner = toBreak != bottomBrick;

		await UniTask.WaitForSeconds(toBreak.breakAnim.length * 0.25f);

		if (toBreak.type != BrickType.Gold) {
			Destroy(toBreak.gameObject);
		}
	}
}
}
