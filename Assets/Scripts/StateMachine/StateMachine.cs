using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    
    // current state of the state machine
    
    
   
    private State currentState;

     
     

     
    
    public void SwitchState(State newState){
      
      currentState?.Exit();
      currentState?.currentSubState?.Exit();
      
      newState?.Enter();
      
       if(newState.isRootState){
            currentState = newState;
          
          
            
       }else if(newState.currentSuperState !=null ){
                Debug.Log("not root");
                currentState.currentSuperState.SetSubState(newState);
       }
       
      
      //currentState?.Enter();

      
    }


    
    // Update is called once per frame
    private void Update()
    {
         CustomUpdate(Time.deltaTime);
        currentState?.Tick(Time.deltaTime);
        currentState?.currentSubState?.Tick(Time.deltaTime);
       
       
    }

    public virtual void CustomUpdate(float deltaTime){
      
    }
  
}
