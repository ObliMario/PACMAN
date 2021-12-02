using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class MenuManager : MonoBehaviour
{
    private PacmanControl pacmanControl;
    private Text TextnameofPlayer;
    private Text TextMaxScore;
    private void Awake(){
        pacmanControl=new PacmanControl();
    }
    private void Start() {
        ShowMaxScore();
    }
    private void OnEnable(){
        pacmanControl.Enable();
    }
    private void OnDisable(){
        pacmanControl.Disable();
    }
    public void btnPlay(){
        SceneManager.LoadScene("SceneGame");
    }
    public void btnMenu(){
        SaveScore();
        SceneManager.LoadScene("SceneMenu");
    }
    public void btnExit(){
        SaveScore();
        Application.Quit();
    }
    public void btnExit0(){
        //SaveScore();
        Application.Quit();
    }


    private void SaveScore(){
        SaveObject saveObject;

        int PlayerScore=ScoreManager.scoreManager.Score;
        string PlayerName="Pacman";
            TextnameofPlayer=GameObject.FindWithTag("PlayerName").GetComponent<Text>();
            if(TextnameofPlayer.text!="") PlayerName=TextnameofPlayer.text;
        
        int[] Scores={0,0,0};
        string[] Names={"null","null","null"};
        
        //Check if score exists
        if(LoadScore()!=null){
            saveObject = LoadScore();
            Scores= new int[]{saveObject.playerScore1,saveObject.playerScore2,saveObject.playerScore3};
            Names= new string[]{saveObject.playerName1,saveObject.playerName2,saveObject.playerName3};
            }
        if(PlayerScore>Scores[0]){
            Scores[2]=Scores[1];
            Scores[1]=Scores[0];
            Scores[0]=PlayerScore;

            Names[2]=Names[1];
            Names[1]=Names[0];
            Names[0]=PlayerName;
        }else if(PlayerScore>Scores[1]){
            Scores[2]=Scores[1];
            Scores[1]=PlayerScore;

            Names[2]=Names[1];
            Names[1]=PlayerName;
        }else if(PlayerScore>Scores[2]){
            Scores[2]=PlayerScore;
            Names[2]=PlayerName;
        }

        
    
        //ESCRIBIR EN UN TEXT?
        saveObject = new SaveObject{
            playerName1=Names[0],
            playerName2=Names[1],
            playerName3=Names[2],
            playerScore1=Scores[0],
            playerScore2=Scores[1],
            playerScore3=Scores[2],
        };
        string json = JsonUtility.ToJson(saveObject);

        File.WriteAllText(Application.dataPath + "/save.txt",json);

    }

    private SaveObject LoadScore(){
        if(File.Exists(Application.dataPath+"/save.txt")){
            string saveString =File.ReadAllText(Application.dataPath+"/save.txt");
            SaveObject saveObject= JsonUtility.FromJson<SaveObject>(saveString);
            return saveObject;
        }else return null;
    }

    void Update()
    {
        if(pacmanControl.Menu.QuitGame.ReadValue<float>()==1){
            Application.Quit();
        }
        
    }
    void ShowMaxScore(){
        if(GameObject.FindWithTag("MaxScore")!=null){
            string MaxScoreString;
            TextMaxScore=GameObject.FindWithTag("MaxScore").GetComponent<Text>();
            
            
            if(LoadScore()==null){
                MaxScoreString="NO MAX SCORE";
            }
            else{
                SaveObject saveObject = LoadScore();
                MaxScoreString="Max Scores:\n\t\t1. "+saveObject.playerName1 +"\t\t - "+ saveObject.playerScore1+"\n\n\t\t2. "+saveObject.playerName2
                +"\t\t - "+saveObject.playerScore2+"\n\n\t\t3. "+saveObject.playerName3+"\t\t - "+saveObject.playerScore3;
            }
            TextMaxScore.text=MaxScoreString;
        }
        //else Debug.Log("NOT FOUND");
    }
    private class SaveObject{
        public string playerName1;
        public int playerScore1;
        public string playerName2;
        public int playerScore2;
        public string playerName3;
        public int playerScore3;
    }
}
    
