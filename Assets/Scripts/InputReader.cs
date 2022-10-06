using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputReader : MonoBehaviour,Controls.IPlayerActions
{


    private Controls controls;

     public Vector2 MovementValue {get; private set;}


      public bool  isJumping {get; private set;}


       public event Action JumpEvent;


        public event Action DashEvent;




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
}
