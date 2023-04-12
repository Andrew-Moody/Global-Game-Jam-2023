using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntTest : MonoBehaviour
{

    public Animator Animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            Animator.SetBool("IsMoving", true);
        }
		else
		{
            Animator.SetBool("IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.J))
		{
            Animator.SetTrigger("DeathTrigger");
		}

        if (Input.GetKeyDown(KeyCode.K))
        {
            Animator.SetTrigger("AttackTrigger");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Animator.SetTrigger("DamageTrigger");
        }
    }
}
