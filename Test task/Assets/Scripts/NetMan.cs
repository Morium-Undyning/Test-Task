using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMan : NetworkManager
{
    

    private PlayerController playerObj;

    public void SetPlayer(PlayerController pl){
        playerObj = pl;
    }
    public void SetNamePlayer(){
        playerObj.NewName();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update(){
        playerObj.ScopeMaft();
    }
    }
