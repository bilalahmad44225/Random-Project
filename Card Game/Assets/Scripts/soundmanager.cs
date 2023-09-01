using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundmanager : MonoBehaviour
{
    [SerializeField]
    private AudioClip Flip;
    [SerializeField]
    private AudioClip CorrectMatch;
    [SerializeField]
    private AudioClip WrongMatch;
    [SerializeField]
    private AudioClip GameOver;
    [SerializeField]
    private AudioSource Source;

    public static soundmanager Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance=this;
    }
    
    // Play specific audio 
    public void PlaySoundEffect(string audioname)
    {
        switch (audioname)
        {
            case "flip":
                Source.clip=Flip;
                Source.Play();
                break;
            case "correct":
                Source.clip=CorrectMatch;
                Source.Play();
                break;
            case "wrong":
                Source.clip=WrongMatch;
                Source.Play();
                break;
            case "over":
                Source.clip=GameOver;
                Source.Play();
                break;
        }
    }
}
