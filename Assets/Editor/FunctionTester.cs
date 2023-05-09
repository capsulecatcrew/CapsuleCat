using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Test driven development example. Copied from https://www.studytonight.com/game-development-in-2D/tdd-unit-testing
///
/// Procedure: Red-Green-Refactor.
/// Red. write your test case with the code unimplemented
///      make it at least compile, then fail
/// Green. edit the code until test case passes
/// Refactor. edit code to assimilate the new changes
///
/// PlayMode tests only function when play mode is running
/// </summary>
[TestFixture] // tells unity that this is a test class used for testing
public class FunctionTester
{
    /// <summary>
    /// Ensures that the testing system is functioning properly.
    /// </summary>
    [Test]
    public void T00_PassingTest()
    {
        Assert.AreEqual(1, 1); // throws AssertionException if unequal
    }

    /// <summary>
    /// Using Functions in tests
    /// </summary>
    public Function function = new Function();
    [Test]
    public void T01_X2Y0()
    {
        Assert.AreEqual(0f, function.Value(2f));
    }
    
    /// <summary>
    /// Third exercise
    /// When x == 0, y == 4
    /// </summary>
    [Test]
    public void T02_X0Y4()
    {
        Assert.AreEqual(4f, function.Value(0f));
    }
}
