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
    float h;
    float v;
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

    void Start()
    {
        if(hasAuthority)
        UIM = GameObject.FindObjectOfType<UIManager>();
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
        Move();
        Jerk();
        ScopeMaft();
        Tagexp();
        Win();
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
        if(v==0f)
        h = Input.GetAxis("Horizontal");
        if(h==0f)
        v = Input.GetAxis("Vertical");
        directionVector = new Vector3(h,0,v);
        if(Vector3.Angle(Vector3.forward,directionVector)>1f||Vector3.Angle(Vector3.forward,directionVector) ==0 )
        {
            Vector3 direct = Vector3.RotateTowards(transform.forward,directionVector,speed,0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }
        rg.velocity = Vector3.ClampMagnitude(directionVector,1)*speed;
    }
    void Jerk(){
        float ImpulseTime = 0.1f;
        if(Input.GetMouseButton(0)&&ReJerking<=0)
        {
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
            cc.center = new Vector3(0,1.16f,-ForceJerk*0.025f);
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
        if(other.tag == "Player" && ReImpulse > 0 && h != 0 || v != 0)
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
        if(hasAuthority){
            if(isServer){
                ChangeScope(Scope);
            }else{
                CmdChangeScope(Scope);
            }
        }
    }
    public void Tagexp(){
        if(hasAuthority){
            if(isServer){
                ChangeTag(tag);
            }else{
                CmdChangeTag(tag);
            }
        }
    }

    public void Win(){
        UIM.ScopeInt = Scope;
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

    [SyncVar(hook = nameof(SyncTag))]
    string _SyncTag;

    void SyncTag(string oldValue, string newValue){
        tag = newValue;
    }
    [Server]
    public void ChangeTag(string newValue){
        _SyncTag = newValue;
        
    }

    [Command]
    public void CmdChangeTag(string newValue){
        ChangeTag(newValue);
        
    }

}
