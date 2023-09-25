using System;
using GruntzUnityverse.Enumz;

namespace GruntzUnityverse.Utility {
  public static class DirectionUtility {
    public static Direction OppositeOf(Direction dir) {
      return dir switch {
        Direction.North => Direction.South,
        Direction.East => Direction.West,
        Direction.South => Direction.North,
        Direction.West => Direction.East,
        Direction.Northeast => Direction.Southwest,
        Direction.Northwest => Direction.Southeast,
        Direction.Southeast => Direction.Northwest,
        Direction.Southwest => Direction.Northeast,
        var _ => throw new ArgumentOutOfRangeException(),
      };
    }
  }
}
