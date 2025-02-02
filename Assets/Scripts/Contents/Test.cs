using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    Canvas canvas;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 uiPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, canvas.worldCamera, out uiPosition);

        rectTransform.localPosition = uiPosition;
    }
}
