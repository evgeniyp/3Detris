using UnityEngine;

public class Group : MonoBehaviour
{
    private const float GAME_TICK_SECONDS = 1f;
    private const float LERP_SPEED = 0.1f;

    private Quaternion _targetRot;

    private float _lastFall = 0;

    private GameObject _real;

    void Start()
    {
        _real = new MirrorGameObject(gameObject).Me;
    }

    void Update()
    {
        if (Time.time - _lastFall >= GAME_TICK_SECONDS)
        {
            _real.transform.position += new Vector3(0, -1, 0);
            _lastFall = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _real.transform.rotation *= Quaternion.Euler(Vector3.forward * 90);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _real.transform.rotation *= Quaternion.Euler(Vector3.forward * -90);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _real.transform.rotation *= Quaternion.Euler(Vector3.right * -90);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _real.transform.rotation *= Quaternion.Euler(Vector3.right * 90);
        }

        transform.position = Vector3.Lerp(transform.position, _real.transform.position, LERP_SPEED);
        transform.rotation = Quaternion.Lerp(transform.rotation, _real.transform.rotation, LERP_SPEED);
    }
}
