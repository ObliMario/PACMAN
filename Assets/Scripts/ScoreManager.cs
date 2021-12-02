using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager scoreManager;
    Text scoreText;
    public int Score = 0;
    public GameObject[] Ghosts= new GameObject[4];
    
    void Start()
    {
        if(scoreManager==null){
            scoreManager=this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
        Score=0;    
    }
    private void Update() {
        if(scoreText==null){
            if(GameObject.FindWithTag("ScoreText")!= null){
                scoreText= GameObject.FindWithTag("ScoreText").GetComponent<Text>();
                scoreText.text=Score+" ";
            }
        }
    }
    
    public void RaiseScore(int s){
        Score+=s;
        if(scoreText!=null) scoreText.text= "Score: \n"+ Score;
        
        //Debug.Log(Score);
    }
    public void EatCoin(){
        PacManager.pacManager.RemainingCoins-=1;
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
