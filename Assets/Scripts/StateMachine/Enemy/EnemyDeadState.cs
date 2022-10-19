using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine EnemystateMachine) : base(EnemystateMachine)
    {
    }

    public override void Enter()
    {
      
    }


      public override void Tick(float deltaTime)
    {
        
        //if we are dead we want to destroy the target component
        // for now we will destroy the whole object
        GameObject.Destroy(enemystateMachine.gameObject);

    }

    public override void Exit()
    {
        
    }

    public override void IntiliazeSubState()
    {
        
    }

  
}
