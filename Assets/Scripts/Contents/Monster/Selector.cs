using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : IBehavior
{
    private List<IBehavior> _children;

    public Selector(List<IBehavior> children)
    {
        _children = children;
    }

    public BehaviorState Execute()
    {
        foreach (var child in _children)
        {
            if (child.Execute() != BehaviorState.Failure)
            {
                return BehaviorState.Success;
            }
        }
        return BehaviorState.Failure; // 모두 실패하면 Selector는 실패
    }
}
