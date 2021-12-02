using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController soundController;
    public float Volume=0.5f;
    public AudioClip Waka,BoosterS,Intermission,EatenS,DeathS,StartS;
    public GameObject GameText,PacText;
    AudioSource audiox;
    // Start is called before the first frame update
    private void Start() {
        audiox = GetComponent<AudioSource>();
        soundController=this;
    }
    public void StartSound(){
        GameText.GetComponent<Animator>().SetTrigger("StartGame");
        audiox.PlayOneShot(StartS,Volume);
    }
    public void WakaSound(){
        audiox.PlayOneShot(Waka,Volume);
    }
    public void BoosterSound(){
        audiox.PlayOneShot(BoosterS,Volume);
        audiox.PlayOneShot(Intermission,Volume);
    }

    public void FruitSound(){
        audiox.PlayOneShot(BoosterS,Volume);
    }
    
    public void DeathSound(){
        audiox.PlayOneShot(DeathS,Volume);
    }

    public void EatenSound(){
        PacText.GetComponent<Animator>().SetTrigger("EatGhost");
        audiox.PlayOneShot(EatenS,Volume);
    }
}
