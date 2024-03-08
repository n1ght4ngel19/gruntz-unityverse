using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
public class EquippedPowerup : ScriptableObject {
	public string powerupName;
	public string description;

	[Range(0, 100)]
	public int duration;

	[Range(0, 100)]
	public int delay;

	public async void Equip(Grunt affectedGrunt) {
		await UniTask.WaitForSeconds(delay);

		ActivateEffect(affectedGrunt);

		await UniTask.WaitForSeconds(duration);

		DeactivateEffect(affectedGrunt);
	}

	protected virtual void ActivateEffect(Grunt affectedGrunt) { }

	protected virtual void DeactivateEffect(Grunt affectedGrunt) { }
}
}
