using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    ReadSurface cubeState;
    int layerMsker = 1<<3;

    void Start()
    {
        cubeState = FindObjectOfType<ReadSurface>();
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            cubeState.ReadState();

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit,100.0f,layerMsker))
            {
                GameObject face = hit.collider.gameObject;
                List<List<GameObject>> faces = new List<List<GameObject>>()
                {
                    cubeState.upState,
                    cubeState.downState,
                    cubeState.leftState,
                    cubeState.rightState,
                    cubeState.frontState,
                    cubeState.backState
                };

                //if the face exist within a side
                foreach (List<GameObject> cubeFace in faces)//找到包含被点击物体的面表
                {
                    if(cubeFace.Contains(face))
                    {
                        cubeState.PickUp(cubeFace);//设置它的中心和旋转
                    }
                }
            }
         
        }
    }
}
