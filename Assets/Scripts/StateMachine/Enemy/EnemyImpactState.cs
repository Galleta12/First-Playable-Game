using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    
    
    //animator variables
    
    private readonly int ImpactHash = Animator.StringToHash("BruteImpact");
    private const float CrossFadeDuration = 0.1f;
    
    
    //--------------------------------------------
    private Vector3 currentKnockback;
    private Vector3 impact;

    // damping velocity for the smoothdamp method
    private Vector3 dampingVelocity;
    // how long will take the impact state
    private float duration = 1f;

    public EnemyImpactState(EnemyStateMachine enemystateMachine, Vector3 directionknockback) : base(enemystateMachine)
    {
        //this is the direction for the knockback
        this.currentKnockback = directionknockback;
    }
    
    public override void Enter()
    {
        
        
        //enemystateMachine.Animator.CrossFadeInFixedTime(ImpactHash,CrossFadeDuration);
        //we set the vector for the force impact
        AddForceImpact(currentKnockback);


    }

    public override void Tick(float deltaTime)
    {
        MoveImpact(deltaTime);

       duration -= deltaTime;
       if(duration <= 0f){
        enemystateMachine.SwitchState(new EnemyIdleState(enemystateMachine));
       }
        // need to know more about this, the dampingvelocity is only for this code, but we ensure to smootly go back to zero
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity,enemystateMachine.drag);
    }


    public override void Exit()
    {
        
    }

    public override void IntiliazeSubState()
    {
        
    }


    private void AddForceImpact(Vector3 force){
        impact += force;
    }

    private void MoveImpact(float deltatime){
        enemystateMachine.Controller.Move((impact + enemystateMachine.Movement) * deltatime);

    }

}
