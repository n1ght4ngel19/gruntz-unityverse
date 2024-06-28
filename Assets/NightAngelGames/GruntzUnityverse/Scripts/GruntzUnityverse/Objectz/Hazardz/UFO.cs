using Cysharp.Threading.Tasks;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
public class UFO : GridObject {
	public Spotlight spotlight1;
	public Spotlight spotlight2;
	public Node[] route;

	private void Start() {
		Debug.Log("Starting UFO");
		route = new Node[12];
		
		route[0] = node.neighbourSet.up.neighbourSet.up;
		route[1] = node.neighbourSet.up.neighbourSet.upRight;
		route[2] = node.neighbourSet.right.neighbourSet.upRight;
		route[3] = node.neighbourSet.right.neighbourSet.right;
		route[4] = node.neighbourSet.right.neighbourSet.downRight;
		route[5] = node.neighbourSet.down.neighbourSet.downRight;
		route[6] = node.neighbourSet.down.neighbourSet.down;
		route[7] = node.neighbourSet.down.neighbourSet.downLeft;
		route[8] = node.neighbourSet.left.neighbourSet.downLeft;
		route[9] = node.neighbourSet.left.neighbourSet.left;
		route[10] = node.neighbourSet.left.neighbourSet.upLeft;
		route[11] = node.neighbourSet.up.neighbourSet.upLeft;
		
		MoveLights();
	}

	private async void MoveLights() {
		await UniTask.WaitForSeconds(0.6f);
		
		spotlight1.transform.position = route[0].transform.position;
		spotlight2.transform.position = route[6].transform.position;
		
		await UniTask.WaitForSeconds(0.6f);

		spotlight1.transform.position = route[1].transform.position;
		spotlight2.transform.position = route[7].transform.position;

		await UniTask.WaitForSeconds(0.6f);

		spotlight1.transform.position = route[2].transform.position;
		spotlight2.transform.position = route[8].transform.position;

		await UniTask.WaitForSeconds(0.6f);

		spotlight1.transform.position = route[3].transform.position;
		spotlight2.transform.position = route[9].transform.position;
		
		MoveLights();
	}
}
}
