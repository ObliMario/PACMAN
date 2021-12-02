using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public ScenarioScr scenarioScr;
    private float[] HousesTimes={5,4,3,2,1,0};
    private float[] ChasingTimes1={7,20,7,20,5,20,5}, ChasingTimes2={7,20,7,20,5,17.13f,0.1f}, ChasingTimes3={7,20,7,20,5,17.17f,0.1f};
    public GameObject[] Fruits = new GameObject[8];
    private int[] DotsRed={20,30,40,50,60,80,100,120};
    public GameObject[] Ghosts= new GameObject[4];
    public PacManager pacManager;
    public Text levelText;
    GhostManager ghostManager; 
    GameObject[] ExistingFruits= new GameObject[2];
    private void Awake(){
        
    }   
    void Start(){
        levelManager=this;
    }
    public void BoostRed(){
        for(int i=0; i<Ghosts.Length; i++ ){
            ghostManager=Ghosts[i].GetComponent<GhostManager>();
            ghostManager.isRedBoosted=true;
            if(ghostManager.isRed){
                //Debug.Log("RED BOOSTED!");
                Ghosts[i].GetComponent<Animator>().SetBool("isBoosted",true);
                ghostManager.moveSpeed=pacManager.moveSpeed;
            } 
        }
    }
    public void ShowFruit(int level,int iFruit){
        GameObject thisFruit;
        if      (level<=1)      thisFruit=Fruits[0];
        else if (level<=2)      thisFruit=Fruits[1];
        else if (level<=4)      thisFruit=Fruits[2];
        else if (level<=6)      thisFruit=Fruits[3];
        else if (level<=8)      thisFruit=Fruits[4];
        else if (level<=10)     thisFruit=Fruits[5];            
        else if (level<=12)     thisFruit=Fruits[6];             
        else  thisFruit=Fruits[7];
             
        ExistingFruits[iFruit-1]= Instantiate(thisFruit,new Vector3(15.5f,-17f,0f),Quaternion.identity) as GameObject;
        //ScoreManager.scoreManager.RaiseScore(thisScore);
    }

    public void DestroyFruit(){
        if(ExistingFruits[0]!=null) Destroy(ExistingFruits[0]);
        if(ExistingFruits[1]!=null) Destroy(ExistingFruits[1]);
        }
    public void SetLevelVars(int level){
        //GAME
        //Debug.Log("Next Level: "+level);
        levelText.text="Level: "+level;
        
        
        //PACMAN
            //DOTS REMAINING FOR INKY

            if      (level<=1)      pacManager.DotsRed=DotsRed[0];
            else if (level<=2)      pacManager.DotsRed=DotsRed[1];
            else if (level<=5)      pacManager.DotsRed=DotsRed[2];
            else if (level<=8)      pacManager.DotsRed=DotsRed[3];
            else if (level<=11)     pacManager.DotsRed=DotsRed[4];
            else if (level<=14)     pacManager.DotsRed=DotsRed[5];
            else if (level<=18)     pacManager.DotsRed=DotsRed[6];
            else                    pacManager.DotsRed=DotsRed[7];
            //Fruits
        //GHOSTS
        //GameObject ghost;
        
        for(int i=0; i<Ghosts.Length; i++ ){
            
            ghostManager=Ghosts[i].GetComponent<GhostManager>();
            //Restart Ghosts
            if(ghostManager.isRed)Ghosts[i].GetComponent<Animator>().SetBool("isBoosted",false);
            ghostManager.LaststateTimeLeft=new float[7]{0,0,0,0,0,0,0};
            ghostManager.waiting=true;
            ghostManager.gameTime=0;
            ghostManager.RestartGhosts(5f);
            //CHASING TIME
            if      (level<=1)  ghostManager.LaststateTimeLeft=ChasingTimes1;   
            else if (level<=4)  ghostManager.LaststateTimeLeft=ChasingTimes2;
            else                ghostManager.LaststateTimeLeft=ChasingTimes3;

            //HOUSE TIME
            if      (level<=1)      ghostManager.HouseTime=HousesTimes[0];
            else if (level<=2)      ghostManager.HouseTime=HousesTimes[1];
            else if (level<=5)      ghostManager.HouseTime=HousesTimes[2];
            else if (level<=8)      ghostManager.HouseTime=HousesTimes[3];
            else if (level<=11)     ghostManager.HouseTime=HousesTimes[4];
            else                    ghostManager.HouseTime=HousesTimes[5];
        }
    }
}
