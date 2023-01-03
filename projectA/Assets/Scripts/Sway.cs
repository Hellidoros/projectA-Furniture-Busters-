using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Sway : MonoBehaviour
{
    [SerializeField] private float _intensity;
    [SerializeField] private float _smooth;
    [SerializeField] private FixedTouchField _fixedTouchField;

    [HideInInspector]public Quaternion OriginRotation;

    private void Start()
    {
        OriginRotation = transform.localRotation;
    }

    void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        //controls

        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");

        //calculate target rotation

        Quaternion t_x_adj = Quaternion.AngleAxis(-_intensity * t_x_mouse, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(_intensity * t_y_mouse, Vector3.up);
        Quaternion targetRotation = OriginRotation * t_x_adj * t_y_adj;

        //rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * _smooth);
    }

    //For Mobile

    //private void UpdateSway()
    //{
    //    if (_fixedTouchField.Pressed)
    //    {
    //        //controls
    //        float t_x_mouse = _fixedTouchField.TouchDist.x;
    //        float t_y_mouse = _fixedTouchField.TouchDist.y;


    //        //calculate target rotation

    //        Quaternion t_x_adj = Quaternion.AngleAxis(-_intensity * t_x_mouse, Vector3.up);
    //        Quaternion t_y_adj = Quaternion.AngleAxis(_intensity * t_y_mouse, Vector3.up);
    //        Quaternion targetRotation = OriginRotation * t_x_adj * t_y_adj;

    //        //rotate towards target rotation
    //        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * _smooth);
    //    }
    //}
}
