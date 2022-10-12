using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// abstarct class we cannot creat an instance of this class logic to inherit
public abstract class State
{
  
  public State currentSubState;
  public State currentSuperState;

  private bool IsRoot = false;
  public bool isRootState{get{ return IsRoot; } set {IsRoot= value;}}
    

  public abstract void Enter();
  public abstract void Tick(float deltaTime);

  public abstract void Exit();

  public abstract void IntiliazeSubState();
// function locally defined
// check how far from an animation we are
protected float GetNormalizedTime(Animator animator){

  //animator annonying you can be on multiple states
  //this will help us to check on which animator we are
  // if we are blending on two we can check on which we are currently int

  AnimatorStateInfo currentAnimator = animator.GetCurrentAnimatorStateInfo(0);
  AnimatorStateInfo nextAnimator = animator.GetNextAnimatorStateInfo(0);

   // if the animator is on trasition and the tag is Attack
   //this means we are not on trasition, therefore we are on our current state
    if(animator.IsInTransition(0) && nextAnimator.IsTag("Attack")){
            return nextAnimator.normalizedTime;
        }else if(!animator.IsInTransition(0) && currentAnimator.IsTag("Attack")){
            return currentAnimator.normalizedTime;
        }else{
            return 0f;
        }

        // if the normalize time is greater than 1, it means that we havent done anything the animation has finished
        
}



public void SetSuperState(State newSuperState){
 this.currentSuperState = newSuperState;
}

public void SetSubState(State newSubState){
 this.currentSubState = newSubState;
  this.currentSubState.SetSuperState(this);
  this.currentSubState.Enter();
 //Debug.Log(this.currentSubState);
}





}