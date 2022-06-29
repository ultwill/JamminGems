using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public static SoundManager Instance;
    private AudioSource Source;

    void Awake()
    {
        Instance = this;
        Source = GetComponent<AudioSource>();
    }

    public void PlaySound(int clip)
    {
        Source.PlayOneShot(audioClips[clip]);
    }
}
