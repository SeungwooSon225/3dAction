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

        // �ʱ� ������ �����ɴϴ�.
        float startAlpha = _canvasGroup.alpha;
        float duration = _fadeDuration * (1 - startAlpha);

        // ���̵� �� ����
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 1f, (startAlpha + elapsedTime) / _fadeDuration);
            _canvasGroup.alpha = newAlpha;
            yield return null; // ���� �����ӱ��� ���
        }

        // ������ �������ϰ� ����
        _canvasGroup.alpha = 1f;

        

        yield return new WaitForSeconds(3f);


        elapsedTime = 0f;

        // �ʱ� ������ �����ɴϴ�.
        startAlpha = _canvasGroup.alpha;
        duration = _fadeDuration * (startAlpha);

        // ���̵� �ƿ� ����
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, (1 - startAlpha + elapsedTime) / _fadeDuration);
            _canvasGroup.alpha = newAlpha;
            yield return null; // ���� �����ӱ��� ���
        }

        // ������ �����ϰ� ����
        _canvasGroup.alpha = 0f;
    }
}
