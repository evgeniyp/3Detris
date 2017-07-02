using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    private const float LERP_SPEED = 0.2f;
    private Vector3 _targetPos;
    private float _lastFall = 0;

    void Start()
    {
        _targetPos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - _lastFall >= 1)
        {
            _targetPos += new Vector3(0, -1, 0);
            _lastFall = Time.time;
        }

        transform.position = Vector3.Lerp(transform.position, _targetPos, LERP_SPEED);
    }
}
