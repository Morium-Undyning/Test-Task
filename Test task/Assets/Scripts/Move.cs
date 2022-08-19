using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class Move : NetworkBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasAuthority){
        if(Input.GetKey(KeyCode.W)){
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
       }
       
       if(Input.GetKey(KeyCode.S)){
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
       }
       
       if(Input.GetKey(KeyCode.A)){
        transform.Rotate(Vector3.up,  -turnSpeed * Time.deltaTime);
       }
       
       if(Input.GetKey(KeyCode.D)){
        transform.Rotate(Vector3.up,  turnSpeed * Time.deltaTime);
       }   
        }
    }
   
}
