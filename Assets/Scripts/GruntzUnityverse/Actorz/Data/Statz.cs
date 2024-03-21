using UnityEngine;

namespace GruntzUnityverse.Actorz.Data {
[System.Serializable]
public struct Statz {
	public const int MAX_VALUE = 20;

	[Range(0, MAX_VALUE)]
	public int health;

	[Range(0, MAX_VALUE)]
	public int stamina;

	[Range(0, 1f)]
	public float staminaRegenRate;

	public int powerupTime;
	public int toyTime;
	public int wingzTime; // Is this needed?
}
}
