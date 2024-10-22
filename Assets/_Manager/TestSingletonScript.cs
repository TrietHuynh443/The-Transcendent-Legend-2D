using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingletonScript : UnitySingleton<TestSingletonScript>
{
    protected override void SingletonAwake()
    {
        Debug.Log("Test singleton awaked");
    }

    protected override void SingletonOnDestroy()
    {
        Debug.Log("Test singleton destroy");
    }

    protected override void SingletonStarted()
    {
        Debug.Log("Test singleton start");
    }

    public void test()
    {
        Debug.Log("This is public test script");
    }
}
