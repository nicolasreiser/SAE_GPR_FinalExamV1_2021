using NUnit.Framework;
using UnityEngine;

public class LootTests
{
    [Test]
    public void EmptyLootDescriptionTest()
    {
        var lootDescription = ScriptableObject.CreateInstance<LootDescription>();

        lootDescription.SetDrops();
        for (int i = 0; i < 100; i++)
        {
            var drop = lootDescription.SelectDropRandomly();
            Assert.AreEqual(null, drop);
        }

        Object.DestroyImmediate(lootDescription);
    }

    [Test]
    public void CertainDropLootDescriptionTest()
    {
        var lootDescription = ScriptableObject.CreateInstance<LootDescription>();
        var testDrop = ScriptableObject.CreateInstance<Drop>();
        testDrop.DropName = "TestDrop";

        lootDescription.SetDrops(new DropProbabilityPair[] {
            new DropProbabilityPair() { Drop = testDrop, Probability = 1 } });

        for (int i = 0; i < 100; i++)
        {
            var drop = lootDescription.SelectDropRandomly();
            Assert.AreEqual(testDrop, drop);
        }

        Object.DestroyImmediate(lootDescription);
        Object.DestroyImmediate(testDrop);
    }
}
