using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public float stickOffset = 40f;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 startMousePosition;
    private Vector2 pointB;

    public Transform circle;
    public Transform outerCircle;

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

            startMousePosition = Input.mousePosition;
            circle.transform.position = startMousePosition;
            outerCircle.transform.position = startMousePosition;
            circle.GetComponent<Image>().enabled = true;
            outerCircle.GetComponent<Image>().enabled = true;
        }
        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            touchStart = false;
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                pointA = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.z));

                startMousePosition = touch.position;
                circle.transform.position = startMousePosition;
                outerCircle.transform.position = startMousePosition;
                circle.GetComponent<Image>().enabled = true;
                outerCircle.GetComponent<Image>().enabled = true;
            }

            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.z));

            if (touch.phase == TouchPhase.Ended)
            {
                touchStart = false;
            }
        }
#endif
    }
    public EventHandler<Vector3> OnMoveJoystick;
    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
            var tempDirection = new Vector3(direction.x, 0, direction.y);
            OnMoveJoystick?.Invoke(this, tempDirection * -1);
            circle.transform.position = new Vector2(startMousePosition.x - (direction.x * stickOffset), startMousePosition.y - (direction.y * stickOffset));
        }
        else
        {
            circle.GetComponent<Image>().enabled = false;
            outerCircle.GetComponent<Image>().enabled = false;
        }

    }
}