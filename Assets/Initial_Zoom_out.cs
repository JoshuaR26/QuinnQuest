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
    [SerializeField] public GameObject Player;

    void Start(){
        Player.GetComponent<Rigidbody2D>().gravityScale = 40f;
    }

    void Update(){
        if(camDistance == maxDistance){
            Player.GetComponent<Rigidbody2D>().gravityScale = 3f;
        }
        if (vcam != null && camDistance < maxDistance){
            camDistance = Mathf.MoveTowards(camDistance, maxDistance, camSpeed * Time.deltaTime);
            vcam.m_Lens.OrthographicSize = camDistance;
            camSpeed *= acceleration;
        }
    }
}
