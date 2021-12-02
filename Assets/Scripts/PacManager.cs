using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class PacManager : MonoBehaviour
{

    public ScenarioScr scenarioScr;
    public static PacManager pacManager;
    public GameObject LivePrefab;
    public LevelManager levelManager;
    private PacmanControl pacmanControl;
    public LayerMask wall,door;
    public Transform movePoint,PinkPointer,CyanPointer,RedPosition;
    public float moveSpeed=0.2f;
    private float isRotate=0;
    private char lastDir='E',movingto='E';
    private Vector3 lastDirVector= new Vector3(-1f,0.0f,0.0f);
    int lives,level;
    public int RemainingCoins,DotsRed,DotsFruit1,DotsFruit2;
    public GameObject[] Ghosts= new GameObject[4];
    float CountTime,waitTime;
    bool existFruit1=false,existFruit2=false,dead=false;
    public GameObject GameText;
    
    private void Awake(){
        pacmanControl=new PacmanControl();
    }
    private void OnEnable(){
        pacmanControl.Enable();
    }
    private void OnDisable(){
        pacmanControl.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        dead=false;
        level=1;
        RemainingCoins= 241;
        DotsFruit1=RemainingCoins/2;
        DotsFruit2=DotsFruit1/2;
        levelManager.SetLevelVars(1);
        waitTime=4;
        CountTime=0;
        pacManager=this;
        movePoint.parent=null;
        PinkPointer.parent=null;
        CyanPointer.parent=null;
        lives=3;
        StartGame(lives);
        scenarioScr.RestartPoint();
        dead=false;
        gameObject.SetActive(true);
        ScoreManager.scoreManager.Score=0;
        
    }

    // Update is called once per frame
    void Update()
    {
        CountTime+=Time.deltaTime;
        if(dead){
            if(lives==0)GameText.GetComponent<Animator>().SetBool("GameOver",true);
            
            if(CountTime>waitTime){
                gameObject.SetActive(false);
                if(lives>0){
                    
                    StartGame(lives);
                }
                else EndGame();
                }
        }
        
        //Check for remaining dots for fruit to appear
        if(RemainingCoins<DotsFruit1&&!existFruit1){
            LevelManager.levelManager.ShowFruit(level,1);
            existFruit1=true;
        } 
        if(RemainingCoins<DotsFruit2&&!existFruit2){
            LevelManager.levelManager.DestroyFruit();
            LevelManager.levelManager.ShowFruit(level,2);
            existFruit2=true;
        } 
        
        //Check for Remaining Dots for Red Booster
        if(RemainingCoins<DotsRed) LevelManager.levelManager.BoostRed();
        //Check yo Remaining dots for next level
        if(RemainingCoins<1) NextLevel(level+1);
        
        if(pacmanControl.Menu.QuitGame.ReadValue<float>()==1) Application.Quit();
        if(CountTime>waitTime){
            
            //***************MOVING***************

            transform.position=Vector3.MoveTowards(transform.position, movePoint.position,moveSpeed+Time.deltaTime);
            float MoveH=pacmanControl.pacman.MoveH.ReadValue<float>();
            float MoveV=pacmanControl.pacman.MoveV.ReadValue<float>();

            if      (MoveH==    1f)     movingto='E';
            else if (MoveH==   -1f)     movingto='O';
            else if (MoveV==    1f)     movingto='N';
            else if (MoveV==   -1f)     movingto='S';

            if(Vector3.Distance(transform.position,movePoint.position)==0f) lastDirVector=MoveTo(movingto,lastDirVector);
        }
        
        
        //GameText.GetComponent<Animator>().SetBool("StartGame",false);
    }
    private void NextLevel(int thislevel){
        LevelManager.levelManager.DestroyFruit();
        scenarioScr.RestartPoint();
        RemainingCoins=241;
        CountTime=0;
        level=thislevel;
        levelManager.SetLevelVars(thislevel);
        existFruit1=false;
        existFruit2=false;
        StartGame(lives);
    }
    private void ApplyFlip(char Dir){
        if(Dir=='E' || Dir=='O') {
            gameObject.GetComponent<SpriteRenderer>().flipY=false;
            transform.Rotate(0.0f,0.0f,-isRotate);
            isRotate=0;
            if(Dir=='E') gameObject.GetComponent<SpriteRenderer>().flipX=true;
            else gameObject.GetComponent<SpriteRenderer>().flipX=false;   
        } 
        else if(Dir=='N' || Dir=='S') {
            transform.Rotate(0.0f,0.0f,-isRotate);
            if(gameObject.GetComponent<SpriteRenderer>().flipX==true){
                gameObject.GetComponent<SpriteRenderer>().flipX=false;
                gameObject.GetComponent<SpriteRenderer>().flipY=true;
            }
            if(Dir=='N') isRotate=-90f;
            else isRotate=90f;
            
            transform.Rotate(0.0f,0.0f,isRotate);                
        } 
    }
    private Vector3 MoveTo(char Dir, Vector3 lastDirVector){
        Vector3 DirVector=new Vector3(1f,0f,0f);
        //Dirección a donde el jugador se quiere mover
            if(Dir=='O'){
                DirVector=new Vector3(-1f,0f,0f);
            }else if(Dir=='E'){
                DirVector=new Vector3(1f,0f,0f);
            }else if(Dir=='N'){
                DirVector=new Vector3(0f,1f,0f);
            }else if(Dir=='S'){
                DirVector=new Vector3(0f,-1f,0f);
            }
        if((!Physics2D.OverlapCircle(movePoint.position+DirVector,.2f,door))&&(!Physics2D.OverlapCircle(movePoint.position+DirVector,.2f,wall))){
            //Si puedo moverme a donde quiero moverme
            gameObject.GetComponent<Animator>().SetBool("moving",true);
            movePoint.position+= DirVector;
            PinkPointer.position=gameObject.transform.position+DirVector*4;
            CyanPointer.position=gameObject.transform.position+DirVector*2;
            
            float Cyanx=CyanPointer.transform.position.x;
            float Cyany=CyanPointer.transform.position.y;
            float Redx=RedPosition.transform.position.x;
            float Redy=RedPosition.transform.position.y;
            CyanPointer.position=new Vector3(2*Cyanx-Redx,2*Cyany-Redy,0);

            ApplyFlip(Dir);
            lastDir=Dir;
            return DirVector;
        }
        else{
            //Si no puedo, contiúa a donde estaba yendo antes a menos que no pueda
            
            if(!Physics2D.OverlapCircle(movePoint.position+lastDirVector,.2f,wall)){
                gameObject.GetComponent<Animator>().SetBool("moving",true);
                movePoint.position+= lastDirVector;
                PinkPointer.position=gameObject.transform.position+lastDirVector*4;
                CyanPointer.position=gameObject.transform.position+lastDirVector*2;
                ApplyFlip(lastDir);
                return lastDirVector;
            }
            else gameObject.GetComponent<Animator>().SetBool("moving",false);
            return lastDirVector;
        }
    }

    public void Die(){
        if(!dead){
            dead=true;
            gameObject.GetComponent<Animator>().SetBool("Die",true);
            
            CountTime=0;
            waitTime=4f;
            LevelManager.levelManager.DestroyFruit();
            lives=lives-1;
        }
        //DIE animation
        //gameObject.GetComponent<Animator>().SetBool("moving",false);
            
    
        
    }
    private void EndGame(){
        //int FinalScore=ScoreManager.scoreManager.GetScore();
        
        SceneManager.LoadScene("SceneGameOver");
    }
    private void StartGame(int lives){
        //GameText.GetComponent<Animator>().SetTrigger("StartGame");
        //GameText.GetComponent<Animator>().SetBool("StartGame",true);
        gameObject.GetComponent<Animator>().SetBool("Die",false);
        SoundController.soundController.StartSound();
        lastDir='O';
        ApplyFlip('O');
        
        CountTime=0;
        ShowLives(lives);
        LevelManager.levelManager.SetLevelVars(level);
        movePoint.transform.position= new Vector3(15f,-23f,0f);
        gameObject.transform.position= new Vector3(15f,-23f,0f);
        
        dead=false;
        gameObject.SetActive(true);
        
        GameObject ghost;
        for(int i=0; i<Ghosts.Length; i++ ){
            ghost=Ghosts[i];
            //Debug.Log(ghost);
            ghost.GetComponent<GhostManager>().RestartGhosts(4f);
        }
        

    }
    private void ShowLives(int lives){
        float offset=0;
        //
        var LivePrefabtag = GameObject.FindGameObjectsWithTag("LivePrefab");
        
        foreach(var item in LivePrefabtag) Destroy(item);
        
        //
        for(int i=0; i<lives; i++){
            Instantiate(LivePrefab, new Vector3(26f+offset, -17.5f, 0f), Quaternion.identity);    
            offset+=1f;
        }
    }

}
