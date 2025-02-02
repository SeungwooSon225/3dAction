using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static T GetOrAddCompoenet<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static Transform FindDeepChild(Transform parent, string name)
    {
        // 직속 자식 먼저 검사
        Transform result = parent.Find(name);
        if (result != null) return result;

        // 모든 자식의 하위 계층 탐색
        foreach (Transform child in parent)
        {
            result = FindDeepChild(child, name);
            if (result != null) return result;
        }

        return null;
    }
}
