using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SpeciesMovementTest
{

    [UnityTest]
    public IEnumerator SpeciesMovementTestWithEnumeratorPasses()
    {
        // Test for movements of a species within AR scene. Should move along only one axis
        GameObject gameObject = new GameObject();
        Species species = gameObject.AddComponent<Species>();

        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(species.transform.position.z > 0f);
        Assert.AreEqual(species.transform.position.x, 0);
        Assert.AreEqual(species.transform.position.y, 0);
    }
}
