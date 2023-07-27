using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide = new List<GameObject>(); //�洢��ǰ�����棨Ҫ������ת���棩�� GameObject �б�
    private Vector3 localForward;//��ʾ��ǰ��������ħ�����ĵ�ƫ������
    private Vector3 mouseRef;//���ڴ洢���λ�õĲο�ֵ
    private bool dragging = false;//���ڱ�ʶ�Ƿ�������ק��ת��
    private float sensitivity = 0.4f;//��ת��������
    private Vector3 rotation;//���ڴ洢��ת������ŷ���ǣ����Ƹ������ת�Ƕȣ�
    private bool autoRotating = false;
    private ReadSurface readSurface;//���ڴ洢�Ͷ�ȡħ��������Ϣ
    private float speed = 300f;
    private Quaternion targetQuaternion;

    void Start()
    {
        readSurface=FindObjectOfType<ReadSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dragging)
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
    private void SpinSide(List<GameObject> side) //����ʵ�������ת��������������ƶ���ƫ����������ת�Ƕȣ�Ȼ����� Transform.Rotate() ������ʵ����ת
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

    public void Rotate(List<GameObject> side)//���ڼ���ָ���沢��ʼ��ת����
    {
        activeSide=side;
        mouseRef=Input.mousePosition;
        dragging = true;
        //create a vector3 to rotate around
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;

    }

    public void RotateToRightAngle()//ת90��
    {
        Vector3 vec = transform.localEulerAngles; 
        // round vec to nearest 90 degrees
        vec.x=Mathf.Round(vec.x/90)*90;
        vec.y=Mathf.Round(vec.y/90)*90;
        vec.z=Mathf.Round(vec.z/90)*90;
        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    private void AutoRotate()//�Զ���ת
    {
        dragging= false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        if(Quaternion.Angle(transform.localRotation,targetQuaternion)<=1)//���ֻ��һ��
        {
            transform.localRotation = targetQuaternion;
            //unparent the little cubs
            readSurface.PutDown(activeSide,transform.parent);
            readSurface.ReadState();
            autoRotating=false;
            dragging= false;
        }
    }
}
