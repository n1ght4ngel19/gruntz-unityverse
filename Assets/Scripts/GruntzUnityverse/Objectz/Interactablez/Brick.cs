using Animancer;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
public enum BrickType {
	Brown = 1,
	Red = 2,
	Gold = 3,
	Black = 4,
	Blue = 5,
}
public class Brick : GridObject {
	public AnimancerComponent animancer;
	public AnimationClip breakAnim;
	public BrickType type;
}
}
