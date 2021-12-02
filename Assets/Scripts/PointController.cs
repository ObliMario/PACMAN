using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject==PacManager.pacManager.gameObject){
            ScoreManager.scoreManager.RaiseScore(10);
            SoundController.soundController.WakaSound();
            ScoreManager.scoreManager.EatCoin();
            Destroy(gameObject);
        }
    }
}
