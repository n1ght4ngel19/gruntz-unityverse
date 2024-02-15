namespace GruntzUnityverse.V2.Grunt {
[System.Serializable]
public struct Flagz {
	public bool selected;
	public bool moving;

	public bool interrupted;
	public bool moveForced;

	public bool idle;
	public bool hostileIdle;

	public bool setToInteract;
	public bool interacting;

	public bool setToAttack;
	public bool attacking;

	public bool setToGive;
	public bool giving;
}
}
