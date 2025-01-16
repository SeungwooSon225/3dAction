using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float _head = 1.0f;

    [SerializeField]
    Vector3 _offset = new Vector3(0f, 0f, -5f);

    float _minYAngle = -30f;     // 카메라의 최소 각도 제한
    float _maxYAngle = 60f;      // 카메라의 최대 각도 제한
    [SerializeField]
    float _sensitivity = 100f;

    float _currentYaw = 0f;     // 좌우 회전 값
    float _currentPitch = 0f;  // 상하 회전 값

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
        // 마우스 입력 값 가져오기 (감쇠 없는 입력 사용)
        float mouseX = Input.GetAxisRaw("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _sensitivity;

        // 카메라 회전 계산
        _currentYaw += mouseX * Time.deltaTime;
        _currentPitch -= mouseY * Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch, _minYAngle, _maxYAngle); // 상하 회전 제한
    }

    void LateUpdate()
    {
        // 타겟 위치 계산 (플레이어의 머리 높이를 기준으로 조정 가능)
        _target = _player.position + Vector3.up * _head;

        // 카메라 회전 및 위치 계산
        Quaternion rotation = Quaternion.Euler(_currentPitch, _currentYaw, 0);
        Vector3 desiredPosition = _target + rotation * _offset;

        // 스무딩 적용 (카메라의 움직임을 부드럽게 처리)
        //transform.position = Vector3.Lerp(transform.position, desiredPosition, 30f * Time.deltaTime);
        transform.position = desiredPosition;

        // 카메라가 타겟을 항상 바라보도록 설정
        transform.LookAt(_target);
    }
}
