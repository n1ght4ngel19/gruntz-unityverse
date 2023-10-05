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
    
    public static Direction StringDirectionAsDirection(string dir) {
      return dir switch {
        StringDirection.North => Direction.North,
        StringDirection.East => Direction.East,
        StringDirection.South => Direction.South,
        StringDirection.West => Direction.West,
        StringDirection.Northeast => Direction.Northeast,
        StringDirection.Northwest => Direction.Northwest,
        StringDirection.Southeast => Direction.Southeast,
        StringDirection.Southwest => Direction.Southwest,
        var _ => throw new ArgumentOutOfRangeException(),
      };
    }
  }
}
