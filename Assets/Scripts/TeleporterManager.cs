using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public bool isR;
    public GameObject Pacman;
    private Transform PC_movePoint;
    private GameObject Ghost;
    private Transform GhostMovePoint;
    void Start(){
        PC_movePoint=Pacman.GetComponent<PacManager>().movePoint;
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(isR){
            if(collision.gameObject==Pacman){
                Pacman.transform.position= new Vector3(0f,-14f,0f);
                PC_movePoint.transform.position= new Vector3(0f,-14f,0f);
            }
            else{
                Ghost=collision.gameObject as GameObject;
                GhostMovePoint= Ghost.GetComponent<GhostManager>().movePoint;

                Ghost.transform.position= new Vector3(0f,-14f,0f);
                GhostMovePoint.transform.position= new Vector3(0f,-14f,0f);
                Ghost.GetComponent<GhostManager>().BestDir='E';
            }
            
        }else{
            if(collision.gameObject==Pacman){
                Pacman.transform.position= new Vector3(30f,-14f,0f);
                PC_movePoint.transform.position= new Vector3(30f,-14f,0f);
            }else{
                Ghost=collision.gameObject as GameObject;
                GhostMovePoint= Ghost.GetComponent<GhostManager>().movePoint;

                Ghost.transform.position= new Vector3(30f,-14f,0f);
                GhostMovePoint.transform.position= new Vector3(30f,-14f,0f);
                Ghost.GetComponent<GhostManager>().BestDir='O';
            }
        }
    }
}
