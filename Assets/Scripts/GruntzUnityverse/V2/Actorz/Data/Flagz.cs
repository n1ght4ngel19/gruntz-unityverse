namespace GruntzUnityverse.V2.Actorz.Data {
[System.Serializable]
public struct Flagz {
	public bool selected;
	public bool moving;

	public bool interrupted;
	public bool moveForced;

	public bool idle;
	public bool hostileIdle;
	/// <summary>
	/// If true, the Grunt is set to interact or attack. This combines the 'setToInteract' and 'setToAttack' flags.
	/// </summary>
	public bool SetToAct => setToInteract || setToAttack;

	public bool setToInteract;
	public bool setToAttack;
	public bool setToGive;
}
}
