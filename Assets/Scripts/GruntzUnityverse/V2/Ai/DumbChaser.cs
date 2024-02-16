using System.Linq;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Ai {
public class DumbChaser : MonoBehaviour {
	private GruntV2 _self;
	public GruntV2 target;
	public Post post;

	private void Start() {
		_self = GetComponent<GruntV2>();
		post.managedGruntz.Add(this);
	}

	public void ActOnTarget() {
		target = post.targetedGruntz.First();
		_self.attackTarget = target;

		_self.Action();
	}

	public void ReturnToPost() {
		target = null;
		_self.attackTarget = null;

		_self.targetNode = post.node;
		_self.Move();
	}
}
}
