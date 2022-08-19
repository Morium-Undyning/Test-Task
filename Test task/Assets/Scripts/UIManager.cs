using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject panelWin;
    [SerializeField] private TextMeshProUGUI nameWiner;
    [SerializeField] private TextMeshProUGUI Scope;
    public int ScopeInt;
    public float n =5;
    bool stop = false;

    void Update(){
        Scope.text = ScopeInt.ToString();
        if(stop == true){
            n -= Time.deltaTime;
        }
        if(n <=0){
            panelWin.SetActive(false);
            stop = false;
            n=5f;
        }
    }

    public void SetPlayerNameWinner(string NamePlayer){
        nameWiner.text = NamePlayer;
        panelWin.SetActive(true);
        stop = true;

    }
}
