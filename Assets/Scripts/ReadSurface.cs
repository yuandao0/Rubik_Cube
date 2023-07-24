using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 读取表面名称
/// 返回每个面的颜色名称表
/// 
/// 一个射线源头一张表，包含从该面射出的激光
/// 一面对应一张表，存各个块的名字
/// </summary>
public class ReadSurface : MonoBehaviour
{
    public List<Transform> rays = new List<Transform>(6);
    int layerMask = 1 << 3; // Layer mask for cube object

    [Header("CubeState")]
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down= new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right= new List<GameObject>();

    CubeMap cubeMap;
     
    void Start()
    {
        cubeMap = FindObjectOfType<CubeMap>();
    }

    void Update()
    {
        SurfDetect();
    }

    private void SurfDetect()
    {
        List<GameObject> hitFace = new List<GameObject>();
        RaycastHit hit;
        Vector3 ray = rays[0].position;
        if (Physics.Raycast(ray, -rays[0].up, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(ray, -rays[0].up * hit.distance,Color.green);
            hitFace.Add(hit.collider.gameObject);
            
        }
        else Debug.DrawRay(ray, -rays[0].up * 1000, Color.yellow);

        up = hitFace;
        cubeMap.SetColor();
    }


}
