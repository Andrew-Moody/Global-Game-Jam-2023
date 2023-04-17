using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorState : StateMachineBehaviour
{
    public string StateName;

	public event Action<string> StateExitEvent;

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		StateExitEvent?.Invoke(StateName);
	}
}
