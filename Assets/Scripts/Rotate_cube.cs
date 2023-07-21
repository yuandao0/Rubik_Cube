using UnityEngine;

/// <summary>
/// 旋转魔方：
/// 1. 如果拖拽，则跟着鼠标移动方向旋转
/// 2. 如果拖拽完毕，自动旋转90°
/// 
/// 具体实施：
/// 1. 判断是否拖拽魔方(isDragging, hasDraged)
/// 2. 定义旋转域(rotateDirction)
/// 3. 限制只转一次，且平滑完成()
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
            autoRotateDir = Vector3.up; // 左划：向左旋转（0, 1, 0）
        }
        else if (deltaNor.x > 0 && deltaNor.y > -border && deltaNor.y < border)
        {
            autoRotateDir = Vector3.down; // 右划：向右旋转（0, -1, 0）
        }
        else if (deltaNor.y >= border && deltaNor.x < 0)
        {
            autoRotateDir = Vector3.left; // 上左划：向上左旋转（-1, 0, 0）
        }
        else if (deltaNor.y <= -border && deltaNor.x > 0)
        {
            autoRotateDir = Vector3.right; // 下右划：向下右旋转（1, 0, 0）
        }
        else if (deltaNor.y >= border && deltaNor.x > 0)
        {
            autoRotateDir = Vector3.forward; // 上右划：向上右旋转（0, 0, 1）
        }
        else if (deltaNor.y <= -border && deltaNor.x < 0)
        {
            autoRotateDir = Vector3.back; // 下左划：向下左旋转（0, 0, -1）
        }
    }
}