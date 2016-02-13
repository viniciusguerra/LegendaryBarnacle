using UnityEngine;
using System.Collections;
using System;

public class AnimationEventHandler : MonoBehaviour
{
    public string[] methods;
    public GameObject[] targets;

    public void CallMethod(string method)
    {
        if (Array.Exists(methods, x => x == method))
        {
            GameObject target = targets[Array.FindIndex(methods, x => x == method)];

            if (target != null)
                target.SendMessage(method);
            else
                print("No Target set for " + method);
        }
        else
            print(method + " method not set");
    }
}
