using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is the context
public class PlayerStateMachine : StateMachine
{
[field: SerializeField] public InputReader InputReader {get; private set; }

[field: SerializeField] public CharacterController Controller {get; private set; }

[field: SerializeField] public Animator Animator {get; private set; }
// gameobject weapon
[field:SerializeField] public WeaponHandler Weapon {get; private set; }
// the speed of the player
[field: SerializeField] public float FreeLookMovementSpeed {get; private set; }
// speed when is jumping
[field: SerializeField] public float JumpMoveSpeed {get; private set; }

// the damp velocity when the player is rotating
[field: SerializeField] public float RotationDampSpeed {get; private set; }

// the force of the dash
[field: SerializeField] public float DashForce {get; private set; }

// the stationary force of the dash
[field: SerializeField] public float DashStationaryForce {get; private set; }

// how long the dash will took
[field: SerializeField] public float DashTime {get; private set; }
// the cooldown for the dash
[field: SerializeField] public float DashCoolDown {get;set; }
// the force of the roll
[field: SerializeField] public float RollForce {get; private set; }

//stationary force of the roll
[field: SerializeField] public float RollStationaryForce {get; private set; }

// how long the chracter will roll
[field: SerializeField] public float RollTime {get; private set; }
// the cooldown for the roll
[field: SerializeField] public float RoolCoolDown {get;set; }


// get acces to the main camera that  is using the cinemachine
public Transform MainCameraPlayer {get; private set; }


// all this should be hide on the inspector, the vertical velocity and the currment movement input
[HideInInspector]
public Vector3 currentMovement; 
// all this data is for the jumping
[HideInInspector]
public float verticalVelocity;
public float gravity {get; private set; } = -9.8f;
public float intialJumpVelocity {get; private set; }

private float groundGravity = -.05f;

private float maxJumpHeight = 1.2f;

private float maxJumpTime = 0.7f;
// this will return the movemnt for the movement of the character with the character controller
public Vector3 Movement =>  (Vector3.up * verticalVelocity);
// delegate for the move 

public delegate void MoveDelegate(Vector3 motion, float deltaTime);

public MoveDelegate moveDelegate;

// delegates to set the cool down
public delegate void SetCoolDown(float deltaTime);

public SetCoolDown setCoolDown;

// this is the cool down that should affect the roll and the dash independenly 
// each variable is for each cooldown time
public float coolDownTimeDash {get; set; }
public float coolDownTimeRoll {get; set; }


private void Start() {
    //set up the delegate for the move
     moveDelegate = Move;
     
     setupJumpVariable();
    
    MainCameraPlayer = Camera.main.transform;
    // suscribe to an event of the dash
     InputReader.DashEvent += OnDash;
   
    SwitchState(new PlayerGroundState(this));
     
}

// this is a method coming from the statemachine class, is being called on the update funciton
public override void CustomUpdate(float deltaTime){
  
  // call the delegate if it exists
  moveDelegate?.Invoke(currentMovement,deltaTime);
  // call the gravity
  handleGravity(deltaTime);
  // call the set cool down delegate if it exists
  setCoolDown?.Invoke(deltaTime);
  
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
 
   
      Controller.Move((motion + Movement) * deltaTime);
 
}



// set the variables for the jumping
private void setupJumpVariable()
{
    float timeToApex =maxJumpTime / 2;
    gravity = (-2 * maxJumpHeight) / MathF.Pow(timeToApex, 2);
    intialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
      
}

// call the dash function, since this can be called almost on all the states.
public void OnDash(){
    if(coolDownTimeDash <= 0f){
       SwitchState(new PlayerDashState(this,this.currentMovement));
    }
    
}

}
