using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class RotateCube : MonoBehaviour
{
    // Variables for tracking mouse movement
    private Vector2 currentPos;
    private Vector2 nextPos;
    private Vector2 prePos;
    private Vector2 postPos;

    // Variables for checking if the cube is being dragged
    int layerMask = 1 << 3; // Layer mask for cube object
    private bool isDragging = false;
    private bool hasDragged = false;

    // Variables for defining rotation direction
    private Vector3 autoRotateDir = Vector3.zero;
    private Vector3 manualDir = Vector3.zero;
    private float border = 0.5f;
    public float rotateSpeed;
    public GameObject target;

    void Update()
    {
        Rotate();
    }

    private Vector2 DeltaVector()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            hasDragged = false;
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                currentPos = Input.mousePosition;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            hasDragged = true;
            nextPos = Input.mousePosition;
            return nextPos - currentPos;

        }
        return Vector2.zero;
    }

    public void Rotate()
    {
        Vector2 deltaDir = DeltaVector();
        Vector2 deltaNor = deltaDir.normalized;

        // Define the rotation direction based on mouse input
        if (deltaNor.x < 0 && deltaNor.y > -border && deltaNor.y < border) // Left swipe: turn left (0, 1, 0)
            autoRotateDir = Vector3.up; 
        if (deltaNor.x > 0 && deltaNor.y > -border && deltaNor.y < border)  // Right swipe: turn right (0, -1, 0)
            autoRotateDir = Vector3.down;
        if (deltaNor.y >= border && deltaNor.x < 0) // Up-left swipe: turn up-left (-1, 0, 0)
            autoRotateDir = Vector3.left; 
        if (deltaNor.y <= -border && deltaNor.x > 0) // Down-right swipe: turn down-right (1, 0, 0)
            autoRotateDir = Vector3.right; 
        if (deltaNor.y >= border && deltaNor.x > 0) // Up-right swipe: turn up-right (0, 0, 1)
            autoRotateDir = Vector3.forward; 
        if (deltaNor.y <= -border && deltaNor.x < 0) // Down-left swipe: turn down-left (0, 0, -1)
            autoRotateDir = Vector3.back; 

        if (isDragging == false && hasDragged) // Rotate automatically
        {
            target.transform.Rotate(autoRotateDir * 90, Space.World);
            autoRotateDir = Vector3.zero;

            // Smoothly interpolate the rotation towards the target rotation
            if (transform.rotation != target.transform.rotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, rotateSpeed * Time.deltaTime);
            }
        }
        if (isDragging && !hasDragged) // Rotate manually
        {
            postPos = Input.mousePosition;

            manualDir.y = -(postPos - prePos).x;
            manualDir.x = -(postPos - prePos).y;
            manualDir = manualDir.normalized;
            transform.rotation = Quaternion.Euler(manualDir * 15 * Time.deltaTime) * transform.rotation;
        }
        prePos = Input.mousePosition;
    }
}