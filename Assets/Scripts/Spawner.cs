﻿using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject[] groups;

    public void SpawnNext() {
        int i = Random.Range(0, groups.Length);
        Instantiate(groups[i], transform.position, Quaternion.identity);
    }

    void Start() {
        SpawnNext();
    }
}
