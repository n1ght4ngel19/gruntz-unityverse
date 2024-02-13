using UnityEngine;

namespace GruntzUnityverse.V2.Grunt {
[System.Serializable]
public struct Statz {
	public const int MaxValue = 20;

	[Range(0, MaxValue)]
	public int health;

	[Range(0, MaxValue)]
	public int stamina;

	[Range(0, MaxValue)]
	public int staminaRegenRate;

	public int powerupTime;
	public int toyTime;
	public int wingzTime; // Is this needed?
}
}
