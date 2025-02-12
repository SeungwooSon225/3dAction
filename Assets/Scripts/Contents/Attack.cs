using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Basic,
    Heavy,
}

public class Attack : MonoBehaviour
{
    public AttackType AttackType { get; set; } = AttackType.Basic;

    public float Damage { get; set; }

    //public bool IsActive { get; set; }
    [SerializeField]
    private bool _isPlayer = true;

    public bool IsPlayer { get { return _isPlayer; } set { _isPlayer = value; } }


    protected virtual void AttackOnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        //if (!IsActive || (other.tag == "Player" && IsPlayer)) return;
        if (other.GetComponent<Attack>() != null) return;
        if (other.tag == "Player" && _isPlayer) return;
        if (other.tag == "Monster" && !_isPlayer) return;

        Debug.Log(other.name + " " +gameObject.name);
        Stat stat = other.GetComponent<Stat>();

        if (stat == null) return;

        stat.OnAttacked(this);
    }


    private void OnTriggerEnter(Collider other)
    {
        AttackOnTriggerEnter(other);
    }
}
