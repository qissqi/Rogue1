using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    private float curBGMVolume, curSEVolume;
    public float CurBGMVolume { get => curBGMVolume;set { curBGMVolume = value;SaveManager.Instance.saveData.BGMvolume = value; } }
    public float CurSEVolume { get => curSEVolume; set { curSEVolume = value; SaveManager.Instance.saveData.SEvolume = value; } }
    public AudioSource BGM_Source;
    public AudioMixerGroup BGM_Mixer;
    public AudioMixerGroup SE_Mixer;
    public List<AudioClip> SE_clips;
    public List<AudioClip> BGM_clips;

    
    private Queue<AudioSource> SE_Source = new Queue<AudioSource>();
    private bool stopNextBgm = false;
    private Dictionary<string, AudioClip> SE_Dic = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> BGM_Dic = new Dictionary<string, AudioClip>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        var s = gameObject.AddComponent<AudioSource>();
        s.loop = false;
        s.playOnAwake = false;
        s.outputAudioMixerGroup = SE_Mixer;
        SE_Source.Enqueue(s);
        foreach (var clip in BGM_clips)
        {
            BGM_Dic.Add(clip.name, clip);
        }
        foreach (var clip in SE_clips)
        {
            SE_Dic.Add(clip.name, clip);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            PlaySE("CardPick");
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            PlayBGM("Contort_Front");
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            StopBGM();
        }
    }

    private IEnumerator PlayNextBgm(string nextBGM, float time)
    {
        yield return new WaitForSeconds(time-0.15f);
        if (stopNextBgm)
        {
            stopNextBgm = false;
        }
        else
        {
            PlayBGM(nextBGM);
        }
    }

    public void PlayBGM(string name)
    {
        StopAllCoroutines();
        var bgm = BGM_Dic[name];
        var _n = name.Split('_');
        if (_n.Length>1&& _n[1] == "Front")
        {
            var nextBGM = $"{_n[0]}_Loop";
            StartCoroutine(PlayNextBgm(nextBGM,bgm.length));
        }
        BGM_Source.Play();
        BGM_Source.volume = 0.5f;
        BGM_Source.clip = bgm;
        BGM_Source.Play();
    }

    public void PlaySE(AudioClip se)
    {
        var s = SE_Source.Dequeue();
        if(!s.isPlaying)
        {
            s.clip = se;
            s.Play();
            SE_Source.Enqueue(s);
            return;
        }
        SE_Source.Enqueue(s);
        var ns = gameObject.AddComponent<AudioSource>();
        ns.loop = false;
        ns.playOnAwake = false;
        ns.outputAudioMixerGroup = SE_Mixer;
        ns.clip = se;
        ns.Play();
        SE_Source.Enqueue(ns);
    }

    public void PlaySE(string name)
    {
        PlaySE(SE_Dic[name]);
    }

    public void SetBGMVolume(float volume)
    {
        CurBGMVolume = volume;
        float v = (1 - volume) * (1 - volume) * (1 - volume) * (-80);
        BGM_Mixer.audioMixer.SetFloat("Volume",v);
    }
    public void SetSEVolume(float volume)
    {
        CurSEVolume = volume;
        SE_Mixer.audioMixer.SetFloat("Volume", (1 - volume) * (1 - volume) * (1 - volume) * (-80));
    }

    public void StopBGM()
    {
        BGM_Source.Pause();
        StopAllCoroutines();
    }

}
