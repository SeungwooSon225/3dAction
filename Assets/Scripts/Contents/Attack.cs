using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Damage { get; set; }

    //public bool IsActive { get; set; }

    [SerializeField]
    private bool _isPlayer = true;

    public bool IsPlayer { get { return _isPlayer; } set { _isPlayer = value; } }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        //if (!IsActive || (other.tag == "Player" && IsPlayer)) return;
        if (other.tag == "Player" && _isPlayer) return;
        if (other.tag == "Monster" && !_isPlayer) return;

        //Debug.Log(other.name);

        Stat stat = other.GetComponent<Stat>();

        if (stat == null) return;

        stat.OnAttacked(this);

        gameObject.GetComponent<Collider>().enabled = false;
    }
}
