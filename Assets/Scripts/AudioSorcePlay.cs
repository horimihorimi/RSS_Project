using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSorcePlay : MonoBehaviour
{
    public AudioSource[] audioSources;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AudioPlay(int num)
    {
        audioSources[num].Play();
    }
}
