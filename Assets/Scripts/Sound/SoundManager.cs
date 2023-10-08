using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Manager;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
  public class SoundManager : MonoBehaviour
  {
    public AudioMixer masterMixer;
    public SerializedDictionary<SoundType, AudioMixerGroup> groups;
    private AudioSourceCollection collection;
    private Dictionary<SoundType, AudioSource> audioSources = new();

    public static readonly Dictionary<SoundType, float> defaultVolumes
      = Enum.GetValues(typeof(SoundType)).OfType<SoundType>().ToDictionary(x => x, _ => 0f);

    public void SetVolume(SoundType type, float value)
    {
      masterMixer.SetFloat(type.ToString(), value);
    }

    public float GetVolume(SoundType type)
    {
      masterMixer.GetFloat(type.ToString(), out var volume);
      return volume;
    }

    private void Awake()
    {
      collection = GetComponent<AudioSourceCollection>();

      foreach (var type in Enum.GetValues(typeof(SoundType)).OfType<SoundType>())
      {
        var obj = new GameObject(type.ToString(), typeof(AudioSource));
        obj.transform.SetParent(transform);

        var audioSource = obj.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = groups[type];
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        audioSources.Add(type, audioSource);
      }
    }

    private void Start()
    {
      foreach (var soundType in Enum.GetValues(typeof(SoundType)).OfType<SoundType>())
        SetVolume(soundType, GameManager.Data.data.volume[soundType]);
    }

    public void Play(SoundType type, string audioName)
    {
      var source = audioSources[type];
      var clip = collection.Get(audioName);

      if (type == SoundType.BGM)
      {
        source.loop = true;
        StopBGM();
        source.clip = clip;
        source.Play();
      }
      else
      {
        source.PlayOneShot(clip);
      }
    }

    public void StopBGM() => audioSources[SoundType.BGM].Stop();
  }
}
