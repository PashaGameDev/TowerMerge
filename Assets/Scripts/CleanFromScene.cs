using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanFromScene : MonoBehaviour
{
    [SerializeField] float cleanTime = 0f;
    void Start()
    {
        Destroy(this.gameObject, cleanTime);
    }

}
