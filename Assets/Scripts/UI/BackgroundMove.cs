using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.3f;

    private Material _mat;

    private void Start()
    {
        _mat = GetComponent<Image>().material;
    }
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * speed, Time.time * -speed);
        _mat.mainTextureOffset = offset;
    }
}
