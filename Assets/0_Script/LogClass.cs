using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class LogClass
{
    static bool info = true;
    static bool warn = false;
    static bool error = false;

    public static void LogInfo(object log)
    {
        if (info)
            Debug.Log(log);
    }


    public static void LogWarn(object log)
    {
        if (warn)
            Debug.LogWarning(log);
    }

    public static void LogError(object log)
    {
        if (error)
            Debug.LogError(log);
    }
}
