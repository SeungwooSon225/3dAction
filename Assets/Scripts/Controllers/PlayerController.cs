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

    //protected Dictionary<string, float> _attackRatio = new Dictionary<string, float>();
    protected Dictionary<string, ParticleSystem> _effects = new Dictionary<string, ParticleSystem>(); 

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _playerStat = gameObject.GetComponent<PlayerStat>();
        _animator = gameObject.GetComponent<Animator>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        UI_Stat uiStat = Managers.UI.ShowUI<UI_Stat>();
        uiStat.PlayerStat = _playerStat;


        _movementDir = Vector3.forward;

        // To do
        //_playerStat.StaminaMpConsumption.Add("Dodge", 10f);

        Transform effects = transform.Find("Effects");
        Transform skillE = effects.Find("SkillE");
        _effects.Add("SkillE", skillE.GetComponent<ParticleSystem>());
        skillE.parent = null;
        Transform skillR = effects.Find("SkillR");
        _effects.Add("SkillR", skillR.GetComponent<ParticleSystem>());
        skillR.parent = null;
    }

    protected virtual void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (evt)
        {
            case Define.MouseEvent.LeftShortClick:
                if (_playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["BasicAttack"])
                    _animator.SetTrigger("LeftShortClick");

                break;
            case Define.MouseEvent.LeftLongClick:
                _animator.SetTrigger("LeftLongClick");
                break;
        }
    }

    protected virtual void Skill()
    {
        if (_animator.GetBool("IsAttacking") || _animator.GetBool("IsDodging"))
            return;

        if (Input.GetKeyDown(KeyCode.E) && _playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["SkillE"])
        {
            _animator.SetTrigger("SkillE");
        }
        else if (Input.GetKeyDown(KeyCode.R) && _playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["SkillR"])
        {
            _animator.SetTrigger("SkillR");
        }

    }

    protected virtual void Moving()
    {
        if (!_animator.GetBool("IsDodging") && Input.GetKeyDown(KeyCode.Space) && _playerStat.StaminaMp >= _playerStat.StaminaMpConsumption["Dodge"])
        {
            _playerStat.StaminaMp -= _playerStat.StaminaMpConsumption["Dodge"];
            _animator.SetTrigger("Dodge");
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
              
            if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward.normalized, out RaycastHit hit, 0.5f) ||
                (!hit.collider.CompareTag("Obstacle") && !hit.collider.CompareTag("Monster")))
            {
                transform.position += _movementDir * Time.deltaTime * _playerStat.MoveSpeed;
            }
        }
        else 
        {
            if (_animator.GetBool("IsRunning"))
                _animator.SetBool("IsRunning", false);
        } 
    }

    protected virtual void OnDodgeEvent() { }

    //protected virtual void SetDamage(Attack attack, string attackName)
    //{
    //    attack.Damage = _stat.Attack * _attackRatio[attackName];
    //}

    protected IEnumerator MoveForwardCo(float distance, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * distance;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration; // 진행 비율 (0~1)

            // 위치 업데이트
            if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward.normalized, out RaycastHit hit, 0.5f) ||
                (!hit.collider.CompareTag("Obstacle") && !hit.collider.CompareTag("Monster")))
            {
                transform.position = Vector3.Slerp(startPosition, targetPosition, t);
            }

            yield return null;
        }
    }

    #region Animation
    private void SetIsAttacking(int value)
    {
        _animator.SetBool("IsAttacking", value == 0 ? false : true);
    }

    private void DoFastRotation()
    {
        StartCoroutine(FastRotationCo());
    }

    private IEnumerator FastRotationCo()
    {
        while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(_movementDir)) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_movementDir), 30f * Time.deltaTime);

            yield return null;
        }

        yield return null;
    }

    private void ResetClickTriggers()
    {
        _animator.ResetTrigger("LeftShortClick");
        _animator.ResetTrigger("LeftLongClick");
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

        StartCoroutine(MoveForwardCo(distance, duration));
    }

    private void PlayEffect(string effectName)
    {
        _effects[effectName].gameObject.transform.position = transform.position;
        _effects[effectName].gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
        _effects[effectName].Play();
    }

    private void ShootProjectile(string name)
    {
        GameObject projectile = Managers.Resource.Instantiate($"Projectiles/{name}");
        projectile.GetComponent<Projectile>().Shoot(_playerStat);
    }

    #endregion Animation
}
