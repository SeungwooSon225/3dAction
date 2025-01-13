using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehavior
{
    BehaviorState Execute();
}

public enum BehaviorState
{ 
    Success,
    Failure,
    Running
}
