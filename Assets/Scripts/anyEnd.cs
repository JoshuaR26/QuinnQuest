using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anyEnd : MonoBehaviour
{
    [SerializeField] private float endDelay = 1f;
    [SerializeField] private float endSize;
    

    private bool isEnd = false;
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")){
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}
