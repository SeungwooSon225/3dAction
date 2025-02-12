using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    protected Animator _animator;

    [SerializeField]
    protected Vector3 _movementDir;
    Vector3 _movementDirX;
    Vector3 _movementDirY;

    protected PlayerStat _playerStat;
    protected IEnumerator _moveForwardCo;
    protected IEnumerator _fastRotationCo;

    protected bool _isCanDodge;

    //protected Dictionary<string, float> _attackRatio = new Dictionary<string, float>();
    protected Dictionary<string, ParticleSystem> _effects = new Dictionary<string, ParticleSystem>();

    protected UI_Stat _uiStat;

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

        if (Input.GetKeyDown(KeyCode.F))
            SetLockOnTarget();

        RecoverMpStamina();

        if (_playerStat.IsOnAttacked || _playerStat.IsDown)
            return;

        Moving();

        if (_animator.GetBool("IsAttacking") || _animator.GetBool("IsDodging"))
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

        _isCanDodge = true;

        Transform effects = transform.Find("Effects");
        if (effects != null)
        {
            Transform skillE = effects.Find("SkillE");
            if (skillE != null)
            {
                _effects.Add("SkillE", skillE.GetComponent<ParticleSystem>());
                skillE.parent = null;
            }

            Transform skillR = effects.Find("SkillR");
            if (skillR != null)
            {
                _effects.Add("SkillR", skillR.GetComponent<ParticleSystem>());
                skillR.parent = null;
            }
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
        if (Input.GetKeyDown(KeyCode.E) && _playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["SkillE"] && !_uiStat.IsSkillECool)
        {
            _animator.SetTrigger("SkillE");
            StartCoroutine(_uiStat.SkillECoolDown());
        }
        else if (Input.GetKeyDown(KeyCode.R) && _playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["SkillR"] && !_uiStat.IsSkillRCool)
        {
            _animator.SetTrigger("SkillR");
            StartCoroutine(_uiStat.SkillRCoolDown());
        }

    }

    protected virtual void Moving()
    {
        if (_isCanDodge && !_animator.GetBool("IsDodging") && Input.GetKeyDown(KeyCode.Space) && _playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["Dodge"])
        {
            _playerStat.StaminaMp -= _playerStat.StaminaMpConsumption["Dodge"];
            _animator.SetTrigger("Dodge");
            if (_fastRotationCo != null) StopCoroutine(_fastRotationCo);
            if(_moveForwardCo != null) StopCoroutine(_moveForwardCo);
            OnDodgeEvent();
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (_animator.GetBool("IsRunning") == false)
                _animator.SetBool("IsRunning", true);

            _movementDirX = Vector3.zero;
            _movementDirY = Vector3.zero;

            if (_animator.GetBool("IsDodging"))
                return;

            if (Input.GetKey(KeyCode.W))
            {
                _movementDirX = Camera.main.transform.forward;
            }

            if (Input.GetKey(KeyCode.S))
            {
                _movementDirX = -Camera.main.transform.forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                _movementDirY = -Camera.main.transform.right;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _movementDirY = Camera.main.transform.right;
            }

            _movementDirX.y = 0f;
            _movementDirY.y = 0f;
            _movementDir = (_movementDirX.normalized + _movementDirY.normalized).normalized;

            if (_animator.GetBool("IsAttacking"))
                return;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_movementDir), 20f * Time.deltaTime);

            // 앞에 장애물이 있으면 못움직인다
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

            transform.position += _movementDir * Time.deltaTime * _playerStat.MoveSpeed; 
        }
        else 
        {
            if (_animator.GetBool("IsRunning"))
                _animator.SetBool("IsRunning", false);
        } 
    }

    protected virtual void OnDodgeEvent() 
    {
        ResetClickTriggers();
    }

    protected virtual void RecoverMpStamina() { }

    protected IEnumerator MoveForwardCo(float distance, float duration)
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

            // 위치 업데이트

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

            transform.position = Vector3.Slerp(startPosition, targetPosition, t);     
        }
    }

    void SetLockOnTarget()
    {  
        //Debug.Log("S L T " + (gameObject.transform.position - Managers.Game.Monster.transform.position).magnitude);
        if (_playerStat.Target == null && (gameObject.transform.position - Managers.Game.Monster.transform.position).magnitude < 1000f)
        {
            //Debug.Log("S L T sdf" + Managers.Game.Monster);
            _playerStat.Target = Managers.Game.Monster.transform;
        }
        else if (_playerStat.Target != null)
            _playerStat.Target = null;
    }

    #region Animation
    private void SetIsAttacking(int value)
    {
        _animator.SetBool("IsAttacking", value == 0 ? false : true);
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

    private void MoveForward(int distanceDuration)
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

        //Debug.Log($"{distance} {duration}");
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
        //_playerStat.IsAttackable = true;
        _collider.enabled = true;
    }

    private void SetAttackableFalse()
    {
        //_playerStat.IsAttackable = false;
        _collider.enabled = false;
    }

    private void SetIsCanDodgeTrue()
    {
        _isCanDodge = true;
    }

    private void SetIsCanDodgeFalse()
    {
        _isCanDodge = false;
    }

    private void ShootProjectile(string name)
    {
        //Debug.Log("Shoot " + name);
        GameObject projectile = Managers.Resource.Instantiate($"Projectiles/{name}");
        projectile.GetComponent<Projectile>().Shoot(_playerStat);
    }

    #endregion Animation
}
