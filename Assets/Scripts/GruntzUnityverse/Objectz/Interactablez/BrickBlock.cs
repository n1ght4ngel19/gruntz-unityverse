using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
public class BrickBlock : GridObject {
	public Brick bottomBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.name.EndsWith("B"));
	public Brick middleBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.name.EndsWith("M"));
	public Brick topBrick => transform.GetComponentsInChildren<Brick>().FirstOrDefault(br => br.name.EndsWith("T"));

	public override void Setup() {
		base.Setup();

		node.isBlocked = bottomBrick != null;
		node.hardCorner = bottomBrick != null;
	}

	public async void Break() {
		Brick toBreak =
			topBrick != null
				? topBrick
				: middleBrick != null
					? middleBrick
					: bottomBrick;


		toBreak.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		toBreak.spriteRenderer.sortingLayerName = "Default";
		toBreak.spriteRenderer.sortingOrder = 4;

		toBreak.animancer.Play(toBreak.breakAnim);

		await UniTask.WaitForSeconds(toBreak.breakAnim.length * 0.75f);

		toBreak.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		toBreak.spriteRenderer.sortingLayerName = "AlwaysBottom";
		toBreak.spriteRenderer.sortingOrder = 4;

		node.isBlocked = toBreak != bottomBrick;
		node.hardCorner = toBreak != bottomBrick;

		await UniTask.WaitForSeconds(toBreak.breakAnim.length * 0.25f);

		Destroy(toBreak.gameObject);
	}
}
}
