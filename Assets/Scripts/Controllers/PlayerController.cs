using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    protected PlayerStat _playerStat;
    protected Animator _animator;

    protected Vector3 _movementDir;

    [SerializeField]
    bool _isDodging;
    [SerializeField]
    bool _isAttacking;
    public bool IsDodging { get { return _isDodging; } set { _isDodging = value; } }
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }

    protected Dictionary<string, ParticleSystem> _effects = new Dictionary<string, ParticleSystem>();

    protected UI_Stat _uiStat;

    protected IEnumerator _moveForwardCo;
    protected IEnumerator _fastRotationCo;

    Collider _collider;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (_playerStat.IsDead)
            return;

        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        SetLockOnTarget();

        if (_isDodging)
            return;

        RecoverMpStamina();

        if (_playerStat.IsOnAttacked || _playerStat.IsDown)
            return;

        Dodge();
        Move();

        if (_isAttacking)
            return;

        Skill();
    }

    protected virtual void Init()
    {
        _playerStat = GetComponent<PlayerStat>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        _movementDir = Vector3.forward;

        Transform skillE = Util.FindDeepChild(transform, "SkillE");
        if (skillE != null)
        {
            _effects.Add("SkillE", skillE.GetComponent<ParticleSystem>());
        }
        Transform skillR = Util.FindDeepChild(transform, "SkillR");
        if (skillR != null)
        {
            _effects.Add("SkillR", skillR.GetComponent<ParticleSystem>());
        }
    }

    protected virtual void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_playerStat.IsOnAttacked)
            return;

        switch (evt)
        {
            case Define.MouseEvent.LeftShortClick:
                if (_playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["BasicAttack"])
                    _animator.SetTrigger("LeftShortClick");
                break;
            case Define.MouseEvent.LeftLongClick:
                if (_playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["ChargeAttack"])
                    _animator.SetTrigger("LeftLongClick");
                break;
            case Define.MouseEvent.LeftClickUp:
                _animator.SetTrigger("LeftClickUp");
                break;
        }
    }

    protected virtual void Skill()
    {
        Define.Skill skill = Managers.Input.GetSkillInput();

        switch (skill)
        {
            case Define.Skill.E:
                if (_playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["SkillE"] && !_uiStat.IsSkillECool)
                {
                    _animator.SetTrigger("SkillE");
                    StartCoroutine(_uiStat.SkillECoolDown());
                }
                break;

            case Define.Skill.R:
                if (_playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["SkillR"] && !_uiStat.IsSkillRCool)
                {
                    _animator.SetTrigger("SkillR");
                    StartCoroutine(_uiStat.SkillRCoolDown());
                }
                break;
        }
    }

    protected virtual void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["Dodge"])
        {
            _playerStat.StaminaMp -= _playerStat.StaminaMpConsumption["Dodge"];

            _animator.SetTrigger("Dodge");
            ResetClickTriggers();

            if (_fastRotationCo != null) StopCoroutine(_fastRotationCo);
            if (_moveForwardCo != null) StopCoroutine(_moveForwardCo);

            OnDodgeEvent();
        }
    }

    protected virtual void Move()
    {
        Vector3 movementDir = Managers.Input.GetMovementInput();

        if (movementDir != Vector3.zero)
        {
            if (_animator.GetBool("IsRunning") == false)
                _animator.SetBool("IsRunning", true);

            // 공격 콤보 도중 방향 전환을 위해 만약 공격 중이라면 _movementDir만 업데이트 한 후 실제 이동은 하지 않는다.
            _movementDir = movementDir;
            if (_isAttacking)
                return;

            // 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_movementDir), 20f * Time.deltaTime);

            // 앞에 장애물이 있으면 못움직임
            if (Physics.Raycast(transform.position + Vector3.up * 1.5f, transform.forward.normalized, out RaycastHit hit, 0.5f) &&
                hit.collider.CompareTag("Obstacle"))
            {
                return;
            }

            // 몬스터와 충돌 방지
            if ((Managers.Game.Monster.transform.position - transform.position).magnitude < 2.5f)
            {
                Vector3 dir = transform.position - Managers.Game.Monster.transform.position;
                transform.position += dir * Time.deltaTime * _playerStat.MoveSpeed;

                return;
            }

            // 이동
            transform.position += _movementDir * Time.deltaTime * _playerStat.MoveSpeed;
        }
        else
        {
            if (_animator.GetBool("IsRunning"))
                _animator.SetBool("IsRunning", false);
        }
    }

    protected virtual void OnDodgeEvent() { }

    protected virtual void RecoverMpStamina() { }

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

            // 앞에 장애물이 있으면 못움직인다
            if (Physics.Raycast(transform.position + Vector3.up * 1.5f, transform.forward.normalized, out RaycastHit hit, 0.5f) &&
                hit.collider.CompareTag("Obstacle"))
            {
                Vector3 dir = transform.position - hit.transform.position;
                targetPosition += dir * Time.deltaTime * speed;
            }

            // 몬스터와 충돌 방지
            if ((Managers.Game.Monster.transform.position - transform.position).magnitude < 2.5f)
            {
                Vector3 dir = transform.position - Managers.Game.Monster.transform.position;
                targetPosition += dir * Time.deltaTime * speed;
                //continue;
            }

            // 위치 업데이트
            transform.position = Vector3.Slerp(startPosition, targetPosition, t);     
        }
    }

    void SetLockOnTarget()
    {
        if (Input.GetKeyDown(KeyCode.F) && _playerStat.Target == null)
        {
            _playerStat.Target = Managers.Game.Monster.transform;
        }
        else if (_playerStat.Target != null)
        {
            _playerStat.Target = null;
        }
    }

    #region Animation
    private void SetIsAttacking(int value)
    {
        //_animator.SetBool("IsAttacking", value == 0 ? false : true);

        _isAttacking = value == 0 ? false : true;
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
        if (_playerStat.Target == null)
        {
            while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_movementDir)) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_movementDir), 30f * Time.deltaTime);

                yield return null;
            }
        }
        else
        {
            while (_playerStat.Target != null && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_playerStat.Target.position - gameObject.transform.position)) > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_playerStat.Target.position - gameObject.transform.position), 30f * Time.deltaTime);

                yield return null;
            }
        }
        
        yield return null;
    }

    protected void ResetClickTriggers()
    {
        _animator.ResetTrigger("LeftShortClick");
        _animator.ResetTrigger("LeftLongClick");
        _animator.ResetTrigger("LeftClickUp");
        _animator.ResetTrigger("SkillE");
        _animator.ResetTrigger("SkillR");
    }

    private void SpendStaminaMp(string attackName)
    {
        if (_playerStat.StaminaMp >= _playerStat.StaminaMpConsumption[attackName])
        {
            _playerStat.StaminaMp -= _playerStat.StaminaMpConsumption[attackName];
        }
    }

    protected void MoveForward(int distanceDuration)
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

    private void PlayEffect(string effectName)
    {
        _effects[effectName].gameObject.transform.position = transform.position;
        _effects[effectName].gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
        //Debug.Log($"e {transform.forward}, {_effects[effectName].gameObject.transform.rotation} {_effects[effectName].gameObject.transform.position}");
        _effects[effectName].Play();
    }

    private void SetAttackableTrue()
    {
        _collider.enabled = true;
    }

    private void SetAttackableFalse()
    {
        _collider.enabled = false;
    }

    private void SetIsDodgingTrue()
    {
        _isDodging = true;
    }

    private void SetIsDodgingFalse()
    {
        _isDodging = false;
    }

    private void ShootProjectile(string name)
    {
        GameObject projectile = Managers.Resource.Instantiate($"Projectiles/{name}");
        projectile.GetComponent<Projectile>().Shoot(_playerStat);
    }

    #endregion Animation
}
