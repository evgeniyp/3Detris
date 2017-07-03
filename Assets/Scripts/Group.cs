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
        _targetRot = transform.rotation;
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
            _targetRot *= Quaternion.Euler(Vector3.left * 90);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _targetRot *= Quaternion.Euler(Vector3.left * -90);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _targetRot *= Quaternion.Euler(Vector3.forward * -90);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _targetRot *= Quaternion.Euler(Vector3.forward * 90);
        }

        transform.position = Vector3.Lerp(transform.position, _targetPos, LERP_SPEED);
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, LERP_SPEED);
    }
}
