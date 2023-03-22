namespace GruntzUnityverse.Utility {
  public struct Death {
    public readonly string animationName;
    public readonly float animationDuration;

    public Death(string name, float duration) {
      animationName = name;
      animationDuration = duration;
    }
    
    public static Death FallInHole = new Death("Death_Hole", 0.5f);
    public static Death Sink = new Death("Death_Sink", 0.5f);
    public static Death GetSquashed = new Death("Death_Squash", 0.5f);
  }
}
