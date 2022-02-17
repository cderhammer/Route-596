using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// https://www.youtube.com/watch?v=HVCsg_62xYw&t=195s
public class Animation : MonoBehaviour
{

public Animator anim;

void start(){
    anim = GetComponent<Animator>();
}
void update(){
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

}
}
