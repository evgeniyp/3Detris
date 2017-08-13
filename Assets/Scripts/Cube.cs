using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public CubeReal Real;
    public Group DisplayedGroup;
    private Renderer _renderer;
    private float _fadeStart;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Sometimes cubes are moved separately from group when planes are removed
        if (!DisplayedGroup.DrivenByPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, Real.transform.position, Group.LERP_SPEED);
        }

        var now = Time.time;
        if (_fadeStart > 0)
        {
            if (now - _fadeStart > .5)
            {
                Destroy(Real);
                Destroy(gameObject);
            }
            else
            {
                var color = _renderer.material.color;
                color.a = (.5f - (now - _fadeStart)) / .5f;
                Debug.Log(color.a);
                _renderer.material.color = color;
            }
        }
    }

    public void FadeToDeath()
    {
        _fadeStart = Time.time;
    }
}
