using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GhostManager : MonoBehaviour
{
    public enum State{
        IDLE,
        inGhostZone,
        Chasing,
        Feared,
        Eaten,
        Roaming,
        
    }
    public State state,Globalstate;
    public float feartime=5f, roamingtime,chasetime;
    float timeLeft =5f; 
    public float[] stateTimeLeft,LaststateTimeLeft;
    public float gameTime=0;
    public bool isRed,isPink,isCyan,isOrange;
    public GameObject Pacman;
    public LayerMask wall,door;
    public float moveSpeed;
    public Transform movePoint,Pointer;
    private Vector3 GointTo;
    List<char> PossibleDirs;
    char lastDir='O';
    public char BestDir;
    public bool FirstTimeState=true;
    private Vector3 StartVector;
    float waitTime,timeStamp;
    public float HouseTime;
    bool FirstTimeinHouse=false,wasEaten=false;
    public bool waiting,isRedBoosted;
    private int t;
    public int DotsRemainingForRed;
    void Awake(){
        
        state=State.IDLE;
        //Globalstate=state;
        if(isRed)StartVector= new Vector3(15f,-13f,0f);
        if(isPink)StartVector= new Vector3(16f,-13f,0f);
        if(isCyan)StartVector= new Vector3(13f,-13f,0f);
        if(isOrange)StartVector= new Vector3(14f,-13f,0f);
    }
    
    void Start()
    {
        t=0;
        waiting=true;
        isRedBoosted=false;
        waitTime=4;
        GointTo = new Vector3(0f,0f,0f);
        state=State.IDLE;
        Globalstate=State.Roaming;
        gameTime=0;
        timeStamp=0f;
        gameObject.GetComponent<Animator>().SetBool("isFear",false);
        gameObject.GetComponent<Animator>().SetBool("isEaten",false);
        
        float PacmanSpeed=Pacman.GetComponent<PacManager>().moveSpeed;

        //SPEED
        if(isRed)moveSpeed=PacmanSpeed*0.85f;
        if(isPink)moveSpeed=PacmanSpeed*0.8f;
        if(isCyan)moveSpeed=PacmanSpeed*0.7f;
        if(isOrange)moveSpeed=PacmanSpeed*0.6f;
        
        //
        //LaststateTimeLeft=stateTimeLeft;
        

        //DIRECTION
        movePoint.parent=null;
               
    }

    void Update()
    {
        if(isRed && isRedBoosted){
            gameObject.GetComponent<Animator>().SetBool("isBoosted",true);
            moveSpeed=PacManager.pacManager.moveSpeed;
        } 
        Debug.DrawLine(gameObject.transform.position,GointTo, Color.white);
        gameTime+=Time.deltaTime;
        if(gameTime>waitTime && waiting) {
            waiting=false;
            gameTime=0;
        } 
        //if(isPink)Debug.Log(gameObject.name.ToString() +": " +state.ToString());
        //Debug.Log(gameTime);
        if(FirstTimeinHouse){
            timeStamp=gameTime;
            FirstTimeinHouse=false;
        } 
        if(isRed && gameTime>0 && state==State.IDLE && !waiting)Summon();
        if(isPink && gameTime>1 && state==State.IDLE && !waiting)Summon();
        if(isCyan && gameTime>5  && state==State.IDLE && !waiting)Summon();
        if(isOrange && gameTime>10  && state==State.IDLE && !waiting)Summon();
        if(!waiting && t<7) stateTimeLeft[t] -= Time.deltaTime;        
            if(t<7){
                if ( stateTimeLeft[t] < 0 )
                {
                    if(Globalstate==State.Chasing) {

                        //state=State.Roaming;
                        Globalstate=State.Roaming;
                        if(state==State.Chasing) state=Globalstate;
                        if(isRed && isRedBoosted && state!=State.Eaten && state!=State.Feared ){
                            state= State.Chasing;
                            Globalstate=state;
                        } 
                    }else if(Globalstate==State.Roaming){
                        Globalstate=State.Chasing;
                        if(state==State.Roaming) state=Globalstate;
                    } 
                    t++;
                    FirstTimeState=true;
                }
                
            }
        if(isRed && isRedBoosted && state!=State.Eaten && state!=State.Feared &&state!=State.IDLE &&state!=State.inGhostZone){
            state=State.Chasing;
            FirstTimeState=false;
        }
        
        switch(state){
            default:
            case State.IDLE:
            break;

            case State.Roaming:
                transform.position=Vector3.MoveTowards(transform.position, movePoint.position,moveSpeed+Time.deltaTime);
                if(FirstTimeState){
                    if(lastDir=='N') BestDir='S';
                    if(lastDir=='S') BestDir='N';
                    if(lastDir=='E') BestDir='O';
                    if(lastDir=='O') BestDir='E';
                    
                    if(Vector3.Distance(transform.position,movePoint.position)==0f){
                        //Move Movepoint
                        MovePointPossition(BestDir);
                        lastDir=BestDir;
                        FirstTimeState=false;
                    } 
                }else{
                    if(isRed) GointTo= new Vector3(29.0f,0f);
                    else if(isPink) GointTo= new Vector3(2.0f,0f);
                    else if(isOrange) GointTo= new Vector3(2.0f,-30.0f);
                    else if(isCyan) GointTo= new Vector3(29.0f,-30.0f);
                    BestDir=GetBestDirTo(GointTo);
                    
                    if(Vector3.Distance(transform.position,movePoint.position)<=0.005f){
                        //Move Movepoint
                        MovePointPossition(BestDir);
                        lastDir=BestDir;
                    }
                }
                
                
            break;

            case State.inGhostZone: 
                //Globalstate=state;
                //Debug.Log(gameTime);
                if(wasEaten){
                    wasEaten=false;
                    if(timeStamp+HouseTime>gameTime) GointTo= new Vector3(15.0f,-14.0f,0f);
                    else GointTo= new Vector3(15.0f,-11.0f,0f);     
                } else GointTo= new Vector3(15.0f,-11.0f,0f);
                
                
                
                BestDir=GetBestDirTo(GointTo);
                if(Vector3.Distance(transform.position,movePoint.position)<=0.005f){
                    //Move Movepoint
                    MovePointPossition(BestDir);
                    lastDir=BestDir;
                }
                transform.position=Vector3.MoveTowards(transform.position, movePoint.position,moveSpeed+Time.deltaTime);
            break;

            case State.Feared:
                //Globalstate=state;
                gameObject.GetComponent<Animator>().SetBool("isFear",true);
                timeLeft -= Time.deltaTime;
                if ( timeLeft < 0 )
                {
                    state=Globalstate;
                    FirstTimeState=true;
                    gameObject.GetComponent<Animator>().SetBool("isFear",false);
                }
                if(FirstTimeState){
                    if(lastDir=='N') BestDir='S';
                    if(lastDir=='S') BestDir='N';
                    if(lastDir=='E') BestDir='O';
                    if(lastDir=='O') BestDir='E';
                    
                    if(Vector3.Distance(transform.position,movePoint.position)==0f){
                        //Move Movepoint
                        MovePointPossition(BestDir);
                        lastDir=BestDir;
                        FirstTimeState=false;
                    } 
                }
                else{
                    BestDir=GetRandomDir();
                    if(Vector3.Distance(transform.position,movePoint.position)==0f){
                        //Move Movepoint
                        MovePointPossition(BestDir);
                        lastDir=BestDir;
                    } 
                }
                transform.position=Vector3.MoveTowards(transform.position, movePoint.position,moveSpeed+Time.deltaTime);
            break;

            case State.Chasing:
                if(FirstTimeState){
                    if(lastDir=='N') BestDir='S';
                    if(lastDir=='S') BestDir='N';
                    if(lastDir=='E') BestDir='O';
                    if(lastDir=='O') BestDir='E';
                    
                    if(Vector3.Distance(transform.position,movePoint.position)==0f){
                        //Move Movepoint
                        MovePointPossition(BestDir);
                        lastDir=BestDir;
                        FirstTimeState=false;
                    } 
                }else{
                    if(isOrange){
                        if(Vector3.Distance(transform.position,Pacman.transform.position)<8){
                            GointTo=new Vector3(2.0f,-30.0f);
                            gameObject.GetComponent<Animator>().SetBool("isPacmanNear",true);
                        }else{
                            GointTo=new Vector3(Pointer.position.x,Pointer.position.y,0f); 
                            gameObject.GetComponent<Animator>().SetBool("isPacmanNear",false);
                        }
                    }else{
                        GointTo=new Vector3(Pointer.position.x,Pointer.position.y,0f); 
                    }
                    BestDir=GetBestDirTo(GointTo);

                    if(Vector3.Distance(transform.position,movePoint.position)==0f){
                        //Move Movepoint
                        MovePointPossition(BestDir);
                        lastDir=BestDir;
                    }
                }
                //Move to MovePoint
                transform.position=Vector3.MoveTowards(transform.position, movePoint.position,moveSpeed+Time.deltaTime);
                //Get BestDir to Move to a target
                
                //IF orange is near Pacman, go to roam zone
                
            break;

            case State.Eaten:
                transform.position=Vector3.MoveTowards(transform.position, movePoint.position,moveSpeed+Time.deltaTime);            
                GointTo=new Vector3(15.0f,-13.0f);
                BestDir=GetBestDirTo(GointTo);
                if(Vector3.Distance(transform.position,movePoint.position)<=0.005f){
                    //Move Movepoint
                    MovePointPossition(BestDir);
                    lastDir=BestDir;
                }
            break;
        }
    }
        
 
    public void RestartGhosts(float InwaitTime){
        //
        int i;
        stateTimeLeft= new float[7];
        for(i =0; i<7; i++)stateTimeLeft[i]=LaststateTimeLeft[i];
        //
        t=0;
        gameTime=0;
        timeStamp=0f;
        waitTime=InwaitTime;
        waiting=true;
        //
        state=State.IDLE;
        Globalstate=State.Roaming;
        isRedBoosted=false;
        //
        gameObject.transform.position=StartVector;
        movePoint.transform.position=StartVector;
        GointTo = new Vector3(0f,0f,0f);
        gameObject.GetComponent<Animator>().SetBool("isFear",false);
        gameObject.GetComponent<Animator>().SetBool("isEaten",false);
    }
    public void OutGhostZone(){
        state=Globalstate;
        FirstTimeState=false;
    }    
    public void InGhostZone(){

        if(state==State.Eaten){
            FirstTimeinHouse=true;
            moveSpeed=moveSpeed/1.5f;
        }
        if(state!=State.IDLE){
            state=State.inGhostZone;
            FirstTimeState=true;
        }
        gameObject.GetComponent<Animator>().SetBool("isFear",false);
        gameObject.GetComponent<Animator>().SetBool("isEaten",false);
    }
    public void Fear(){
        timeLeft = feartime;
        if(state==State.Chasing ||state==State.Roaming){
            state=State.Feared;
            FirstTimeState=true;
        }
    }
    private void Summon(){
        state=State.inGhostZone;
        return; 
    }
    //*****When Hit With Pacman
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject == Pacman){
            if(state==State.Feared){
                //Fantasma comido
                moveSpeed=moveSpeed*1.5f;
                ScoreManager.scoreManager.RaiseScore(200);
                wasEaten=true;
                state=State.Eaten;
                //isEaten=true;
                lastDir=' ';
                gameObject.GetComponent<Animator>().SetBool("isEaten",true);
                SoundController.soundController.EatenSound();
            }else if(state!=State.Eaten){
                SoundController.soundController.DeathSound();
                KillPacman();
            }
        }
    }
    private void KillPacman(){
        PacManager.pacManager.Die();
    } 
    private void MovePointPossition(char Dir){
        Vector3 DirVector=new Vector3(0f,0f,0f);
        //Dirección a donde el jugador se quiere mover
            if(Dir=='O') DirVector=new Vector3(-1f,0f,0f);
            else if(Dir=='E') DirVector=new Vector3(1f,0f,0f);
            else if(Dir=='N') DirVector=new Vector3(0f,1f,0f);
            else if(Dir=='S') DirVector=new Vector3(0f,-1f,0f);
            
        movePoint.position+= DirVector;
    }
    private char GetRandomDir(){
        PossibleDirs = GetPossibleDirs(gameObject.transform.position.x,gameObject.transform.position.y);
        var choice = Random.Range(0,PossibleDirs.Count);
        return PossibleDirs[choice];
    }
    private char GetBestDirTo(Vector3 target){
        float targetX=target.x;
        float targetY=target.y;
        PossibleDirs = GetPossibleDirs(gameObject.transform.position.x,gameObject.transform.position.y);
        char minDistDir=' ';
        float minDist=999f;
        for(int i=0; i< PossibleDirs.Count; i++){
            float Distance=CalculateDistance(PossibleDirs[i], targetX, targetY);
            if(Distance<minDist){
                minDist=Distance;
                minDistDir=PossibleDirs[i];
            } 
        }
        return minDistDir;
    }

    private float CalculateDistance(char Dir,float targetX,float targetY){
        Vector3 DirVector= new Vector3(0f,0f,0f);
        if(Dir=='O'){
                DirVector=new Vector3(-1f,0f,0f);
            }else if(Dir=='E'){
                DirVector=new Vector3(1f,0f,0f);
            }else if(Dir=='N'){
                DirVector=new Vector3(0f,1f,0f);
            }else if(Dir=='S'){
                DirVector=new Vector3(0f,-1f,0f);
            }

        return GetDistance(DirVector[0]+movePoint.transform.position.x,DirVector[1]+movePoint.transform.position.y,targetX,targetY);
    }

    private List<char> GetPossibleDirs(float x,float y){
        List<char> outDirs = new List<char>();
        Vector3 OVector=new Vector3(-1f,0f,0f);
        Vector3 EVector=new Vector3(1f,0f,0f);
        Vector3 NVector=new Vector3(0f,1f,0f);
        Vector3 SVector=new Vector3(0f,-1f,0f);
        
        if(state!=State.inGhostZone && state!=State.Eaten){
            
            if(!Physics2D.OverlapCircle(movePoint.transform.position+OVector,.2f,wall) && lastDir!='E'&& (!Physics2D.OverlapCircle(movePoint.transform.position+OVector,.2f,door))){
                outDirs.Add('O');
            }
            if(!Physics2D.OverlapCircle(movePoint.transform.position+EVector,.2f,wall) && lastDir!='O'&& !Physics2D.OverlapCircle(movePoint.transform.position+EVector,.2f,door)){
                outDirs.Add('E');
            }
            if(!Physics2D.OverlapCircle(movePoint.transform.position+NVector,.2f,wall )&& lastDir!='S'&& !Physics2D.OverlapCircle(movePoint.transform.position+NVector,.2f,door)){
                outDirs.Add('N');
            }
            if(!Physics2D.OverlapCircle(movePoint.transform.position+SVector,.2f,wall )&& lastDir!='N'&& !Physics2D.OverlapCircle(movePoint.transform.position+SVector,.2f,door)){
                outDirs.Add('S');
            }
        }else{
            if(!Physics2D.OverlapCircle(movePoint.transform.position+OVector,.2f,wall) && lastDir!='E'){
                outDirs.Add('O');
            }
            if(!Physics2D.OverlapCircle(movePoint.transform.position+EVector,.2f,wall) && lastDir!='O'){
                outDirs.Add('E');
            }
            if(!Physics2D.OverlapCircle(movePoint.transform.position+NVector,.2f,wall )&& lastDir!='S'){
                outDirs.Add('N');
            }
            if(!Physics2D.OverlapCircle(movePoint.transform.position+SVector,.2f,wall )&& lastDir!='N'){
                outDirs.Add('S');
            }
        }
        //Debug.Log(outDirs);
        return outDirs;
    }
    private float GetDistance(float x1,float y1,float x2,float y2){
        float dx = Mathf.Abs(x2 - x1);
        float dy = Mathf.Abs(y2 - y1);

        float min = Mathf.Min(dx, dy);
        float max = Mathf.Max(dx, dy);

        float diagonalSteps = min;
        float straightSteps = max - min;

        return Mathf.Sqrt(2) * diagonalSteps + straightSteps;
    }
}
