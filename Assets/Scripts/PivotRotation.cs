using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide = new List<GameObject>(); //存储当前激活面（要进行旋转的面）的 GameObject 列表
    private Vector3 localForward;//表示当前激活面与魔方中心的偏移向量
    private Vector3 mouseRef;//用于存储鼠标位置的参考值
    private bool dragging = false;//用于标识是否正在拖拽旋转面
    private float sensitivity = 0.4f;//旋转的灵敏度
    private Vector3 rotation;//用于存储旋转操作的欧拉角（即绕各轴的旋转角度）
    private bool autoRotating = false;
    private ReadSurface readSurface;//用于存储和读取魔方表面信息
    private float speed = 300f;
    private Quaternion targetQuaternion;

    void Start()
    {
        readSurface=FindObjectOfType<ReadSurface>();
    }

    //Late Update is called once per frame at the end
    void LateUpdate()
    {
        if(dragging && !autoRotating)
        {
            SpinSide(activeSide);
            if(Input.GetMouseButtonUp(0))
            {
                dragging = false;
                RotateToRightAngle();
            }           
        }
        if (autoRotating)
        {
            AutoRotate();
        }
    }
    private void SpinSide(List<GameObject> side) //用于实现面的旋转操作。根据鼠标移动的偏移量计算旋转角度，然后调用 Transform.Rotate() 方法来实现旋转
    {
        rotation = Vector3.zero;
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);
        if (side == readSurface.frontState)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1 ;
        }
        if (side == readSurface.backState)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == readSurface.upState)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == readSurface.downState)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == readSurface.leftState)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == readSurface.rightState)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        transform.Rotate(rotation, Space.World);
        mouseRef = Input.mousePosition;
    }

    public void Rotate(List<GameObject> side)//用于激活指定面并开始旋转操作
    {
        activeSide=side;
        mouseRef=Input.mousePosition;
        dragging = true;
        //create a vector3 to rotate around
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;

    }

    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        readSurface.PickUp(side);
        Vector3 localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
    }

    public void RotateToRightAngle()//转90度
    {
        Vector3 vec = transform.localEulerAngles; 
        // round vec to nearest 90 degrees
        vec.x=Mathf.Round(vec.x/90)*90;
        vec.y=Mathf.Round(vec.y/90)*90;
        vec.z=Mathf.Round(vec.z/90)*90;
        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    private void AutoRotate()//自动旋转
    {
        dragging= false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        if(Quaternion.Angle(transform.localRotation,targetQuaternion)<=1)//如果只差一度
        {
            transform.localRotation = targetQuaternion;
            //unparent the little cubs
            readSurface.PutDown(activeSide,transform.parent);
            readSurface.ReadState();
            ReadSurface.autoRotating = false;
            autoRotating=false;
            dragging= false;
        }
    }
}



