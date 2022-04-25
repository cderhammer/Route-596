using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamRecenter : MonoBehaviour
{

    private CinemachineFreeLook cam;
    // public float clampMin;
    // public float clampMax;
    // public float pitch;
    // public float speed;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        pitch += speed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, clampMin, clampMax);
        cam.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.y = pitch;
        */

        if (Input.GetAxis("CameraRecenter") == 1)
        {
            cam.m_RecenterToTargetHeading.m_enabled = true;
        }
        else
        {
            cam.m_RecenterToTargetHeading.m_enabled = false;
        }
    }
}
