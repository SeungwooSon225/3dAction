using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Damage { get; set; }

    //public bool IsActive { get; set; }

    public bool IsPlayer { get; set; } = true;


    private void OnTriggerEnter(Collider other)
    {
        //if (!IsActive || (other.tag == "Player" && IsPlayer)) return;
        if (other.tag == "Player" && IsPlayer) return;

        Stat stat = other.GetComponent<Stat>();

        if (stat == null) return;

        stat.OnAttacked(this);

        gameObject.GetComponent<Collider>().enabled = false;
    }
}
