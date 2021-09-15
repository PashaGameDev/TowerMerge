using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinusHPCanvasAnimation : MonoBehaviour
{

    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) { return; }
        animator.SetInteger("AnimIndex", Random.Range(1,4));
    }

    
}
