using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Awake()
    {
        SetupSingleton();
    }

    private void SetupSingleton()
    {
        int numOfMusicManagers = FindObjectsOfType<MusicManager>().Length;
        if (numOfMusicManagers > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }
}
