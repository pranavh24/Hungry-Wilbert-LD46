using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearRenderer : MonoBehaviour
{
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RenderTexture rt = UnityEngine.RenderTexture.active;
        UnityEngine.RenderTexture.active = cam.targetTexture;
        print(cam.targetTexture);
        GL.Clear(true, true, Color.clear);
        UnityEngine.RenderTexture.active = rt;
    }
}
