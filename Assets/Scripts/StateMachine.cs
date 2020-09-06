using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        crawl,
        walk,
        run,
    }

    public State state;
    public int myInt = 0;

    private IEnumerator crawlState()
    {
        float startTime = Time.time;
        Debug.Log("crawl: Enter");
        while (state == State.crawl)
        {
            yield return null;
        }
        Debug.Log("We were crawling for " + (Time.time - startTime) + "seconds");
        Debug.Log("crawl: Exit");
        NextState();
    }

    private IEnumerator walkState()
    {
        Debug.Log("walk: Enter");
        while (state == State.walk)
        {
            Debug.Log("AHHHHHHHHHH");
            yield return null;
        }
        Debug.Log("walk: Exit");
        NextState();
    }

    private IEnumerator runState()
    {
        Debug.Log("run: Enter");
        myInt = 0; 
        while (state == State.run)
        {
            myInt++;
            yield return null;
            if (myInt > 500)
            {
                state = State.walk;
            }
        }
        Debug.Log("Ran for this many frames " + myInt);
        Debug.Log("run: Exit");
        NextState();
    }

    private void Start()
    {
        NextState();
    }

    private void NextState()
    {
        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info = GetType().GetMethod(methodName,
                                                                System.Reflection.BindingFlags.NonPublic |
                                                                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

}




