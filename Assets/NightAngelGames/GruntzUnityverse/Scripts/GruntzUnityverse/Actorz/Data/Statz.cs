using UnityEngine;

namespace GruntzUnityverse.Actorz.Data {
[System.Serializable]
public struct Statz {
	public const float MAX_VALUE = 20;

	[Range(0, MAX_VALUE)]
	public float health;

	[Range(0, MAX_VALUE)]
	public float stamina;

	[Range(0, 1f)]
	public float staminaRegenRate;

	public float powerupTime;
	public float toyTime;
	public float wingzTime; // Is this needed?
}
}
