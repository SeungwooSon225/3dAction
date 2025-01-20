using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnvObjHPBar : MonoBehaviour
{
    Stat _stat;

    [SerializeField]
    Slider _slider;
    [SerializeField]
    CanvasGroup _canvasGroup;
    float _fadeDuration = 1.0f;

    IEnumerator _fadeInOutCo;

    void Start()
    {
        _stat = transform.parent.GetComponent<Stat>();
    }

    void Update()
    {
        transform.position = transform.parent.position + Vector3.up * (transform.parent.GetComponent<Collider>().bounds.size.y) + Vector3.up * 1f;
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp;
        SetHPRatio(ratio);
    }

    public void SetHPRatio(float ratio)
    {
        _slider.value = ratio;
    }

    public void FadeInOut()
    {
        if (_fadeInOutCo != null)
            StopCoroutine(_fadeInOutCo);

        _fadeInOutCo = FadeInOutCo();
        StartCoroutine(_fadeInOutCo); 
    }

    private IEnumerator FadeInOutCo()
    {
        float elapsedTime = 0f;

        // 초기 투명도를 가져옵니다.
        float startAlpha = _canvasGroup.alpha;
        float duration = _fadeDuration * (1 - startAlpha);

        // 페이드 인 시작
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1f, (startAlpha + elapsedTime) / _fadeDuration);
            _canvasGroup.alpha = newAlpha;
            yield return null; // 다음 프레임까지 대기
        }

        // 완전히 불투명하게 설정
        _canvasGroup.alpha = 1f;

        

        yield return new WaitForSeconds(3f);


        elapsedTime = 0f;

        // 초기 투명도를 가져옵니다.
        startAlpha = _canvasGroup.alpha;
        duration = _fadeDuration * (startAlpha);

        // 페이드 아웃 시작
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, (1 - startAlpha + elapsedTime) / _fadeDuration);
            _canvasGroup.alpha = newAlpha;
            yield return null; // 다음 프레임까지 대기
        }

        // 완전히 투명하게 설정
        _canvasGroup.alpha = 0f;
    }
}
