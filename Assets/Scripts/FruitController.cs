using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    public static  FruitController fruitController;
    public int FruitScore;
    private void Start() {
        fruitController=this;
        //ADD SOUND    
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject==PacManager.pacManager.gameObject){
            ScoreManager.scoreManager.RaiseScore(FruitScore);
            SoundController.soundController.FruitSound();
            Destroy(gameObject);
        }
    }
}
