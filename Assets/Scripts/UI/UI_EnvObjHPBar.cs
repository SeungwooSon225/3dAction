using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnvObjHPBar : MonoBehaviour
{
    Stat _stat;

    [SerializeField]
    Slider _slider;

    void Start()
    {
        _stat = transform.parent.GetComponent<Stat>();
    }

    void Update()
    {
        transform.position = transform.parent.position + Vector3.up * (transform.parent.GetComponent<Collider>().bounds.size.y) + Vector3.up * 0.5f;
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp;
        SetHPRatio(ratio);
    }

    public void SetHPRatio(float ratio)
    {
        _slider.value = ratio;
    }
}
