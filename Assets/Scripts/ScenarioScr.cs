using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioScr : MonoBehaviour
{
    public static ScenarioScr scenarioScr;
    public GameObject pared; 
    public GameObject coin; 
    public GameObject booster; 
    public TextAsset strReader;//= Resources.Load<TextAsset>("pacmancsv");
    public int[,] ScenarioMatrix;
    int i,j,w,h,ScenarioSize=31;
    public int CoinCounter;


    //public GameObject[] Ghosts;

    // Start is called before the first frame update
    void Awake(){
        ScenarioMatrix = new int[ScenarioSize,ScenarioSize];
        w=ScenarioSize;
        h=ScenarioSize;
        for(i=0; i<w; i++) for(j=0; j<h; j++) if(ScenarioMatrix[j,i]==3) CoinCounter++;
        
    }
    void Start()
    {
        scenarioScr=this;
        CoinCounter=0;

        int size=1;
        //
        
        
        ReadCSV(ScenarioMatrix);
        DrawScenario(ScenarioMatrix,size);
    }

    public void RestartPoint(){
        ScenarioMatrix=new int[ScenarioSize,ScenarioSize];
        ReadCSV(ScenarioMatrix);
        int size=1;
        for(i=0; i<w; i++) for(j=0; j<h; j++){
            if(ScenarioMatrix[j,i]==3)Instantiate(coin, new Vector3(i * size, -j*size, 1), Quaternion.identity);
            if(ScenarioMatrix[j,i]==5)Instantiate(booster, new Vector3(i * size, -j*size, 1), Quaternion.identity);    
        } 
    }
    
    private void DrawScenario(int[,] Matrix,int size){
        
        for(i=0; i<w; i++){
            for(j=0; j<h; j++){
                if((i==15&&j==12)||(i==16&&j==12)) continue;
                if(Matrix[j,i]==0)Instantiate(pared, new Vector3(i * size, -j*size, 0), Quaternion.identity);
            }
        }
    }
    private void ReadCSV(int[,] Matrix){

        string[] data = strReader.text.Split(new char[]{'\n'});
        
        for(int i=0; i<ScenarioSize; i++){
            var data_values = data[i].Split(',');
            for(int j=0; j<ScenarioSize; j++){
                //Debug.Log(i+" "+j+" "+data_values[j]);
                Matrix[i,j]=System.Int32.Parse(data_values[j]);
            }
        }
        
        
    }

}
