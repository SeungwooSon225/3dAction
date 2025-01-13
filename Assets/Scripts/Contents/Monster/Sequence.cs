using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : IBehavior
{
    private List<IBehavior> _children;

    public Sequence(List<IBehavior> children)
    {
        _children = children;
    }

    public BehaviorState Execute()
    {
        foreach (var child in _children)
        {
            if (child.Execute() == BehaviorState.Failure)
            {
                return BehaviorState.Failure;
            }
        }
        return BehaviorState.Success;
    }
}
