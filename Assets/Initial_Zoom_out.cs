using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Initial_Zoom_Out : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] float camDistance;
    [SerializeField] float camSpeed = 0.1f;
    [SerializeField] float acceleration = 1.05f; 
    [SerializeField] float maxDistance;

    void Update(){
        if (vcam != null && camDistance < maxDistance){
            camDistance = Mathf.MoveTowards(camDistance, maxDistance, camSpeed * Time.deltaTime);
            vcam.m_Lens.OrthographicSize = camDistance;
            camSpeed *= acceleration;
        }
    }
}
