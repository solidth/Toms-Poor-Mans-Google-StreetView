using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class VRCamera : MonoBehaviour
{ // flag to keep track whether we are dragging or not    private Vector3 _origin;
    public  bool isDragging = false;

    // starting point of a camera movement
    float startMouseX;
    float startMouseY;

    float centerX;
    float centerY; 

    // Camera component
    public  Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        // Get our camera component
        cam = this.gameObject.GetComponent<Camera>();
    }

	// Update is called once per frame
	void Update () {
		    
        // if we press the left button and we haven't started dragging
        if(Input.GetMouseButtonDown(0) && !isDragging )
        {                
            // set the flag to true
            isDragging = true;

            // save the mouse starting position
            startMouseX = Input.mousePosition.x;
            startMouseY = Input.mousePosition.y;
        }
        // if we are not pressing the left btn, and we were dragging
        else if(Input.GetMouseButtonUp(0) && isDragging)
        {                
            // set the flag to false
            isDragging = false;
        }
    }

    void LateUpdate()
    {
        // Check if we are dragging
         if(isDragging)
        {
            //Calculate current mouse position
            float endMouseX = Input.mousePosition.x;
            float endMouseY = Input.mousePosition.y;

            //Difference (in screen coordinates)
            float diffX = startMouseX - endMouseX;
            float diffY = startMouseY - endMouseY;

            //New center of the screen
            // if((diffX > 0) ^ (diffY > 0)){
            //     centerX = Screen.width / 2 + diffX;
            //     centerY = Screen.height / 2 + diffY;
            // }
            // else {
            //     centerX = endMouseX;
            //     centerY = endMouseY;
            // }
            centerX = Screen.width / 2 + diffX;
            centerY = Screen.height / 2 + diffY;
            //Get the world coordinate , this is where we want to look at
            Vector3 LookHerePoint = cam.ScreenToWorldPoint(new Vector3(centerX, centerY, cam.nearClipPlane));

            //Make our camera look at the "LookHerePoint"
            transform.LookAt(LookHerePoint);

            //starting position for the next call
            startMouseX = endMouseX;
            startMouseY = endMouseY;
        }
    }
}