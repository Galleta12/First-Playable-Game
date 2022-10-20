using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    
     // this is the roll state is similar to the dash with slightly difference
    // the first difference is that roll only occur on the ground state

    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree"); 

    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    
    private const float CrossFadeDuration = 0.1f;

    private Vector3 DodgeInput;

    private float remainingDodgeTime;
    
    
    
    
    
    public PlayerDodgeState(PlayerStateMachine stateMachine, Vector3 currentMotion) : base(stateMachine)
    {
       
        if(currentMotion!= Vector3.zero){
           this.DodgeInput = currentMotion;
        }
       
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeTime;
    
        // we are going to hanlde the move we dont want to be subscribe on the main move delgate
        stateMachine.moveDelegate -= stateMachine.Move;
        // for the animator
        stateMachine.Animator.SetFloat(DodgeForwardHash,DodgeInput.y);
        stateMachine.Animator.SetFloat(DodgeRightHash,DodgeInput.x);
         stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);
        
    }

    
     public override void Tick(float deltaTime)
    {
        
        if(DodgeInput !=Vector3.zero){
             // get the movement regarding the current motion passed on the constructor
            Vector3 dodgemove = new Vector3(); 
            dodgemove+= stateMachine.transform.right * DodgeInput.x * stateMachine.DodgeForce/stateMachine.DodgeTime;
            dodgemove+= stateMachine.transform.forward * DodgeInput.y * stateMachine.DodgeForce/stateMachine.DodgeTime;
            NewMoveTargetDodge( dodgemove,deltaTime);
        }else{
            Vector3 dodgeMousedirection = new Vector3();
            dodgeMousedirection += stateMachine.transform.right * getMouseDirection().x * stateMachine.DodgeForce/stateMachine.DodgeTime;
             dodgeMousedirection += stateMachine.transform.forward * getMouseDirection().z * stateMachine.DodgeForce/stateMachine.DodgeTime;
             NewMoveTargetDodge(dodgeMousedirection,deltaTime);
        }
        
        
        RotateToTarget();
        
        remainingDodgeTime -=deltaTime;
       
          
        if ( remainingDodgeTime <= 0f)
        {
           
              stateMachine.SwitchState(new PlayerTargetState(stateMachine,false));
        } 
    }
    
    public override void Exit()
    {
       
       //subscribe back to the move delegate
       stateMachine.moveDelegate = stateMachine.Move;
    }

    public override void IntiliazeSubState()
    {
       
    }

    // if if not moving we want to move depending on the mouse direction
    private Vector3 getMouseDirection()
    {
        Vector3 camera_z = stateMachine.MainCameraPlayer.forward;
      
        camera_z.y =0f;
        
        camera_z.Normalize();
       
        
        
        return camera_z;

       
    }


   

     // this will handle the target move
      private void NewMoveTargetDodge(Vector3 motion, float deltaTime){
       stateMachine.Controller.Move((stateMachine.Movement + motion) * deltaTime);
    }

    

   
}
