using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource music;
    [SerializeField]
    AudioClip[] tracks = new AudioClip[0];
    [SerializeField]
    int currentTrack = 0;
    // 0 = find dog track, 1 = room transformation, 2 = post-mutation track, 3 = death track

    public static Music Instance { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();

    }

    public void PlayTrack(int index)
    {
        music.Stop();
        music.clip = tracks[index];
        music.Play();
    }

    public void PlayCurrentTrack()
    {
        music.Stop();
        music.clip = tracks[currentTrack];
        music.Play();
    }

    public void StopTrack()
    {
        music.Stop();
    }

    public void ToggleLoop()
    {
        music.loop = !music.loop;
    }

    public void SetVolume(float volume)
    {
        music.volume = volume;
    }

    //Still to test
    public IEnumerator FadeVolume(float volume, float seconds)
    {
        float currentVolume = music.volume;

        //Fade up
        if (currentVolume < volume)
        {
            while (currentVolume < volume)
            {
                music.volume += currentVolume * Time.deltaTime / seconds;
                yield return null;
            }
        }
        //Fade down
        else
        {
            while (currentVolume < volume)
            {
                music.volume -= currentVolume * Time.deltaTime / seconds;
                yield return null;
            }
        }
        yield return null;
    }
}
