using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ��ȡ��������
/// ����ÿ�������ɫ���Ʊ�
/// 
/// һ������Դͷһ�ű������Ӹ�������ļ���
/// һ���Ӧһ�ű�������������
/// </summary>
public class ReadSurface : MonoBehaviour
{
    public GameObject emptyGO;
    public List<Transform> rays = new List<Transform>(6);
    int layerMask = 1 << 3; // Layer mask for cube object

    [Header("CubeState")]//�������ɫ����
     public List<GameObject> frontState = new List<GameObject>();
     public List<GameObject> backState = new List<GameObject>();
     public List<GameObject> upState = new List<GameObject>();
     public List<GameObject> downState= new List<GameObject>();
     public List<GameObject> leftState = new List<GameObject>();
     public List<GameObject> rightState= new List<GameObject>();

    //����
     List<GameObject> frontRays = new List<GameObject>();
     List<GameObject> backRays = new List<GameObject>();
     List<GameObject> upRays = new List<GameObject>();
     List<GameObject> downRays = new List<GameObject>();
     List<GameObject> leftRays = new List<GameObject>();
     List<GameObject> rightRays = new List<GameObject>();
     
    CubeMap cubeMap;
    void Start()
    {
        SetTranforms();
        cubeMap = FindObjectOfType<CubeMap>();
    }

    void Update()
    {
        //ReadState();
    }

    public void ReadState()//�����������߷����ϼ�⣬���ؼ�⵽������
    {
        cubeMap = FindObjectOfType<CubeMap>();

        upState = ReadFace(upRays, rays[0]);
        downState = ReadFace(downRays, rays[1]);
        leftState = ReadFace(leftRays, rays[2]);
        rightState = ReadFace(rightRays, rays[3]);
        frontState = ReadFace(frontRays, rays[4]);
        backState = ReadFace(backRays, rays[5]);

        cubeMap.SetColor();//������ɫ
    }

    public List<GameObject> ReadFace(List<GameObject>rayStarts, Transform rayTransform)//�����߼���棬��⵽�򷵻������б�
    {
        List<GameObject> hitFace = new List<GameObject>();
        foreach(GameObject rayStart in rayStarts)
        {
            RaycastHit hit;
            Vector3 ray = rayStart.transform.position;
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray,rayTransform.forward * hit.distance, Color.green);
                hitFace.Add(hit.collider.gameObject);

            }
            else Debug.DrawRay(ray, rayTransform.forward * 1000, Color.yellow);
        }       
        return hitFace;
    }

    private List<GameObject> BuildRays(Transform rayTranform, Vector3 dir) //build the checking rays for one surface
    {
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();
        
        // |0|1|2|
        // |3|4|5|
        // |7|8|9|
        for(int y=1;y>-2;y--)
        {
            for(int x=-1;x<2;x++)
            {
                //Vector3 startPos = new Vector3 (rayTranform.localPosition.x + y ,
                //                               rayTranform.localPosition.y + x,
                //                               rayTranform.localPosition.z );
                Vector3 startPos = rayTranform.TransformPoint(new Vector3(y, x, 0));
                GameObject rayStart = Instantiate(emptyGO,startPos,Quaternion.identity,rayTranform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart); 
                rayCount++;
            }         
        }
        rayTranform.localRotation = Quaternion.Euler(dir);
        return rays;
    }

    private void SetTranforms()//�����������
    {

        upRays = BuildRays(rays[0], Vector3.right*90);
        downRays = BuildRays(rays[1], -Vector3.right * 90);
        leftRays = BuildRays(rays[2], Vector3.forward * 90);
        rightRays = BuildRays(rays[3], Vector3.up * 180);
        frontRays = BuildRays(rays[4], -Vector3.up * 90);
        backRays = BuildRays(rays[5], Vector3.up * 90);
    }

    public void PickUp(List<GameObject> cubeFace)//���������ڵı������
    {
        foreach(GameObject face in cubeFace)
        {
            // attach the parent of each face (the little cube)
            // to the parent of the 4th index (the little cube in the middle)
            // unless it is already the 4th index
            if(face != cubeFace[4])
            {
                face.transform.parent.transform.parent = cubeFace[4].transform.parent; //��������ĸ���������Ϊ��һ��������ĸ�����
            }
        }
        //start the side rotaton logic
        cubeFace[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeFace);//������ cubeFace �б�������Ϊ 4 �ķ������ڵ���ĸ������Ϲ��ص� PivotRotation ����� Rotate ����
    }

    public void PutDown(List<GameObject> littleCubes,Transform  pivot)//ת���������
    {
        foreach(GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }
}
