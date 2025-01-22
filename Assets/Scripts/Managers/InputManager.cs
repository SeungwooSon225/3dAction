using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action<Define.MouseEvent> MouseAction = null;

    bool _isPressed = false;
    bool _isLongPressed = false;
    float _pressedTime = 0;

    public void OnUpdate()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //    return;

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_isPressed)
                {
                    _isPressed = true;
                    _pressedTime = Time.time;
                    MouseAction.Invoke(Define.MouseEvent.LeftShortClick);
                }
                else
                {
                    if (!_isLongPressed && Time.time > _pressedTime + 0.3f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.LeftLongClick);

                        _isLongPressed = true;
                        _pressedTime = 0;
                    }
                }
            }
            else 
            {
                if (_isPressed)
                {
                    //if (!_isLongPressed && Time.time < _pressedTime + 0.2f)
                    //{
                    //    MouseAction.Invoke(Define.MouseEvent.LeftShortClick);
                    //}

                    MouseAction.Invoke(Define.MouseEvent.LeftClickUp);

                    _isPressed = false;
                    _isLongPressed = false;
                    _pressedTime = 0;
                }
            }
        }
    }
}
