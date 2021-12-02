using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndGameManager : MonoBehaviour
{
    public Text ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text="Score: "+ScoreManager.scoreManager.Score;
        //Debug.Log("ENDGAME");        
    }

}
