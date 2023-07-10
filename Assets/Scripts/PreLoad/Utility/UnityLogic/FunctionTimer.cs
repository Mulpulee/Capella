using UnityEngine;
using System.Diagnostics;
using System;

public static class FunctionTimer
{
    public static string GetFunctionTime(Action function, int looptime = 1)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        for(int i=0;i<looptime;i++)
            function.Invoke();
        sw.Stop();
        return sw.ElapsedMilliseconds.ToString();
    }
}
