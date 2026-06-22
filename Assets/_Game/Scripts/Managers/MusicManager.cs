using DG.Tweening;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioClip _initialMusic;

    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();

        Play(_initialMusic);
    }

    public void Play(AudioClip music)
    {
        if (music == null) return;

        _source.volume = 1f;
        _source.clip = music;
        _source.loop = true;
        _source.Play();
    }

    public void Stop()
    {
        _source.DOFade(0f, 1f).OnComplete(() =>
        {
            _source.Stop();
            _source.clip = null;
        });
    }

    public void PlayWithFadeIn(AudioClip music, float fadeInTime = 1f)
    {
        if (music == null) return;

        _source.volume = 0;
        _source.clip = music;
        _source.loop = true;
        _source.Play();

        _source.DOFade(1f, fadeInTime);
    }
}
