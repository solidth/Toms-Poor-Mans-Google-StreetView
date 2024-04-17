using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoramaFade : MonoBehaviour
{
    public bool useLerp = false;
    public Transform[] spheres;
    public float transitionDuration = 0.1f;
    private int currentSphereIndex = 0;
    private bool isTransitioning = false;

    public KeyCode key = KeyCode.Space;
    public float speedScale = 0.15f;
    public Color fadeColor = Color.black;
    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
    public bool startFadedOut = false;


    private float alpha = 0f; 
    private Texture2D texture;
    private int direction = 0;
    private float time = 0f;

    private void Start()
    {
        if (startFadedOut) alpha = 1f; else alpha = 0f;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
    }

    void Update()
    {
        if (!isTransitioning)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (alpha >= 1f) // Fully faded out
                {
                    alpha = 1f;
                    time = 0f;
                    direction = 1;
                }
                else // Fully faded in
                {
                    alpha = 0f;
                    time = 1f;
                    direction = -1;
                }
                SwitchToNextSphere();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                // Fully faded out
                if (alpha >= 1f) 
                {
                    alpha = 1f;
                    time = 0f;
                    direction = 1;
                }
                else // Fully faded in
                {
                    alpha = 0f;
                    time = 1f;
                    direction = -1;
                }
                SwitchToPreviousSphere();
            }
        }
    }

    void SwitchToNextSphere()
    {
        // Start transition to the center of the next sphere
        int nextSphereIndex = (currentSphereIndex + 1) % spheres.Length;
        StartCoroutine(TransitionToSphereCenter(spheres[nextSphereIndex].position, nextSphereIndex));
    }

    void SwitchToPreviousSphere()
    {
        // Start transition to the center of the previous sphere
        // Ensure looping behavior by adding spheres.Length to handle negative indices
        int prevSphereIndex = (currentSphereIndex - 1 + spheres.Length) % spheres.Length;
        StartCoroutine(TransitionToSphereCenter(spheres[prevSphereIndex].position, prevSphereIndex));
    }

    IEnumerator TransitionToSphereCenter(Vector3 targetPosition, int targetIndex)
    {
        isTransitioning = true;

        // Store the starting position of the camera
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        if(useLerp){
            while (elapsedTime < transitionDuration)
            {
                float t = elapsedTime / transitionDuration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                
                yield return null;
            }
        }
        transform.position = targetPosition;
        isTransitioning = false;
        currentSphereIndex = targetIndex;
        yield return null;
    }

    public void OnGUI()
    {
        if (alpha > 0f) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        if (direction != 0)
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = Curve.Evaluate(time);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            if (alpha <= 0f || alpha >= 1f) direction = 0;
        }
    }
}
