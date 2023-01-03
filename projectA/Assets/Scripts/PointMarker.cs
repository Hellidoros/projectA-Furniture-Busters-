using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointMarker : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Transform _target;


    void Update()
    {
        float minX = _image.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = _image.GetPixelAdjustedRect().width / 2;
        float maxY = Screen.width - minY;
        Vector2 pos = Camera.main.WorldToScreenPoint(_target.position);

        if(Vector3.Dot((_target.position - transform.position), transform.forward) < 0)
        {
            //Target is behind the player
            if(pos.x< Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        _image.transform.position = pos;
    }
}
