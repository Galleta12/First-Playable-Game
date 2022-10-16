using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetState : PlayerBaseState
{
    public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        isRootState = true;
    }

    public override void Enter()
    {
       stateMachine.moveDelegate -=  stateMachine.Move;
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
