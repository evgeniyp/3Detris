using UnityEngine;

public class Group : MonoBehaviour {
    private const float GAME_TICK_SECONDS = 1f;
    private const float LERP_SPEED = 0.2f;

    private bool _drivenByPlayer = true;

    private float _lastFall = 0;

    private GroupReal _real;

    private void Start() {
        var o = new GameObject();
        o.transform.position = transform.position;
        o.transform.rotation = transform.rotation;
        o.AddComponent<GroupReal>();
        _real = o.GetComponent<GroupReal>();
        _real.DisplayedGroup = this;

        foreach (Transform child in transform) {
            var c = new GameObject();
            c.transform.position = child.position;
            c.transform.rotation = child.rotation;
            c.transform.SetParent(_real.transform);
            c.AddComponent<CubeReal>();
            var _realChild = c.GetComponent<CubeReal>();
            _realChild.DisplayedCube = child.GetComponent<Cube>();
        }

        if (!IsValidGridPos()) {
            Debug.Log("GAME OVER");
            Destroy(_real.gameObject);
            Destroy(gameObject);
        } else
            UpdateGrid();
    }

    bool IsValidGridPos() {
        foreach (Transform child in _real.transform) {
            Vector3 v = Grid.RoundVec3(child.position);

            if (!Grid.InsideBorder(v))
                return false;

            if (Grid.grid[(int)v.x, (int)v.z, (int)v.y] != null)
                if (Grid.grid[(int)v.x, (int)v.z, (int)v.y].transform.parent != _real.transform)
                    return false;
        }
        return true;
    }

    void Update() {
        if (!_drivenByPlayer) {
            if (_real.transform.childCount == 0) {
                Destroy(_real.gameObject);
                Destroy(gameObject);
            }
        } else {
            if (Time.time - _lastFall >= GAME_TICK_SECONDS || Input.GetKeyDown(KeyCode.Space)) {
                if (TryMove(new Vector3(0, -1, 0))) { } else {
                    FindObjectOfType<Spawner>().SpawnNext();
                    _drivenByPlayer = false;
                }
                _lastFall = Time.time;
            } else if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftAlt))
                TryRotate(Vector3.forward * 90);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftAlt))
                TryRotate(Vector3.forward * -90);
            else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftAlt))
                TryRotate(Vector3.right * -90);
            else if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftAlt))
                TryRotate(Vector3.right * 90);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                TryMove(Vector3.right);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                TryMove(-Vector3.right);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                TryMove(Vector3.forward);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                TryMove(-Vector3.forward);
            else if (Input.GetKeyDown(KeyCode.Return)) {
                do { } while (TryMove(new Vector3(0, -1, 0)));
                FindObjectOfType<Spawner>().SpawnNext();
                _drivenByPlayer = false;
            }
        }

        transform.position = Vector3.Lerp(transform.position, _real.transform.position, LERP_SPEED);
        transform.rotation = Quaternion.Lerp(transform.rotation, _real.transform.rotation, LERP_SPEED);
    }

    private bool TryRotate(Vector3 eulerRotation) {
        _real.transform.rotation *= Quaternion.Euler(eulerRotation);
        if (IsValidGridPos()) {
            UpdateGrid();
            return true;
        } else {
            _real.transform.rotation *= Quaternion.Euler(-eulerRotation);
            return false;
        }
    }

    private bool TryMove(Vector3 deltaPosition) {
        _real.transform.position += deltaPosition;
        if (IsValidGridPos()) {
            UpdateGrid();
            return true;
        } else {
            _real.transform.position += -deltaPosition;
            return false;
        }
    }

    private void UpdateGrid() {
        for (int x = 0; x < Grid.length; x++)
            for (int y = 0; y < Grid.height; y++)
                for (int z = 0; z < Grid.width; z++)
                    if (Grid.grid[x, z, y] != null && Grid.grid[x, z, y].transform.parent == _real.transform)
                        Grid.grid[x, z, y] = null;

        foreach (Transform child in _real.transform) {
            Vector3 v = Grid.RoundVec3(child.position);
            Grid.grid[(int)v.x, (int)v.z, (int)v.y] = child.gameObject.GetComponent<CubeReal>();
        }
    }
}
