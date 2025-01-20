using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster
    }

    public enum MouseEvent
    {
        LeftShortClick,
        LeftLongClick,
    }

    public class AttackWeight
    { 
        public Attack Attack { get; set; }
        public float Weight { get; set; }

        public AttackWeight(Attack attack = null, float weight = 1f)
        {
            Attack = attack;
            Weight = weight;
        }
    }
}
