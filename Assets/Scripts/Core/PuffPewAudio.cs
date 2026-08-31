using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum PuffPewAudioCue
{
    Bgm,
    Hurt,
    Kill
}

public class PuffPewAudio : MonoBehaviour
{
    private const string BgmPath = "Assets/Audio/BGM.mp3";
    private const string HurtPath = "Assets/Audio/Hurt.wav";
    private const string KillPath = "Assets/Audio/Kill.mp3";

    private static PuffPewAudio instance;
    private static readonly Dictionary<PuffPewAudioCue, AudioClip> ClipCache = new();
    private static readonly HashSet<PuffPewAudioCue> MissingClipWarnings = new();

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    public static void Initialize()
    {
        if (instance != null)
        {
            instance.EnsureBgmPlaying();
            return;
        }

        GameObject audioObject = new("PuffPewAudio");
        instance = audioObject.AddComponent<PuffPewAudio>();
        DontDestroyOnLoad(audioObject);
        instance.SetupSources();
        instance.EnsureBgmPlaying();
    }

    public static void PlayHurt()
    {
        PlaySfx(PuffPewAudioCue.Hurt, 0.85f);
    }

    public static void PlayKill()
    {
        PlaySfx(PuffPewAudioCue.Kill, 0.8f);
    }

    private static void PlaySfx(PuffPewAudioCue cue, float volumeScale)
    {
        if (instance == null)
        {
            Initialize();
        }

        AudioClip clip = LoadClip(cue);
        if (clip == null || instance == null || instance.sfxSource == null)
        {
            return;
        }

        instance.sfxSource.PlayOneShot(clip, volumeScale);
    }

    private void SetupSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.spatialBlend = 0f;
        bgmSource.volume = 0.45f;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.spatialBlend = 0f;
        sfxSource.volume = 1f;
    }

    private void EnsureBgmPlaying()
    {
        if (bgmSource == null)
        {
            SetupSources();
        }

        if (bgmSource.isPlaying)
        {
            return;
        }

        AudioClip clip = LoadClip(PuffPewAudioCue.Bgm);
        if (clip == null)
        {
            return;
        }

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    private static AudioClip LoadClip(PuffPewAudioCue cue)
    {
        if (ClipCache.TryGetValue(cue, out AudioClip cachedClip))
        {
            return cachedClip;
        }

        string assetPath = GetAssetPath(cue);
        AudioClip clip = LoadEditorClip(assetPath);
        ClipCache[cue] = clip;

        if (clip == null && MissingClipWarnings.Add(cue))
        {
            Debug.LogWarning($"PuffPewAudio: clip not found at hardcoded path: {assetPath}");
        }

        return clip;
    }

    private static string GetAssetPath(PuffPewAudioCue cue)
    {
        return cue switch
        {
            PuffPewAudioCue.Bgm => BgmPath,
            PuffPewAudioCue.Hurt => HurtPath,
            PuffPewAudioCue.Kill => KillPath,
            _ => null
        };
    }

    private static AudioClip LoadEditorClip(string assetPath)
    {
#if UNITY_EDITOR
        return string.IsNullOrEmpty(assetPath) ? null : AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
#else
        return null;
#endif
    }
}
