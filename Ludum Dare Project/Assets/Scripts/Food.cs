using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    void Update()
    {
        InvokeRepeating("randomMovements", 1f, 1f);
    }

    void randomMovements() {
        float moveBy = 0.005f;
        this.transform.position += new Vector3(
            Random.Range(-moveBy, moveBy),
            Random.Range(-moveBy, moveBy),
            0
        );
    }
}
