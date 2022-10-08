using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawWeapons : PlayerBaseState
{
    
     private readonly int DrawSwordHash = Animator.StringToHash("DrawSword");
     private const float CrossFadeDuration = 0.1f;

    private GameObject hand;

    private GameObject sword;

    private int KeyboardNumber;

     private string DeviceName;

     private WeaponsData currentWeapon;
    
    public PlayerDrawWeapons(PlayerStateMachine stateMachine, int keyboardNumber, string deviceName) : base(stateMachine)
    {
          isRootState = true;
          this.KeyboardNumber = keyboardNumber;
          this.DeviceName = deviceName;
          this.currentWeapon = stateMachine.Weapon.getTypeWeapon(keyboardNumber); 
    }

    public override void Enter()
    {
   
       stateMachine.Animator.CrossFadeInFixedTime(DrawSwordHash ,CrossFadeDuration);

      //  hand = GameObject.Find("RightHand");
      //  sword = GameObject.Find("MagicSword_Ice");
      //  sword.transform.SetParent(hand.transform,false);
      //  sword.transform.position = hand.transform.position;
      
      //this is just for print the data and check that everything  is ok
      // foreach(KeyValuePair<GameObject,WeaponsData> kvp in stateMachine.Weapon.WeapondsDataHash){
            
      //       Debug.Log("Key: " + kvp.Key + "," + "Value: " + kvp.Value.WeaponObject);
      // }
      
    }


      public override void Tick(float deltaTime)
    {
        //Debug.Log( stateMachine.Weapon.WeaponsDatas[0].WeaponObject);
       
    }

    public override void Exit()
    {
       
    }

    public override void IntiliazeSubState()
    {
      
    }

  
}
