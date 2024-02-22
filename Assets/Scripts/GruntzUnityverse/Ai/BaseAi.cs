using UnityEngine;

namespace GruntzUnityverse.Ai {
public abstract class BaseAi : MonoBehaviour {
	// protected Grunt Self;
	// public Grunt target;
	// public Post post;
	//
	// public abstract void ActOnTarget();
	//
	// public abstract void ReturnToPost();
	//
	// /// <summary>
	// /// Action taken towards predefined target.
	// /// </summary>
	// public void Action() {
	// 	if (Self.attackTarget == null || !Self.attackTarget.enabled) {
	// 		return;
	// 	}
	//
	// 	// Check team (friend or enemy)
	// 	// if (Self.attackTarget.team == Self.team) {
	// 	// 	Self.attackTarget = null;
	// 	//  Self.targetNode = Self.node;
	// 	//
	// 	// 	return;
	// 	// }
	//
	// 	if (Self.InRange(Self.attackTarget.node)) {
	// 		Debug.Log("I'm in range!");
	// 		Self.targetNode = Self.node;
	//
	// 		Self.FaceTowards(Self.attackTarget.node);
	//
	// 		Self.Attack();
	//
	// 		return;
	// 	}
	//
	// 	Self.onTargetReached.AddListener(Self.Attack);
	// 	Self.flagz.setToAttack = true;
	// 	Self.targetNode = Self.attackTarget.node;
	//
	// 	Self.Move();
	// }
}
}
