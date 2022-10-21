using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectTargetState : PlayerTargetState, IPlayerSelectTargetInterface
{
    
    private List<Transform> enemyList = new List<Transform>();

    private Vector3 height;
    private Vector3 center;
    
    
    public PlayerSelectTargetState(PlayerStateMachine stateMachine, bool shouldChangeTarget) : base(stateMachine, shouldChangeTarget)
    {
        Debug.Log("Selection of new target with inputs is being call");
        this.height = new Vector3(stateMachine.transform.position.x, stateMachine.transform.position.y + 
        stateMachine.Controller.height, stateMachine.transform.position.z);
        this.center = new Vector3(stateMachine.transform.position.x, stateMachine.transform.position.y + 
        (stateMachine.Controller.height/2), stateMachine.transform.position.z);
        this.enemyList = stateMachine.Targeters.currentEnemiesList;
         
        //call the method on the constructor
       
    }
     
     
     
     
     public override void Tick(float deltaTime){
        selectNewTargetInput();
        newUpdateAnimator(deltaTime);
        RotateToTarget();

       
        if(!stateMachine.InputReader.isTargeting){
       stateMachine.SwitchState(new PlayerTargetState(stateMachine,false));
       
      }
     }





     public void selectNewTargetInput()
    {
       //get the inputs
      
      Vector3 newCurrentInputs = currentInputs().normalized;
     
      
      for(int i = 0; i < stateMachine.Targeters.currentEnemiesList.Count;i++){
           Transform targets = stateMachine.Targeters.currentEnemiesList[i];
           float dist = Vector3.Distance(stateMachine.transform.position,targets.position);
           setNewTarget(newCurrentInputs,targets, dist);
      }
    }

  

   

    public void setNewTarget(Vector3 motion,Transform tar, float dist)
    {
       Ray [] currentRay= CreateRay(motion); 
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


      public Ray[] CreateRay(Vector3 inputs)
    {
      Ray [] ray = new Ray[2];
      ray[0] = new Ray(height,inputs);
      ray[1] = new Ray(center,inputs);
      return ray;
    }

    public Vector3 currentInputs()
    {
        Vector3 camera_z = stateMachine.MainCameraPlayer.forward;
       Vector3 camera_x = stateMachine.MainCameraPlayer.right;
       camera_z.y = 0f;
       camera_x.y = 0f;
       camera_z.Normalize();
       camera_x.Normalize();
       return stateMachine.InputReader.MovementValue.x * camera_x + 
       stateMachine.InputReader.MovementValue.y * camera_z;
    }


      private void newUpdateAnimator(float deltaTime){
      
        
        stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        
       
        
        
        stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        
      
    }

}
