using UnityEngine;

public class Group : MonoBehaviour
{
    private const float GAME_TICK_SECONDS = 0.5f;
    private const float LERP_SPEED = 0.2f;

    private bool _drivenByPlayer = true;

    private float _lastFall = 0;

    private GroupReal _real;

    private void Start()
    {
        var gameObject = new GameObject();
        gameObject.transform.position = transform.position;
        gameObject.transform.rotation = transform.rotation;
        gameObject.AddComponent<GroupReal>();
        _real = gameObject.GetComponent<GroupReal>();
        _real.DisplayedGroup = this;

        foreach (Transform child in transform)
        {
            var childGameObject = new GameObject();
            childGameObject.transform.position = child.position;
            childGameObject.transform.rotation = child.rotation;
            childGameObject.transform.SetParent(_real.transform);
            childGameObject.AddComponent<CubeReal>();
            var _realChild = childGameObject.GetComponent<CubeReal>();
            _realChild.DisplayedCube = child.GetComponent<Cube>();
        }

        if (!IsValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

    bool IsValidGridPos()
    {
        foreach (Transform child in _real.transform)
        {
            Vector3 v = Grid.RoundVec3(child.position);

            if (!Grid.InsideBorder(v))
                return false;

            if (Grid.grid[(int)v.x, (int)v.z, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.z, (int)v.y].transform.parent != _real.transform)
                return false;
        }
        return true;
    }

    void Update()
    {
        if (!_drivenByPlayer)
        {

        }
        else
        {
            if (Time.time - _lastFall >= GAME_TICK_SECONDS)
            {
                _real.transform.position += new Vector3(0, -1, 0);
                if (IsValidGridPos())
                {
                    UpdateGrid();
                }
                else
                {
                    _real.transform.position += new Vector3(0, 1, 0);
                    FindObjectOfType<Spawner>().SpawnNext();
                    _drivenByPlayer = false;
                }
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
        }

        transform.position = Vector3.Lerp(transform.position, _real.transform.position, LERP_SPEED);
        transform.rotation = Quaternion.Lerp(transform.rotation, _real.transform.rotation, LERP_SPEED);
    }

    private void UpdateGrid()
    {
        for (int x = 0; x < Grid.length; x++)
            for (int y = 0; y < Grid.height; y++)
                for (int z = 0; z < Grid.width; z++)
                    if (Grid.grid[x, z, y] != null && Grid.grid[x, z, y].transform.parent == _real.transform)
                        Grid.grid[x, z, y] = null;

        foreach (Transform child in _real.transform)
        {
            Vector3 v = Grid.RoundVec3(child.position);
            Grid.grid[(int)v.x, (int)v.z, (int)v.y] = child.gameObject.GetComponent<CubeReal>();
        }
    }
}
