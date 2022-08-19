using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallcolour : MonoBehaviour
{
    public Texture Fall;
    public Texture Defaute;

    Renderer rd;
    void Start()
    {
        rd = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tag == "FallPlayer")
        {
            rd.material.SetTexture("_MainTex", Fall);
        }else if(tag == "Player")
        {
            rd.material.SetTexture("_MainTex", Defaute);
        }
    }

}
