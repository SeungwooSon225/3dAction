using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    public PlayerStat PlayerStat { get; private set; }
    public Animator Animator { get; private set; }

    public Vector3 MovementDir { get; set; }

    public bool IsDodging { get; set; }
    public bool IsAttacking { get; set; }

    protected IEnumerator _moveForwardCo;
    protected IEnumerator _fastRotationCo;

    public PlayerStateMachine StateMachine;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!PlayerStat.IsDead)
            StateMachine.Update();
    }

    protected virtual void Init()
    {
        PlayerStat = GetComponent<PlayerStat>();
        Animator = GetComponent<Animator>();
        StateMachine = new PlayerStateMachine(this);

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    public virtual int GetIdleRecoverScale() { return 1; }
    public virtual int GetMoveRecoverScale() { return 1; }

    public abstract PlayerDodgeState CreateDodgeState(PlayerStateMachine stateMachine);

    protected virtual void OnMouseEvent(Define.MouseEvent evt)
    {
        StateMachine.HandleMouseEvent(evt);     
    }

    Vector3 CheckObstacle()
    {
        Vector3 dir = Vector3.zero;

        // 앞에 장애물이 있으면 못움직임
        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, transform.forward.normalized, out RaycastHit hit, 0.5f) &&
            hit.collider.CompareTag("Obstacle"))
        {
            dir = transform.position - hit.transform.position;
        }

        // 몬스터와 충돌 방지
        if ((Managers.Game.Monster.transform.position - transform.position).magnitude < 2.5f)
        {
            dir = transform.position - Managers.Game.Monster.transform.position;
        }

        return dir; 
    }

    public void HandleMovement(Vector3 dir)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20f * Time.deltaTime);

        Vector3 obstacleDir = CheckObstacle();
        if (obstacleDir != Vector3.zero)
        {
            transform.position += obstacleDir * Time.deltaTime * PlayerStat.MoveSpeed;
        }
        else
        {
            transform.position += MovementDir * Time.deltaTime * PlayerStat.MoveSpeed;
        }
    }

    public void MoveForward(int distanceDuration)
    {
        int first = distanceDuration / 1000;
        distanceDuration -= first * 1000;
        int second = distanceDuration / 100;
        distanceDuration -= second * 100;
        int third = distanceDuration / 10;
        distanceDuration -= third * 10;
        int forth = distanceDuration;

        float distance = first + second * 0.1f;
        float duration = third + forth * 0.1f;

        if (_moveForwardCo != null) StopCoroutine(_moveForwardCo);
        _moveForwardCo = MoveForwardCo(distance, duration);
        StartCoroutine(_moveForwardCo);
    }

    IEnumerator MoveForwardCo(float distance, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * distance;
        float elapsedTime = 0f;
        float speed = distance / duration;

        while (elapsedTime < duration)
        {
            yield return null;

            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration; // 진행 비율 (0~1)        

            Vector3 obstacleDir = CheckObstacle();

            if (obstacleDir != Vector3.zero)
            {
                targetPosition += obstacleDir * Time.deltaTime * speed;
            }

            // 위치 업데이트
            transform.position = Vector3.Slerp(startPosition, targetPosition, t);     
        }
    }

    private void DoFastRotation()
    {
        if (_fastRotationCo != null)
            StopCoroutine(_fastRotationCo);
        _fastRotationCo = FastRotationCo();
        StartCoroutine(_fastRotationCo);
    }

    private IEnumerator FastRotationCo()
    {
        if (PlayerStat.Target == null)
        {
            while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(MovementDir)) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MovementDir), 30f * Time.deltaTime);

                yield return null;
            }
        }
        else
        {
            while (PlayerStat.Target != null && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(PlayerStat.Target.position - gameObject.transform.position)) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PlayerStat.Target.position - gameObject.transform.position), 30f * Time.deltaTime);

                yield return null;
            }
        }
        
        yield return null;
    }

    private void SetIsAttackingTrue()
    {
        IsAttacking = true;
    }

    private void SetIsAttackingFalse()
    {
        IsAttacking = false;
    }

    private void SetIsDodgingTrue()
    {
        IsDodging = true;
    }

    private void SetIsDodgingFalse()
    {
        IsDodging = false;
    }

    public void StopMoveRotationCo()
    {
        if (_fastRotationCo != null) StopCoroutine(_fastRotationCo);
        if (_moveForwardCo != null) StopCoroutine(_moveForwardCo);
    }

    public virtual void RecoverMpStamina() { }

    public void ResetClickTriggers()
    {
        Animator.ResetTrigger("LeftShortClick");
        Animator.ResetTrigger("LeftLongClick");
        Animator.ResetTrigger("LeftClickUp");
        Animator.ResetTrigger("SkillE");
        Animator.ResetTrigger("SkillR");
    }
}
