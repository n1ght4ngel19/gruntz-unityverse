using GruntzUnityverse.Actorz;
using GruntzUnityverse.Actorz.BehaviourManagement;
using GruntzUnityverse.Editor.PropertyDrawers;
using UnityEngine;

namespace GruntzUnityverse.AI {
public abstract class BaseAI : MonoBehaviour {
	[HideInNormalInspector]
	public Grunt self;

	public Post post;

	public Grunt target;

	private void Start() {
		self = GetComponent<Grunt>();
		post.managedAI.Add(this);
	}

	/// <summary>
	/// Deciding whether to take action, and what kind.
	/// <para/>
	/// <b>To be implemented by subclasses.</b>
	/// </summary>
	public abstract void DecideAttack();

	public void ReturnToPost() {
		target = null;
		self.attackTarget = null;

		self.travelGoal = post.node;
		self.intent = Intent.ToMove;
		self.EvaluateState(whenFalse: self.BetweenNodes || self.committed);
	}

	private void OnDestroy() {
		post.managedAI.Remove(this);
	}
}
}
