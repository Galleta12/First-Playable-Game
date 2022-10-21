using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerSelectTargetInterface
{
   
   void selectNewTargetInput();
   void setNewTarget(Vector3 motion,Transform tar, float dist);
   Ray[] CreateRay(Vector3 inputs);

   Vector3 currentInputs();
}
