using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
            col.gameObject.GetComponent<PlayerMovement>().respawn = this.transform;
        }
    }
}
