using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    GameObject[] Ghosts= new GameObject[4];
    private void Start() {
        Ghosts[0]= GameObject.FindWithTag("Red");
        Ghosts[1]= GameObject.FindWithTag("Rosa");
        Ghosts[2]= GameObject.FindWithTag("Cyan");
        Ghosts[3]= GameObject.FindWithTag("Naranja");
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject==PacManager.pacManager.gameObject){
            RaiseBooster();
            ScoreManager.scoreManager.RaiseScore(50);
            SoundController.soundController.BoosterSound();
            //gameObject.GetComponent<AudioSource> ().Play ();
            Destroy(gameObject);
        }
    }
    public void RaiseBooster(){
        GameObject ghost;
        for(int i=0; i<Ghosts.Length; i++ ){
            ghost=Ghosts[i];
            //Debug.Log(ghost);
            ghost.GetComponent<GhostManager>().Fear();
        }
    }
}
