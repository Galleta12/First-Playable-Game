using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
     // animators variable
     private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetBlendTree"); 

    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;

    
    public PlayerTargetState(PlayerStateMachine stateMachine, bool shouldChangeTarget) : base(stateMachine)
    {
       // call the set target to select the closest target
       // everytime that we change of state this will be called
       if(shouldChangeTarget){
         stateMachine.Targeters.setTheClosestTarget();
       }
       else if(stateMachine.Targeters.currentTarget == null){
         stateMachine.Targeters.setTheClosestTarget();
       }
      
        
    }

    public override void Enter()
    {
        // we want to hande our moves
        stateMachine.moveDelegate -=  stateMachine.Move;
        
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);

        stateMachine.InputReader.DodgeEvent += OnDodge;
        // press the target button to exit the target mode
        stateMachine.InputReader.CancelTargetEvent+= OnExitTarget;

        stateMachine.InputReader.TargetEvent+= ChangeTarget;
       
       
         stateMachine.InputReader.JumpEvent += OnJump;
         stateMachine.InputReader.DrawEvent += OnDraw;

         stateMachine.InputReader.RollEvent += OnRollTarget;
         
        
    }


      public override void Tick(float deltaTime)
    {
        
        // get the target movement
        Vector3 targetMovement = CalculateTargetMovement();
        // move and update the player, with the new movements for get a correct targeting mode
        NewMoveTarget(targetMovement * stateMachine.TargetMovementSpeed,deltaTime);
        UpdateAnimator(deltaTime);
        
      
      checkpossibleChangeofState();
      
      if(stateMachine.InputReader.isAttacking && stateMachine.Weapon.selectedWeapon.WeaponObject != null){
     
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
        return;
        
      }
     
      if(stateMachine.Targeters.currentTarget == null){
         stateMachine.Targeters.setTheClosestTarget();
         //here I need to do a courotine in order to check if the targets is null;
      }
      
     
      

      
      // we check if there are enemies inside the list of the sphere overlap
      //if there is not enemies we can get back to ground state
     
        // rotote so the player if always facing the current target;
        RotateToTarget();
   
      
    }



    public override void Exit()
    {
        
         stateMachine.moveDelegate =  stateMachine.Move;
         // if we exit this means that the last state was target state
        stateMachine.IsTargeting = true;
        
        stateMachine.InputReader.CancelTargetEvent-= OnExitTarget;
        
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        
        stateMachine.InputReader.JumpEvent -= OnJump;

        stateMachine.InputReader.DrawEvent -= OnDraw;

        stateMachine.InputReader.RollEvent -= OnRollTarget;

         stateMachine.InputReader.TargetEvent-= ChangeTarget;
    }

  

    public override void IntiliazeSubState()
    {
       
    }


  

    private Vector3 CalculateTargetMovement(){
       Vector3 movement = new Vector3();
       movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
       movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

       return movement;

    }





    private void NewMoveTarget(Vector3 motion, float deltaTime){
       stateMachine.Controller.Move((stateMachine.Movement + motion) * deltaTime);
    }


  

    private void UpdateAnimator(float deltaTime){
       if (stateMachine.InputReader.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            // if the input y is grater than 0 we set it up as 1f other wise -1f
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }
    }


       private void OnDodge(){
        
         stateMachine.SwitchState(new PlayerDodgeState(stateMachine,stateMachine.InputReader.MovementValue));
        
    }


    private void OnExitTarget()
    {
       //press tab again to get out from the ground state;
       //this will already set up target as false, therefore we don't get unexpected behaviours.
       stateMachine.SwitchState(new PlayerGroundState(stateMachine));
    }


      private void OnRollTarget()
    {
       if(stateMachine.coolDownTimeRoll <=0f){
          stateMachine.SwitchState(new PlayerRollTargetState(stateMachine, stateMachine.InputReader.MovementValue));
        }
    }


           private void OnJump(){
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }


    private void ChangeTarget(){
      stateMachine.SwitchState(new PlayerTargetState(stateMachine,true));
    }
      private void checkpossibleChangeofState(){
     
      if(stateMachine.InputReader.isTargeting){
       selectNewTargetInput();
      }
    }


    private void selectNewTargetInput(){
      
      //get the inputs
      // get the height and center of the player
      Vector3 height= new Vector3(stateMachine.transform.position.x,stateMachine.transform.position.y + 
      stateMachine.Controller.height,stateMachine.transform.position.z);
      Vector3 center= new Vector3(stateMachine.transform.position.x,stateMachine.transform.position.y + 
      (stateMachine.Controller.height/2),stateMachine.transform.position.z);
      Vector3 newCurrentInputs = currentInputs().normalized;
      //the position to pass as an argument
      Vector3 [] position = new Vector3[2];
      position[0]=height;
      position[1]=center;
      
      for(int i = 0; i < stateMachine.Targeters.currentEnemiesList.Count;i++){
           Transform targets = stateMachine.Targeters.currentEnemiesList[i];
           float dist = Vector3.Distance(stateMachine.transform.position,targets.position);
           setTheNewTarget(newCurrentInputs,targets,dist,position);
      }
    }


    private void setTheNewTarget(Vector3 motion,Transform tar, float dist,Vector3[] positions){
      
      Debug.Log("This is being call");
     
      Ray [] currentRay= CreateRay(motion,positions[0],positions[1]);
      // inputs relative to the heigh position
      // we get to vectors direction
      Vector3 vector1 = currentRay[0].direction;
      Vector3 vector2= tar.position - currentRay[0].origin;
      // the same for the center
      Vector3 vector3 = currentRay[1].direction;
      Vector3 vector4= tar.position - currentRay[1].origin;

      // get the dot product with the normals
      // the values are between 1 and -1
      float lookPercentageHeight = Vector3.Dot(vector1.normalized,vector2.normalized);
      float lookPercentageCenter = Vector3.Dot(vector3.normalized,vector4.normalized);
       //check if the look percentage is greate, if it is we can set up the target.
       if(lookPercentageHeight > stateMachine.Targeters.threshold || 
       lookPercentageCenter > stateMachine.Targeters.threshold){
           //therefore if this condition is true we can set up the new target
           stateMachine.Targeters.setNewTarget(tar);
       }
    }

    //create an ray relative to the inputs of the player
    private Ray[] CreateRay(Vector3 inputs, Vector3 height, Vector3 center){
      Ray [] ray = new Ray[2];
      ray[0] = new Ray(height,inputs);
      ray[1] = new Ray(center,inputs);
      return ray;

    }



    private Vector3 currentInputs(){
       Vector3 camera_z = stateMachine.MainCameraPlayer.forward;
       Vector3 camera_x = stateMachine.MainCameraPlayer.right;
       camera_z.y = 0f;
       camera_x.y = 0f;
       camera_z.Normalize();
       camera_x.Normalize();
       return stateMachine.InputReader.MovementValue.x * camera_x + 
       stateMachine.InputReader.MovementValue.y * camera_z;
    }
    


  
}


