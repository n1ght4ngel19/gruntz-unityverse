using System;
using System.Linq;
using GruntzUnityverse.Enumz;
using NUnit.Framework;

namespace Tests {
  public class EnumTest {
    [Test]
    public void UniqueToolTypesTest() {
      ToolName[] toolTypeValues = (ToolName[])Enum.GetValues(typeof(ToolName));

      Assert.AreEqual(toolTypeValues.Length, toolTypeValues.Distinct().Count());
    }
    
    [Test]
    public void UniqueToyTypesTest() {
      ToyName[] toyTypeValues = (ToyName[])Enum.GetValues(typeof(ToyName));

      Assert.AreEqual(toyTypeValues.Length, toyTypeValues.Distinct().Count());
    }
    
    [Test]
    public void UniquePowerupTypesTest() {
      PowerupName[] powerupTypeValues = (PowerupName[])Enum.GetValues(typeof(PowerupName));

      Assert.AreEqual(powerupTypeValues.Length, powerupTypeValues.Distinct().Count());
    }
    
    [Test]
    public void UniqueDeathTypesTest() {
      DeathName[] deathTypeValues = (DeathName[])Enum.GetValues(typeof(DeathName));

      Assert.AreEqual(deathTypeValues.Length, deathTypeValues.Distinct().Count());
    }

    [Test]
    public void UniqueCompassDirectionsTest() {
      Direction[] compassDirectionValues = (Direction[])Enum.GetValues(typeof(Direction));

      Assert.AreEqual(compassDirectionValues.Length, compassDirectionValues.Distinct().Count());
    }
    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator EnumTestWithEnumeratorPasses()
    // {
    //     // Use the Assert class to test conditions.
    //     // Use yield to skip a frame.
    //     yield return null;
    // }
  }
}
