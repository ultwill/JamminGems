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
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberOfGameSessions > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(int clip)
    {
        Source.PlayOneShot(audioClips[clip]);
    }
}
