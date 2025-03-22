using UnityEngine;
using System.Collections; // Required for IEnumerator


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("Audio Clips")]
    public AudioClip clickSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip tickSound;
    public AudioClip bgmMusic;

    private Coroutine tickingCoroutine;

    void Awake()
    {
        // Make it a singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBGM();
    }

    public void PlayClick() => PlaySFX(clickSound);
    public void PlayCorrect() => PlaySFX(correctSound);
    public void PlayWrong() => PlaySFX(wrongSound);

    public void PlayBGM()
    {
        if (bgmSource && bgmMusic)
        {
            bgmSource.clip = bgmMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void StartTicking()
    {
        if (tickSound == null || sfxSource == null) return;

        if (tickingCoroutine != null)
            StopCoroutine(tickingCoroutine);

        tickingCoroutine = StartCoroutine(TickLoop());
    }

    public void StopTicking()
    {
        if (tickingCoroutine != null)
        {
            StopCoroutine(tickingCoroutine);
            tickingCoroutine = null;
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource && clip)
            sfxSource.PlayOneShot(clip);
    }

    private IEnumerator TickLoop()
    {
        while (true)
        {
            PlaySFX(tickSound);
            yield return new WaitForSeconds(1f);
        }
    }
}
