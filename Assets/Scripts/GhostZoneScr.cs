using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostZoneScr : MonoBehaviour
{
    GameObject GhostIn;  
   private void OnTriggerEnter2D(Collider2D collision){
        GhostIn = collision.gameObject as GameObject;
        GhostIn.GetComponent<GhostManager>().InGhostZone();
    
   }
   private void OnTriggerExit2D(Collider2D collision){
        GhostIn = collision.gameObject as GameObject;
        GhostIn.GetComponent<GhostManager>().OutGhostZone();
   }
}
