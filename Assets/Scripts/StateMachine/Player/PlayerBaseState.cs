using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// abstarct class we cannot creat an instance of this class logic to inherit
public abstract class PlayerBaseState : State
{

    
   
    protected PlayerStateMachine stateMachine;

   
    // protected PlayerBaseState currentSubState;
    // protected PlayerBaseState currentSuperState;

  



    public PlayerBaseState(PlayerStateMachine stateMachine){
        this.stateMachine = stateMachine;
      
    }



// public void UpdateStates(){
//    Tick(Time.deltaTime);
//    if(currentSubState != null){
//     currentSubState.UpdateStates();
//    }

// }


// protected void SwitchState(PlayerBaseState newState){
//    Exit();
//    newState.Enter();
//    if(isRootState){
//      CurrentState.currentstate = newState;
//    } else if(currentSuperState !=null){
//     currentSuperState.SetSubState(newState);
//    }
  
   
// }

// protected void SetSuperState(PlayerBaseState newSuperState){
//  currentSuperState = newSuperState;
// }

// protected void SetSubState(PlayerBaseState newSubState){
//  currentSubState = newSubState;
//  newSubState.SetSuperState(this);
// }

// calculate the movement regarding the mouse and the inputs
protected Vector3 CalculateNormalMovement(){

    Vector3 camera_z = stateMachine.MainCameraPlayer.forward;
    Vector3 camera_x = stateMachine.MainCameraPlayer.right;
    camera_z.y = 0f;
    camera_x.y = 0f;
    camera_z.Normalize();
    camera_x.Normalize();
    
    return stateMachine.InputReader.MovementValue.x * camera_x 
    + stateMachine.InputReader.MovementValue.y * camera_z;

}





// protected void Move(Vector3 motion,float deltaTime){
//   stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);


// }

// rotate the chracter
protected void FaceLookMouse( Vector3 direction,float deltaTime){
    if(direction== Vector3.zero){return;}
    stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
    Quaternion.LookRotation(direction),
    deltaTime * stateMachine.RotationDampSpeed
    );

}

//this the the method saved on the delegate setcooldown
protected void handleCoolDownDash(float deltaTime){
  
  //Debug.Log("The dash delegate is being call");
  // we reduce the time of the cooldown time frame by frame
  stateMachine.coolDownTimeDash -= deltaTime;
  // if the cooldowntime is 0 or less we unsubscribe from the delegate
  if(stateMachine.coolDownTimeDash <= 0f){
    // the delegate is only being call if it exists and it will only exist if we exit the roll or dash state
    stateMachine.setCoolDown -= handleCoolDownDash;
  }

}


protected void handleCoolDownRoll(float deltaTime){
  
  
  //Debug.Log("The roll delegate is being call");
  // we reduce the time of the cooldown time frame by frame
  stateMachine.coolDownTimeRoll -= deltaTime;
  // if the cooldowntime is 0 or less we unsubscribe from the delegate
  if(stateMachine.coolDownTimeRoll <= 0f){
    // the delegate is only being call if it exists and it will only exist if we exit the roll or dash state
    stateMachine.setCoolDown -= handleCoolDownRoll;
  }

}


protected void OnDraw(){
  
  
  stateMachine.SwitchState(new PlayerDrawWeapons(stateMachine, stateMachine.InputReader.KeyBoardNumberInt, stateMachine.InputReader.DeviceName));
}








}