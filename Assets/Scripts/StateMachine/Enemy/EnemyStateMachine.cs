using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    
     // animator for the enemy
     [field: SerializeField] public Animator Animator {get; private set; }
     
     // character controller
    [field: SerializeField] public CharacterController Controller {get; private set; }
    
    //Health
    [field: SerializeField] public Health Health {get; private set; }

    // Target

    [field: SerializeField] public Target Target {get; private set; }

    [field: SerializeField] public float drag {get; private set; }
    
    public float verticalVelocity;
    public float gravity {get; private set; } = -9.8f;
    public float intialJumpVelocity {get; private set; }

    private float groundGravity = -.05f;

    //private float maxJumpHeight = 1.2f;

    //private float maxJumpTime = 0.7f;
    // this will return the movemnt for the movement of the character with the character controller
    public Vector3 Movement =>  (Vector3.up * verticalVelocity);


    public Health PlayerHealth {get;private set;}

    private void Start(){
      
      //get the health component of the player
      PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
     // we start on the idle state
     SwitchState(new EnemyIdleState(this));
    
    }
       // this is a method coming from the statemachine class, is being called on the update funciton
    public override void CustomUpdate(float deltaTime){
    
    // call the delegate if it exists

    // call the gravity
    handleGravity(deltaTime);
    // call the set cool down delegate if it exists

    
    }

    private void handleGravity(float deltaTime)
    {
        if(verticalVelocity<0f && Controller.isGrounded){
            verticalVelocity = groundGravity * deltaTime;
            
            
        }else{
            verticalVelocity += gravity * deltaTime;
        
        }
    }

    // the normal move of the character
    public void Move(Vector3 motion, float deltaTime){
    
        //Debug.Log("This should be call now move");
        Controller.Move((motion + Movement) * deltaTime);
    
    }


    
     // we want to enable to the event on death as soon as this component is enable 
     // an we unsubcribe from on disable
     // we also enable on take damage
    private void OnEnable() {
        Health.OnDeath +=HandleDeath;
        Health.OnTakeDamage += HandleImpact;
    }

    private void OnDisable() {
       
        Health.OnDeath -=HandleDeath;
         Health.OnTakeDamage -= HandleImpact;
    }

    // it will change to the death state everytime the OnDeath event is trigger
    private void HandleDeath(){
         SwitchState(new EnemyDeadState(this));
    }


     private void HandleImpact(Vector3 directionknockback){
         SwitchState(new EnemyImpactState(this,directionknockback));
    }








}
