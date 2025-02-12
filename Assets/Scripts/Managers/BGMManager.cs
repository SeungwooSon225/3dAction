using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    AudioSource _audioSources = new AudioSource();

    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();


    public void Init()
    {
        GameObject root = GameObject.Find("@BGM");
        if (root == null)
        {
            root = new GameObject { name = "@BGM" };
            Object.DontDestroyOnLoad(root);

            _audioSources = root.AddComponent<AudioSource>();   
            _audioSources.loop = true;
        }

        AddAudioClips();
    }

    public void Play(string name, float pitch = 1.0f)
    {
        AudioClip audioClip = GetAudioClip(name);

        if (_audioSources.isPlaying)
            _audioSources.Stop();

        _audioSources.pitch = pitch;
        _audioSources.clip = audioClip;
        _audioSources.Play();
    }


    public void AddAudioClips()
    {
        // To do
        string path = $"Sounds/BGM/Game";
        AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
        _audioClips.Add("Game", audioClip);

        path = $"Sounds/BGM/Lobby";
        audioClip = Managers.Resource.Load<AudioClip>(path);
        _audioClips.Add("Lobby", audioClip);

        path = $"Sounds/BGM/Monster";
        audioClip = Managers.Resource.Load<AudioClip>(path);
        _audioClips.Add("Monster", audioClip);
    }

    AudioClip GetAudioClip(string name)
    {
        AudioClip audioClip = null;
        _audioClips.TryGetValue(name, out audioClip);


        if (audioClip == null)
            Debug.Log($"AudioClip Missing! {name}");

        return audioClip;
    }
}
