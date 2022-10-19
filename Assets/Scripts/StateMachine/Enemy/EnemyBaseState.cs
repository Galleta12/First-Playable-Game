using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{

    protected EnemyStateMachine enemystateMachine;



    public EnemyBaseState(EnemyStateMachine  enemystateMachine){
        this.enemystateMachine= enemystateMachine;
      
    }





    
}
