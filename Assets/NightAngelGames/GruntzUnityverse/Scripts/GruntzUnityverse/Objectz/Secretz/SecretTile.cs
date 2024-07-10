using Cysharp.Threading.Tasks;

namespace GruntzUnityverse.Objectz.Secretz {
public class SecretTile : GridObject {
	public float delay;

	public float duration;

	private bool _initiallyBlocked;

	private bool _initiallyWater;

	private bool _initiallyFire;

	private bool _initiallyVoid;

	private void AssignInitialNodeValues() {
		_initiallyBlocked = node.isBlocked;
		_initiallyWater = node.isWater;
		_initiallyFire = node.isFire;
		_initiallyVoid = node.isVoid;
	}

	public async void Reveal() {
		AssignInitialNodeValues();

		await UniTask.WaitForSeconds(delay);

		node.isBlocked = isObstacle;
		node.isWater = isWater;
		node.isFire = isFire;
		node.isVoid = isVoid;
		gameObject.SetActive(true);

		await UniTask.WaitForSeconds(duration);

		node.isBlocked = _initiallyBlocked;
		node.isWater = _initiallyWater;
		node.isFire = _initiallyFire;
		node.isVoid = _initiallyVoid;
		gameObject.SetActive(false);
	}
}
}
