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


 


// rotate the chracter
protected void FaceLookMouse( Vector3 direction,float deltaTime){
    if(direction== Vector3.zero){return;}
    stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
    Quaternion.LookRotation(direction),
    deltaTime * stateMachine.RotationDampSpeed
    );

}

  protected void RotateToTarget(){
       if(stateMachine.Targeters.currentTarget == null){return;}
       // direction to where is the target
       Vector3 lookTarget = stateMachine.Targeters.currentTarget.transform.position - stateMachine.transform.position;
       lookTarget.y = 0f;
       //rotate towards the current target;
       stateMachine.transform.rotation = Quaternion.LookRotation(lookTarget);
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
  
 // we are passing the number int
 // we also check if the button is a keyboard or a controller
 //since it will be handle it different depending if it is a controller or not
 if(stateMachine.InputReader.DeviceName == "Keyboard"){
    stateMachine.SwitchState(new PlayerDrawWeapons(stateMachine, stateMachine.InputReader.KeyBoardNumberInt));
 }
 else{
  stateMachine.SwitchState(new PlayerDrawControllerWeaponState(stateMachine));
 }
 
  
 
}


protected bool GetStateOfAnimation(Animator animator, string animationName){
   AnimatorStateInfo currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
    if(currentAnimation.normalizedTime > 1.0f && currentAnimation.IsTag(animationName)){
      
      //Debug.Log("This is true");
     return false;
    }else{
        //Debug.Log("This is false");
       return true;
      
      
    }
}





}