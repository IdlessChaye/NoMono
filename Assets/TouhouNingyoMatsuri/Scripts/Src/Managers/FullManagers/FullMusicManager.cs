using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{

	[RequireComponent(typeof(AudioSource))]
	public class FullMusicManager : FullSingleton<FullMusicManager>
	{
		public string nowPlayingName;

		private AudioSource audioSource;
		private Dictionary<string, AudioClip> audioClipDict = new Dictionary<string, AudioClip>();
		private const string MusicResourcesRootPath = "Music/";
		private float volume;
		private IEnumerator coroutine;

		public override void Init()
		{
			this.gameObject.name = "FullMusicManager";
			audioSource = GetComponent<AudioSource>();
			volume = 0.6f;
			coroutine = null;
			audioSource.volume = volume;
			nowPlayingName = "希腊奶";

			//GetAudioClip("趣味工房にんじんわいん - 添い寝人形");
		}

		public void Play(string musicName, float crossfade = 1500f)
		{
			if (coroutine != null)
				StopCoroutine(coroutine);

			AudioClip audioClip = GetAudioClip(musicName);
			if (audioClip == null)
				throw new System.Exception("NoThisMusic!");
			nowPlayingName = audioClip.name;
			audioSource.clip = audioClip;
			audioSource.loop = true;
			audioSource.Play();
			coroutine = MusicCrossFadeAdd(musicName, crossfade);
			StartCoroutine(coroutine);
		}

		public void Stop(float crossfade = 1500f)
		{
			if (coroutine != null)
				StopCoroutine(coroutine);

			nowPlayingName = "希腊奶";
			coroutine = MusicCrossFade(false, "", crossfade);
			StartCoroutine(coroutine);
		}

		public void Change(string musicName, float crossfade)
		{
			if (coroutine != null)
				StopCoroutine(coroutine);

			coroutine = MusicCrossFade(true, musicName, crossfade);
			StartCoroutine(coroutine);
		}

		IEnumerator MusicCrossFade(bool isChange, string musicName, float crossfade)
		{
			float startTime = Time.time;
			while (true)
			{
				if ((Time.time - startTime) >= crossfade / 1000f)
					break;
				audioSource.volume = volume - (Time.time - startTime) / crossfade * 1000f * volume;
				yield return new WaitForEndOfFrame();
			}
			audioSource.volume = 0f;
			if (!isChange)
			{
				audioSource.Stop();
				yield break;
			}

			AudioClip audioClip = GetAudioClip(musicName);
			if (audioClip == null)
				throw new System.Exception("NoThisMusic!");
			nowPlayingName = audioClip.name;
			audioSource.clip = audioClip;
			audioSource.loop = true;
			audioSource.Play();
			startTime = Time.time;
			while (true)
			{
				if ((Time.time - startTime) >= crossfade / 1000f)
					break;
				audioSource.volume = volume * (Time.time - startTime) / crossfade * 1000f;
				yield return new WaitForEndOfFrame();
			}
			audioSource.volume = volume;
		}

		IEnumerator MusicCrossFadeAdd(string musicName, float crossfade)
		{
			float startTime = Time.time;
			while (true)
			{
				if ((Time.time - startTime) >= crossfade / 1000f)
					break;
				audioSource.volume = volume * (Time.time - startTime) / crossfade * 1000f;
				yield return new WaitForEndOfFrame();
			}
			audioSource.volume = volume;
		}

		private AudioClip GetAudioClip(string musicName)
		{
			if (audioClipDict.ContainsKey(musicName) == true)
			{
				if (audioClipDict[musicName] == null)
				{
					AudioClip audioClip = Resources.Load(MusicResourcesRootPath + musicName) as AudioClip;
					audioClipDict[musicName] = audioClip;
					return audioClip;
				}
				else
				{
					return audioClipDict[musicName];
				}
			}
			else
			{
				AudioClip audioClip = Resources.Load(MusicResourcesRootPath + musicName) as AudioClip;
				audioClipDict.Add(musicName, audioClip);
				return audioClip;
			}
		}

		public void SetVolume(float volume)
		{
			this.volume = Mathf.Clamp(volume, 0, 1);
			audioSource.volume = this.volume;
		}

	}
}