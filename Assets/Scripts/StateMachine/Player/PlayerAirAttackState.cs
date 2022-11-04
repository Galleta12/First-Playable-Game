using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAirAttackState : PlayerBaseState
{
    
    
    
    private readonly int AirAttackHash = Animator.StringToHash("AttackJump"); 
    
    private const float CrossFadeDuration = 0.1f;

    
    
    public PlayerAirAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }


    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AirAttackHash,CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
    }
    public override void Exit()
    {
        
    }

    public override void IntiliazeSubState()
    {
        
    }

}
