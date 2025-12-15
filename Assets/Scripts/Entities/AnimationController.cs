using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private MovementBase movement;
    private HpManager hpManager;
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");

    public void Initialize(Animator myAnim, MovementBase myMove)
    {
        movement = myMove;
        anim = myAnim;
        hpManager = GetComponent<HpManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isAlive = hpManager ? !hpManager.IsDead : true;
        anim.enabled = isAlive;
        bool isMoving = movement.IsMoving();
        anim.SetBool(IsMoving, isMoving);
    }
}
