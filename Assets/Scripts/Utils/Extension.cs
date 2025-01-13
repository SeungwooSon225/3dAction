using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public static class Extension
{
    public static T GetOrAddCompoenet<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddCompoenet<T>(go);
    }
}
