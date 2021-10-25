using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionTest : MonoBehaviour
{
    public UnityEvent event_0 = new UnityEvent();
    public UnityAction act_0 = delegate () { print("act_0"); };

    Action<string> act_1 = Print;

    Action<string> act_2 = delegate (string _text)
    {
        print(_text);
    };

    Func<int, int, int> func_1 = (x, y) => x + y;

    void Start()
    {
        act_1(string.Empty);

        act_1 += Print2;

        act_1("hi");

        act_2($"{func_1(7, 2)}");

        //print(func_1(15, 2));
    }

    public static void Print(string _text)
    {
        print("Action 발동");
        print(_text);
    }

    public static void Print2(string _text)
    {
        print("Action 기능 더하기");
    }
}
