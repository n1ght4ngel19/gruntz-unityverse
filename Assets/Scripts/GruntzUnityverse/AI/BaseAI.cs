using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Actorz.BehaviourManagement;
using GruntzUnityverse.Editor.PropertyDrawers;
using UnityEngine;

namespace GruntzUnityverse.AI {
public class BaseAI : MonoBehaviour {
	[HideInNormalInspector]
	public Grunt self;

	public Post post;
	public Grunt target;

	private void Start() {
		self = GetComponent<Grunt>();
		post.managedAI.Add(this);
	}

	/// <summary>
	/// Default implementation of deciding whether to take action, and what kind.
	/// <para/>
	/// <b>To be overridden by subclasses.</b>
	/// </summary>
	public void DecideAttack() {
		if (target != null) {
			return;
		}

		if (post.spottedEnemiez.Count == 0) {
			ReturnToPost();

			return;
		}

		target = post.spottedEnemiez.First(); // Todo: Make this smarter (e.g. closest, weakest, etc.)
		self.attackTarget = target;
		self.HandleActionCommand(target.node, Intent.ToAttack);
	}

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
