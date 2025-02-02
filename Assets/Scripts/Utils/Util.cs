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
        // ���� �ڽ� ���� �˻�
        Transform result = parent.Find(name);
        if (result != null) return result;

        // ��� �ڽ��� ���� ���� Ž��
        foreach (Transform child in parent)
        {
            result = FindDeepChild(child, name);
            if (result != null) return result;
        }

        return null;
    }
}
