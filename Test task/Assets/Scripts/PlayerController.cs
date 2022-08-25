using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class PlayerController : NetworkBehaviour 
{
    [SerializeField] private UIManager UIM; 
    
    [SerializeField] private string namePlayer = "New Player";
    private TextMeshPro namePlayerText;
    [SerializeField] private TMP_InputField inputFieldNamePlayer;

    Rigidbody rg;
    CapsuleCollider cc;
    public GameObject Tex;

    float speed = 10f;
    Vector3 directionVector;

    float TimeToJerk = 1f;
    float ReJerking = 0;
    public float ForceJerk;

    public int Scope;

    float ReImpulse = 0f;
    public float TimeFall;
    float ReFall = -1f;
    float RePlayering = 0;

    public int NumPlayer;

    private NetMan netMan;

    bool stop = false;
    public float a ;

    public GameObject Camera;
    public int q = 0;

    void Start()
    {
        if(!hasAuthority){
            Camera.SetActive(false);
        }
        if(hasAuthority){
            UIM = GameObject.FindObjectOfType<UIManager>();
        }
        transform.position = new Vector3 (transform.position.x + Random.Range(-10f,10),transform.position.y,transform.position.z + Random.Range(-10f,10));
        rg = GetComponent<Rigidbody>();
        cc = GetComponent<CapsuleCollider>();
        netMan = GameObject.FindObjectOfType<NetMan>();
        inputFieldNamePlayer = GameObject.FindObjectOfType<TMP_InputField>();
        namePlayerText = transform.GetChild(0).GetComponent<TextMeshPro>();


        if(isClient && isLocalPlayer)
        {
           SetInputManager();
        }
    }

    void Update()
    {
        if(stop == false){
            namePlayerText.text = namePlayer;
        if(hasAuthority){
        UIM.ScopeInt = Scope;
        Move();
        Jerk();
        Win();
        ScopeMaft();
        }
        Falls();


        
        }
        if(a <= 0 && Scope >= 3){
                Scope=0;
                transform.position = new Vector3 (transform.position.x + Random.Range(-10f,10),transform.position.y ,transform.position.z + Random.Range(-10f,10));
                stop = false;
            }
        a -=Time.deltaTime;
        
    }

    public void SetInputManager()
    {
        netMan.SetPlayer(this);
    }

    void Move()
    {
        if(Input.GetKey(KeyCode.W))
        {
           transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
           transform.localPosition += -transform.forward * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
           transform.localPosition += -transform.right * speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
           transform.localPosition += transform.right * speed * Time.deltaTime;
        }
    }
    void Jerk(){
        float ImpulseTime = 0.5f;
        if(Input.GetMouseButton(0)&&ReJerking<=0)
        {
             if(Input.GetKey(KeyCode.W)){
                directionVector = transform.forward;
            }
            else if(Input.GetKey(KeyCode.S)){
                directionVector = -transform.forward;
            }
            else if(Input.GetKey(KeyCode.A)){
                directionVector = transform.right;
            }
            else if(Input.GetKey(KeyCode.D)){
                directionVector = -transform.right;
            }
            rg.AddForce(directionVector*ForceJerk,ForceMode.Impulse);
            ReJerking = TimeToJerk;
            ReImpulse = ImpulseTime;
        }else{
            ReJerking -= Time.deltaTime;
            ReImpulse -= Time.deltaTime;
        }
        if(ReImpulse>0)
        {
            cc.height = ForceJerk*0.025f;
            cc.center = new Vector3(0,1.16f,-ForceJerk);
        }
        else{
            cc.height = 0f;
            cc.center = new Vector3(0,1.16f,0f);
        }
    }
    void Falls()
    {
        if(ReFall < 0 && tag == "FallPlayer")
       {
           Tex.tag = "FallPlayer";
           ReFall = TimeFall + 0.2f;
           RePlayering = TimeFall;
       }
       else if( ReFall > 0 && RePlayering < 0 && tag == "FallPlayer"){
           tag = "Player";
           Tex.tag = "Player";
       }
       else{
            ReFall -= Time.deltaTime;
            RePlayering -= Time.deltaTime;
       }
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player" && ReImpulse > 0 )
        {
            Scope++;
            other.tag = "FallPlayer";
        }
    }

    public void NewName(){
        if(hasAuthority){
            if(isServer){
                ChangeName(inputFieldNamePlayer.text);
                inputFieldNamePlayer.gameObject.transform.parent.gameObject.SetActive(false);
            }else{
                CmdChangeName(inputFieldNamePlayer.text);
                inputFieldNamePlayer.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void ScopeMaft(){
            if(isServer){
                ChangeScope(Scope);

            }else{
                CmdChangeScope(Scope);
                q ++;            
            }
    }
    

    public void Win(){
        if(Scope >=3){
            UIM.SetPlayerNameWinner(namePlayer);
            stop = true;
            a = 5f;
            
        }
        
    }
    
    [SyncVar(hook = nameof(SyncScope))]
    int _SyncScope;

    void SyncScope(int oldValue, int newValue){
        Scope = newValue;
    }

    [Server]

    public void ChangeScope(int newValue){
        _SyncScope = newValue;
        
    }

    [Command]

    public void CmdChangeScope(int newValue){
        ChangeScope(newValue);
        
    }

    [SyncVar(hook = nameof(SyncName))]
    string _SyncName;

    void SyncName(string oldValue, string newValue){
        namePlayer = newValue;
    }
    [Server]
    public void ChangeName(string newValue){
        _SyncName = newValue;
        
    }

    [Command]
    public void CmdChangeName(string newValue){
        ChangeName(newValue);
        
    }

 

}
