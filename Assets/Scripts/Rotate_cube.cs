using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotate_cube : MonoBehaviour
{
    //get mouse movement
    private Vector2 currentPos;
    private Vector2 nextPos;
    
    //check whether drag the cube or not
    int layerMask = 1 << 3; 
    public bool isDraged=false;
    public bool hasDraged=false;

    private Vector3 rotateDirction = Vector3.zero;
    public int speed;

    void Start()
    {
        
    }

    
    void Update()
    {
        Rotate();
        print(rotateDirction);

    }

    public Vector2 DeltaVector()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0))
        {
            isDraged= true;
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                currentPos = Input.mousePosition;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDraged = false;
            hasDraged = true;
            nextPos = Input.mousePosition;
            return  nextPos - currentPos; 
 
        }
        return Vector2.zero;
    }

    public void Rotate() 
    {
        Vector2 delta = DeltaVector();

        if (delta.x<0 && delta.y>-0.5f && delta.y<0.5f) rotateDirction = Vector3.up * 90;
        if (delta.x>0 &&delta.y>-0.5f && delta.y<0.5f) rotateDirction = Vector3.down * 90;
        //if (delta.y>0 && delta.x < 0) rotateDirction = Vector3.up * 90;
        //if (delta.y<0 && delta.x > 0) rotateDirction = Vector3.down * 90;
        //if (delta.y > 0 && delta.x > 0) rotateDirction = Vector3.up * 90;
        //if (delta.y < 0 && delta.x < 0) rotateDirction = Vector3.down * 90;

        if (isDraged == false && hasDraged)
        {
            this.transform.Rotate(rotateDirction);
            
        }
        hasDraged = false;
    }
}
