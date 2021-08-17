using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTurret : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    public GameObject turretOnPlace = null;

    public void SetState(bool state)
    {
        animator.SetBool("isOpen", state);
    }
}
