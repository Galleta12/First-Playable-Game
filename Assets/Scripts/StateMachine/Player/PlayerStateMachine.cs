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

[field: SerializeField] public float FreeLookMovementSpeed {get; private set; }

[field: SerializeField] public float JumpMoveSpeed {get; private set; }


[field: SerializeField] public float RotationDampSpeed {get; private set; }


[field: SerializeField] public float DashForce {get; private set; }


[field: SerializeField] public float DashStationaryForce {get; private set; }


[field: SerializeField] public float DashTime {get; private set; }

[field: SerializeField] public float DashCoolDown {get; private set; }



public Transform MainCameraPlayer {get; private set; }



[HideInInspector]
public Vector3 currentMovement;
public float verticalVelocity;
public float gravity {get; private set; } = -9.8f;
public float intialJumpVelocity {get; private set; }

private float groundGravity = -.05f;

private float maxJumpHeight = 1.2f;

private float maxJumpTime = 0.7f;

public Vector3 Movement =>  (Vector3.up * verticalVelocity);


public delegate void MoveDelegate(Vector3 motion, float deltaTime);

public MoveDelegate moveDelegate;


private void Start() {
    
     moveDelegate = Move;
     
     setupJumpVariable();
    
    MainCameraPlayer = Camera.main.transform;

     InputReader.DashEvent += OnDash;
   
    SwitchState(new PlayerGroundState(this));
     
}


public override void CustomUpdate(float deltaTime){
  
  
  moveDelegate?.Invoke(currentMovement,deltaTime);
  handleGravity(deltaTime);
  
}

private void handleGravity(float deltaTime)
{
      if(verticalVelocity<0f && Controller.isGrounded){
          verticalVelocity = groundGravity * deltaTime;
           
         
      }else{
         verticalVelocity += gravity * deltaTime;
     
      }
}

public void Move(Vector3 motion, float deltaTime){
 
      Debug.Log("This is the normal move");
      Controller.Move((motion + Movement) * deltaTime);
 
}




private void setupJumpVariable()
{
    float timeToApex =maxJumpTime / 2;
    gravity = (-2 * maxJumpHeight) / MathF.Pow(timeToApex, 2);
    intialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
      
}


public void OnDash(){
    SwitchState(new PlayerDashState(this,this.currentMovement));
}






}
