using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamRecenter : MonoBehaviour
{

    private CinemachineFreeLook cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("CameraRecenter") == 1){
            cam.m_RecenterToTargetHeading.m_enabled = true;
        } else {
            cam.m_RecenterToTargetHeading.m_enabled = false;
        }
    }
}
