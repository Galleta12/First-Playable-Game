using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawWeapons : PlayerBaseState
{
    
     private readonly int DrawSwordHash = Animator.StringToHash("DrawSword");
     private const float CrossFadeDuration = 0.1f;

    private GameObject hand;

    private GameObject sword;
    
    public PlayerDrawWeapons(PlayerStateMachine stateMachine) : base(stateMachine)
    {
          isRootState = true;  
    }

    public override void Enter()
    {
   
       stateMachine.Animator.CrossFadeInFixedTime(DrawSwordHash ,CrossFadeDuration);
       hand = GameObject.Find("RightHand");
       sword = GameObject.Find("MagicSword_Ice");
       sword.transform.SetParent(hand.transform,false);
       sword.transform.position = hand.transform.position;
      
      
    }


      public override void Tick(float deltaTime)
    {
        Debug.Log( hand);
        Debug.Log(sword);
    }

    public override void Exit()
    {
       
    }

    public override void IntiliazeSubState()
    {
      
    }

  
}
