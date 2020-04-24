using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    Transform willJr;
    Camera cam;
    RectTransform indicatorTransform;
    Image img;
    // Start is called before the first frame update

    private void Awake()
    {
        willJr = GameObject.Find("WilbertJr").transform;
        img = GetComponent<Image>();
        indicatorTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 willJrViewportPos = cam.WorldToViewportPoint(willJr.position);
        if (Mathf.Abs(willJrViewportPos.x - 0.5f) > 0.5f || Mathf.Abs(willJrViewportPos.y - 0.5f) > 0.5f)
        {
            img.enabled = true;
            Vector2 pointOnScreen = cam.ViewportToScreenPoint(willJrViewportPos);
            pointOnScreen = new Vector2(
                Mathf.Clamp(pointOnScreen.x, 50, Screen.width - 50),
                Mathf.Clamp(pointOnScreen.y, 50, Screen.height - 50));
            indicatorTransform.position = pointOnScreen;
        } else
        {
            img.enabled = false;
        }
        
    }
}
