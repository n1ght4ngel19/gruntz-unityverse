namespace GruntzUnityverse.V2.Utils {
  public static class EnumX {
    public static DirectionV2 Opposite(this DirectionV2 dir) {
      return dir switch {
        DirectionV2.Up => DirectionV2.Down,
        DirectionV2.UpRight => DirectionV2.DownLeft,
        DirectionV2.Right => DirectionV2.Left,
        DirectionV2.DownRight => DirectionV2.UpLeft,
        DirectionV2.Down => DirectionV2.Up,
        DirectionV2.DownLeft => DirectionV2.UpRight,
        DirectionV2.Left => DirectionV2.Right,
        DirectionV2.UpLeft => DirectionV2.DownRight,
        _ => DirectionV2.None,
      };
    }
  }
}
