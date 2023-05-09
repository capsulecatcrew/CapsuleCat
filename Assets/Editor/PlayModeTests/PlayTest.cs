using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// https://www.youtube.com/watch?v=PDYB32qAsLU
/// using asm definitions so the test dir doesnt have to be in Editor
/// </summary>
public class PlayTest
{
    // A Test behaves as an ordinary method
    // this is boilerplate
    [Test]
    public void PlayTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    // this is boilerplate too
    [UnityTest]
    public IEnumerator PlayTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
    
    /// <summary>
    /// Used for testing things that can only be done in play mode
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator TestTransform()
    {
        var o = new GameObject();
        Assert.True(o.GetComponent<Transform>());
        o.transform.position = new Vector3(1, 0, 0); // replace with mvt fn
        yield return new WaitForSeconds(3f);
        Assert.AreEqual(new Vector3(1, 0, 0), 
            o.transform.position);
    }
}
