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
protected float GetNormalizedTime(Animator animator){

  AnimatorStateInfo currentAnimator = animator.GetCurrentAnimatorStateInfo(0);
  AnimatorStateInfo nextAnimator = animator.GetNextAnimatorStateInfo(0);


    if(animator.IsInTransition(0) && nextAnimator.IsTag("Attack")){
            return nextAnimator.normalizedTime;
        }else if(!animator.IsInTransition(0) && currentAnimator.IsTag("Attack")){
            return currentAnimator.normalizedTime;
        }else{
            return 0f;
        }
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