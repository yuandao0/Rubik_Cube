using UnityEngine;

/// <summary>
/// ��תħ����
/// 1. �����ק�����������ƶ�������ת
/// 2. �����ק��ϣ��Զ���ת90��
/// 
/// ����ʵʩ��
/// 1. �ж��Ƿ���קħ��(isDragging, hasDraged)
/// 2. ������ת��(rotateDirction)
/// 3. ����ֻתһ�Σ���ƽ�����()
/// </summary>
public class Rotate_cube : MonoBehaviour
{
    private Vector3 autoRotateDir = Vector3.zero;
    private bool isDragging = false;
    private bool hasDragged = false;
    private Vector2 prePos;
    private float border = 0.5f;
    public float rotateSpeed;
    public GameObject target;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (isDragging && !hasDragged)
        {
            Vector2 postPos = Input.mousePosition;
            Vector2 manualDir = new Vector2(-(postPos - prePos).y, (postPos - prePos).x).normalized;
            transform.rotation = Quaternion.Euler(manualDir * 15 * Time.deltaTime) * transform.rotation;
            prePos = postPos;
        }
        else if (!isDragging && hasDragged)
        {
            target.transform.Rotate(autoRotateDir * 90, Space.World);
            autoRotateDir = Vector3.zero;
            if (transform.rotation != target.transform.rotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, rotateSpeed * Time.deltaTime);
            }
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        hasDragged = false;
        prePos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        hasDragged = true;
        Vector2 currentPos= Input.mousePosition;
        Vector2 deltaPos = currentPos - prePos;
        Vector2 deltaNor = deltaPos.normalized;

        if (deltaNor.x < 0 && deltaNor.y > -border && deltaNor.y < border)
        {
            autoRotateDir = Vector3.up; // �󻮣�������ת��0, 1, 0��
        }
        else if (deltaNor.x > 0 && deltaNor.y > -border && deltaNor.y < border)
        {
            autoRotateDir = Vector3.down; // �һ���������ת��0, -1, 0��
        }
        else if (deltaNor.y >= border && deltaNor.x < 0)
        {
            autoRotateDir = Vector3.left; // ���󻮣���������ת��-1, 0, 0��
        }
        else if (deltaNor.y <= -border && deltaNor.x > 0)
        {
            autoRotateDir = Vector3.right; // ���һ�����������ת��1, 0, 0��
        }
        else if (deltaNor.y >= border && deltaNor.x > 0)
        {
            autoRotateDir = Vector3.forward; // ���һ�����������ת��0, 0, 1��
        }
        else if (deltaNor.y <= -border && deltaNor.x < 0)
        {
            autoRotateDir = Vector3.back; // ���󻮣���������ת��0, 0, -1��
        }
    }
}