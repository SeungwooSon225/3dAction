using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardMaterialController : MonoBehaviour
{
    Renderer _renderer;
    Material _material;
    Color _originalColor;

    [SerializeField]
    Renderer _staffRenderer;
    Material _staffMaterial;
    Color _staffOriginalColor;

    [SerializeField]
    ParticleSystem _teleportStart;
    [SerializeField]
    ParticleSystem _teleportEnd;
    [SerializeField]
    ParticleSystem _fire;

    float _duration = 0.4f;
    

    void Start()
    {
        // 초기 투명 설정 (Shader가 투명 모드를 지원하도록 설정)
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _material.SetFloat("_Surface", 1); // 0 = Opaque, 1 = Transparent
        _material.SetFloat("_Blend", 0);   // Alpha Blend Mode
        _material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        _material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        _originalColor = _material.color;

        _staffMaterial = _staffRenderer.material;
        _staffMaterial.SetFloat("_Surface", 1); // 0 = Opaque, 1 = Transparent
        _staffMaterial.SetFloat("_Blend", 0);   // Alpha Blend Mode
        _staffMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        _staffMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        _staffOriginalColor = _staffMaterial.color;
    }

    private IEnumerator FadeOutCo()
    {
        _teleportStart.Play();
        _fire.Stop();

        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0.1f, elapsedTime / _duration);
            _material.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
            _staffMaterial.color = new Color(_staffOriginalColor.r, _staffOriginalColor.g, _staffOriginalColor.b, alpha);

            yield return null;
        }

        _renderer.enabled = false;
        _staffRenderer.enabled = false;      
    }

    private IEnumerator FadeInCo()
    {
        _renderer.enabled = true;
        _staffRenderer.enabled = true;

        _teleportEnd.Play();
        _fire.Play();

        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.1f, 1f, elapsedTime / _duration);
            _material.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
            _staffMaterial.color = new Color(_staffOriginalColor.r, _staffOriginalColor.g, _staffOriginalColor.b, alpha);

            yield return null;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCo());
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCo());
    }
}
