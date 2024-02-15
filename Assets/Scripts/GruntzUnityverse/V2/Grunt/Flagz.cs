namespace GruntzUnityverse.V2.Grunt {
[System.Serializable]
public struct Flagz {
	public bool selected;
	public bool moving;

	public bool interrupted;
	public bool moveForced;

	public bool idle;
	public bool hostileIdle;
	public bool SetToAct => setToInteract || setToAttack;

	public bool setToInteract;
	public bool setToAttack;
	public bool setToGive;
}
}
