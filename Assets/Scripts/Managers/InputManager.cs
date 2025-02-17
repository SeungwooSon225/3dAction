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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

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


    public Vector3 GetMovementInput()
    {
        Vector3 movementDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Vector3 movementDirX = Vector3.zero;
            Vector3 movementDirY = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movementDirX = Camera.main.transform.forward;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movementDirX = -Camera.main.transform.forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                movementDirY = -Camera.main.transform.right;
            }

            if (Input.GetKey(KeyCode.D))
            {
                movementDirY = Camera.main.transform.right;
            }

            movementDirX.y = 0f;
            movementDirY.y = 0f;
            movementDir = (movementDirX.normalized + movementDirY.normalized).normalized;
        }

        return movementDir;
    }

    public Define.Skill GetSkillInput()
    {
        Define.Skill skillInput = Define.Skill.None;

        if (Input.GetKey(KeyCode.E))
        {
            skillInput = Define.Skill.E;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            skillInput = Define.Skill.R;
        }

        return skillInput;
    }
}
