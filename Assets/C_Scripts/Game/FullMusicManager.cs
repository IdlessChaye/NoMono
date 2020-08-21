using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FullMusicManager : FullSingleton<FullMusicManager> {
    public string nowPlayingName;

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> AudioClipDict = new Dictionary<string, AudioClip>();
    private const string MusicResourcesRootPath = "C_Music/";
    private float volume;
    private IEnumerator coroutine;

    public override void Initial() {
        this.gameObject.name = "FullMusicManager";
        audioSource = GetComponent<AudioSource>();
        volume = 0.6f;
        coroutine = null;
        audioSource.volume = volume;
        nowPlayingName = "希腊奶";

        GetAudioClip("趣味工房にんじんわいん - 添い寝人形");
        GetAudioClip("dBu music - 不思議な不思議なお祓い棒");
        GetAudioClip("ジャージと愉快な仲間たち - 今日ぞハレの日、われらが祭り");
        GetAudioClip("グーシャンダグー - 幽境奏楽");

        
        GetAudioClip("市松椿 - 童祭　~ Innocent Treasures");
        /*
        GetAudioClip("ブラックオアホワイト - 超电动黒白人形型魔操合体玩具");
        GetAudioClip("発热巫女~ず - Pages");
        */


    }

    public void Play(string musicName,float crossfade = 1500f) {
        if (coroutine !=null)
            StopCoroutine(coroutine);
        
        AudioClip audioClip = GetAudioClip(musicName);
        if(audioClip == null)
            throw new System.Exception("NoThisMusic!");
        nowPlayingName = audioClip.name;
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
        coroutine = MusicCrossFadeAdd(musicName, crossfade);
        StartCoroutine(coroutine);
    }

    public void Stop(float crossfade = 1500f) {
        if(coroutine != null)
            StopCoroutine(coroutine);

        nowPlayingName = "希腊奶";
        coroutine = MusicCrossFade(false, "", crossfade);
        StartCoroutine(coroutine);
    }

    public void Change(string musicName,float crossfade) {
        if(coroutine != null)
            StopCoroutine(coroutine);

        coroutine = MusicCrossFade(true, musicName, crossfade);
        StartCoroutine(coroutine);
    }

    IEnumerator MusicCrossFade(bool isChange,string musicName,float crossfade) {
        float startTime = Time.time;
        while(true) {
            if((Time.time - startTime) >= crossfade/1000f)
                break;
            audioSource.volume = volume - (Time.time - startTime) / crossfade * 1000f * volume;
            yield return new WaitForEndOfFrame();
        }
        audioSource.volume = 0f;
        if(!isChange) {
            audioSource.Stop();
            yield break;
        }

        AudioClip audioClip = GetAudioClip(musicName);
        if(audioClip == null)
            throw new System.Exception("NoThisMusic!");
        nowPlayingName = audioClip.name;
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
        startTime = Time.time;
        while(true) {
            if((Time.time - startTime) >= crossfade / 1000f)
                break;
            audioSource.volume = volume * (Time.time - startTime) / crossfade * 1000f;
            yield return new WaitForEndOfFrame();
        }
        audioSource.volume = volume;
    }

    IEnumerator MusicCrossFadeAdd(string musicName,float crossfade) {
        float startTime = Time.time;
        while(true) {
            if((Time.time - startTime) >= crossfade/1000f)
                break;
            audioSource.volume = volume * (Time.time - startTime) / crossfade * 1000f;
            yield return new WaitForEndOfFrame();
        }
        audioSource.volume = volume;
    }

    private AudioClip GetAudioClip(string musicName) {
        if(AudioClipDict.ContainsKey(musicName) == true) {
            if(AudioClipDict[musicName] == null) {
                AudioClip audioClip = Resources.Load(MusicResourcesRootPath + musicName) as AudioClip;
                AudioClipDict[musicName] = audioClip;
                return audioClip;
            } else {
                return AudioClipDict[musicName];
            }
        } else {
            AudioClip audioClip = Resources.Load(MusicResourcesRootPath + musicName) as AudioClip;
            AudioClipDict.Add(musicName, audioClip);
            return audioClip;
        }
    }

    public void SetVolume(float volume) {
        this.volume = Mathf.Clamp(volume, 0, 1);
        audioSource.volume = this.volume;
    }

}
