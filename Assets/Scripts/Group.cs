using UnityEngine;

public class Group : MonoBehaviour
{
    public static readonly float LERP_SPEED = 0.2f;

    private const float GAME_TICK_SECONDS = 1f;

    public bool DrivenByPlayer = true;

    private float _lastFall = 0;

    private GroupReal _real;

    private void Start()
    {
        var o = new GameObject();
        o.transform.position = transform.position;
        o.transform.rotation = transform.rotation;
        o.AddComponent<GroupReal>();
        _real = o.GetComponent<GroupReal>();
        _real.DisplayedGroup = this;

        foreach (Transform child in transform)
        {
            var c = new GameObject();
            c.transform.position = child.position;
            c.transform.rotation = child.rotation;
            c.transform.SetParent(_real.transform);
            c.AddComponent<CubeReal>();
            var _realChild = c.GetComponent<CubeReal>();

            var cube = child.GetComponent<Cube>();
            cube.DisplayedGroup = this;
            _realChild.DisplayedCube = cube;
            
            child.GetComponent<Cube>().Real = c.GetComponent<CubeReal>();
        }

        if (!IsValidGridPos())
        {
            Debug.Log("GAME OVER");
            Destroy(_real.gameObject);
            Destroy(gameObject);
        }
        else
            UpdateGrid();
    }

    bool IsValidGridPos()
    {
        foreach (Transform child in _real.transform)
        {
            Vector3 v = Stack.RoundVec3(child.position);

            if (!Stack.InsideBorder(v))
                return false;

            if (Stack.grid[(int)v.x, (int)v.z, (int)v.y] != null)
                if (Stack.grid[(int)v.x, (int)v.z, (int)v.y].transform.parent != _real.transform)
                    return false;
        }
        return true;
    }

    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        if (!DrivenByPlayer)
        {
            if (_real.transform.childCount == 0)
            {
                //Destroy(_real.gameObject);
                //Destroy(gameObject);
            }
        }
        else
        {
            if (Time.time - _lastFall >= GAME_TICK_SECONDS)
            {
                if (TryMove(new Vector3(0, -1, 0))) { }
                else
                {
                    Stack.DeleteFullPlanes();
                    FindObjectOfType<Spawner>().SpawnNext();
                    DrivenByPlayer = false;
                }
                _lastFall = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftAlt))
                TryRotate(Vector3.forward * 90);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftAlt))
                TryRotate(Vector3.forward * -90);
            else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftAlt))
                TryRotate(Vector3.right * -90);
            else if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftAlt))
                TryRotate(Vector3.right * 90);
            else if (InputEx.GetKeyRepeat(KeyCode.LeftArrow))
                TryMove(Vector3.right);
            else if (InputEx.GetKeyRepeat(KeyCode.RightArrow))
                TryMove(-Vector3.right);
            else if (InputEx.GetKeyRepeat(KeyCode.DownArrow))
                TryMove(Vector3.forward);
            else if (InputEx.GetKeyRepeat(KeyCode.UpArrow))
                TryMove(-Vector3.forward);
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                do
                { } while (TryMove(new Vector3(0, -1, 0)));
                Stack.DeleteFullPlanes();
                FindObjectOfType<Spawner>().SpawnNext();
                DrivenByPlayer = false;
            }

            transform.position = Vector3.Lerp(transform.position, _real.transform.position, LERP_SPEED);
            transform.rotation = Quaternion.Lerp(transform.rotation, _real.transform.rotation, LERP_SPEED);
        }
    }

    private bool TryRotate(Vector3 eulerRotation)
    {
        _real.transform.rotation *= Quaternion.Euler(eulerRotation);
        if (IsValidGridPos())
        {
            UpdateGrid();
            return true;
        }
        else
        {
            _real.transform.rotation *= Quaternion.Euler(-eulerRotation);
            return false;
        }
    }

    private bool TryMove(Vector3 deltaPosition)
    {
        _real.transform.position += deltaPosition;
        if (IsValidGridPos())
        {
            UpdateGrid();
            return true;
        }
        else
        {
            _real.transform.position += -deltaPosition;
            return false;
        }
    }

    private void UpdateGrid()
    {
        for (int x = 0; x < Stack.length; x++)
            for (int y = 0; y < Stack.height; y++)
                for (int z = 0; z < Stack.width; z++)
                    if (Stack.grid[x, z, y] != null && Stack.grid[x, z, y].transform.parent == _real.transform)
                        Stack.grid[x, z, y] = null;

        foreach (Transform child in _real.transform)
        {
            Vector3 v = Stack.RoundVec3(child.position);
            Stack.grid[(int)v.x, (int)v.z, (int)v.y] = child.gameObject.GetComponent<CubeReal>();
        }
    }
}
