using System.Collections;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource backbroundAudioSource;

    [SerializeField]
    private AudioClip[] backgroundMusic;

    private int currentClipIndex;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartMusic()
    {
        StartCoroutine(MusicRoutine());
    }

    private IEnumerator MusicRoutine()
    {
        while (true)
        {
            currentClipIndex++;

            backbroundAudioSource.clip = backgroundMusic[currentClipIndex];
            backbroundAudioSource.Play();

            var audioLength = backgroundMusic[currentClipIndex].length;

            yield return new WaitForSeconds(audioLength);
        }
    }
}