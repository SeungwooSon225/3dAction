using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float _head = 1.0f;

    [SerializeField]
    Vector3 _offset = new Vector3(0f, 0f, -5f);

    float _minYAngle = -30f;     // ī�޶��� �ּ� ���� ����
    float _maxYAngle = 60f;      // ī�޶��� �ִ� ���� ����
    [SerializeField]
    float _sensitivity = 100f;

    float _currentYaw = 0f;     // �¿� ȸ�� ��
    float _currentPitch = 0f;  // ���� ȸ�� ��

    [SerializeField]
    Transform _player = null;
    Vector3 _target;

    public Vector3 CameraDir { get; private set; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // ���콺 �Է� �� �������� (���� ���� �Է� ���)
        float mouseX = Input.GetAxisRaw("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _sensitivity;

        // ī�޶� ȸ�� ���
        _currentYaw += mouseX * Time.deltaTime;
        _currentPitch -= mouseY * Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch, _minYAngle, _maxYAngle); // ���� ȸ�� ����
    }

    void LateUpdate()
    {
        // Ÿ�� ��ġ ��� (�÷��̾��� �Ӹ� ���̸� �������� ���� ����)
        _target = _player.position + Vector3.up * _head;

        // ī�޶� ȸ�� �� ��ġ ���
        Quaternion rotation = Quaternion.Euler(_currentPitch, _currentYaw, 0);
        Vector3 desiredPosition = _target + rotation * _offset;

        // ������ ���� (ī�޶��� �������� �ε巴�� ó��)
        //transform.position = Vector3.Lerp(transform.position, desiredPosition, 30f * Time.deltaTime);
        transform.position = desiredPosition;

        // ī�޶� Ÿ���� �׻� �ٶ󺸵��� ����
        transform.LookAt(_target);
    }
}
