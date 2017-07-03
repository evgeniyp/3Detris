using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    private const float LERP_SPEED = 0.2f;

    private Vector3 _targetPos;
    private Quaternion _targetRot;

    private float _lastFall = 0;

    void Start()
    {
        _targetPos = transform.position;
    }

    void Update()
    {
        if (Time.time - _lastFall >= 5)
        {
            _targetPos += new Vector3(0, -1, 0);
            _lastFall = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _targetRot.eulerAngles += new Vector3(0, -90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _targetRot.eulerAngles += new Vector3(0, +90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _targetRot.eulerAngles += new Vector3(+90, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _targetRot.eulerAngles += new Vector3(-90, 0, 0);
        }

        transform.position = Vector3.Lerp(transform.position, _targetPos, LERP_SPEED);
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, LERP_SPEED);
    }
}
