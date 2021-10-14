using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour
{
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

        print(func_1(15,2));
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
