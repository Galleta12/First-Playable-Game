using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    
     // animator for the enemy
     [field: SerializeField] public Animator Animator {get; private set; }
     
     // character controller
    [field: SerializeField] public CharacterController Controller {get; private set; }
    
    //Health
    [field: SerializeField] public Health Health {get; private set; }

    // Target

    [field: SerializeField] public Target Target {get; private set; }

    [field: SerializeField] public float drag {get; private set; }
    


    public Health PlayerHealth {get;private set;}

    private void Start(){
      
      //get the health component of the player
      PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
     // we start on the idle state
     SwitchState(new EnemyIdleState(this));
    
    }
     // we want to enable to the event on death as soon as this component is enable 
     // an we unsubcribe from on disable
     // we also enable on take damage
    private void OnEnable() {
        Health.OnDeath +=HandleDeath;
        Health.OnTakeDamage += HandleImpact;
    }

    private void OnDisable() {
       
        Health.OnDeath -=HandleDeath;
         Health.OnTakeDamage -= HandleImpact;
    }

    // it will change to the death state everytime the OnDeath event is trigger
    private void HandleDeath(){
         SwitchState(new EnemyDeadState(this));
    }


     private void HandleImpact(Vector3 directionknockback){
         SwitchState(new EnemyImpactState(this,directionknockback));
    }








}
