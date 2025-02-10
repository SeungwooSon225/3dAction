using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define 
{
    public enum Scene
    { 
        LobbyScene,
        GameScene,
    }

    public enum PlayerClass
    {
        Warrior,
        Wizard,
    }

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
        LeftClickHold,
        LeftClickUp,
    }

    public enum UIEvent
    {
        Click,
        Drag,
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
