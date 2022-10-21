using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputReader : MonoBehaviour,Controls.IPlayerActions
{


    private Controls controls;
    
    public Vector2 MovementValue {get; private set;}

    private  String KeyBoardNumber ;

    public int KeyBoardNumberInt => Convert.ToInt32(KeyBoardNumber);
   
   public String DeviceName {get; private set;}

    public bool  isJumping {get; private set;}

    public bool  isAttacking {get; private set;}


    public bool  isTargeting {get; private set;}


    public event Action JumpEvent;


    public event Action DashEvent;
     public event Action RollEvent;

    public event Action DrawEvent;

    public event Action TargetEvent;

    public event Action DodgeEvent;

    public event Action CancelTargetEvent;




       private void Start()
    {
        // store instance of class controls
        controls = new Controls();
        // reference to this class
        controls.Player.SetCallbacks(this);
        // enable it
        controls.Player.Enable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
      //isJumping = context.ReadValueAsButton();
        if(!context.performed) { return;}
        
        JumpEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
       
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        DashEvent?.Invoke();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
       if(!context.performed){return;}
       RollEvent?.Invoke();
    }

    public void OnDrawWeapon(InputAction.CallbackContext context)
    {
       if(!context.performed){return;}
       
       KeyBoardNumber =context.control.name;
       DeviceName =context.control.device.name;
       DrawEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed){
            isAttacking = true;
        }else if(!context.performed){
            isAttacking =false;
        }
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
       if(!context.performed){return;}
         Debug.Log("This is for the target event trigger");
       TargetEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        DodgeEvent?.Invoke();
    }

    public void OnOnCancelTarget(InputAction.CallbackContext context)
    {
       if(!context.performed){return;}
       CancelTargetEvent?.Invoke();
    }

    public void OnTargetSelectorPlayer(InputAction.CallbackContext context)
    {
        
         if(context.performed){
            Debug.Log("This is for the target");
            isTargeting = true;
        }else if(!context.performed){
            isTargeting =false;
             Debug.Log("This is for the target false");
        }
    }
}
