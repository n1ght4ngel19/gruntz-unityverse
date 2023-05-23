using System;
using System.Linq;
using GruntzUnityverse.Enumz;
using NUnit.Framework;

namespace Tests {
  public class EnumTest {
    [Test]
    public void UniqueToolTypesTest() {
      ToolType[] toolTypeValues = (ToolType[])Enum.GetValues(typeof(ToolType));

      Assert.AreEqual(toolTypeValues.Length, toolTypeValues.Distinct().Count());
    }
    
    [Test]
    public void UniqueToyTypesTest() {
      ToyType[] toyTypeValues = (ToyType[])Enum.GetValues(typeof(ToyType));

      Assert.AreEqual(toyTypeValues.Length, toyTypeValues.Distinct().Count());
    }
    
    [Test]
    public void UniquePowerupTypesTest() {
      PowerupType[] powerupTypeValues = (PowerupType[])Enum.GetValues(typeof(PowerupType));

      Assert.AreEqual(powerupTypeValues.Length, powerupTypeValues.Distinct().Count());
    }
    
    [Test]
    public void UniqueDeathTypesTest() {
      DeathType[] deathTypeValues = (DeathType[])Enum.GetValues(typeof(DeathType));

      Assert.AreEqual(deathTypeValues.Length, deathTypeValues.Distinct().Count());
    }

    [Test]
    public void UniqueCompassDirectionsTest() {
      CompassDirection[] compassDirectionValues = (CompassDirection[])Enum.GetValues(typeof(CompassDirection));

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
