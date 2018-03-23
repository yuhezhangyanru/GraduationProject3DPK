//using UnityEngine;
//using System.Collections;

//public enum QuadrantLayout
//{
//    Quadrant1,
//    Quadrant2,
//    Quadrant3,
//    Quadrant4
//}

//[ExecuteInEditMode]
//[AddComponentMenu("Auto Layout #0")]
//public class AutoLayout : MonoBehaviour
//{
//    Camera uiCamera;

//    QuadrantLayout quadrant;
//    Vector2 margin;

//    Vector3 lastPos;
//    void OnEnable()
//    {
//        if (uiCamera == null) uiCamera = Camera.main;
//        if (uiCamera == null)
//        {
//            Debug.LogWarning("The uiCamera must not be null!");
//            this.enabled = false;
//            return;
//        }
//#if !UNITY_EDITOR//        redressTransform();//#endif
//    }

//    Rect mRect;
//    float width, height;
//    Vector2 center, vcenter;
//    Vector3 v, sv;
//#if UNITY_EDITOR
//    void Update()
//    {
//        if (uiCamera == null)
//        {
//            uiCamera = Camera.main;
//        }

//        if (Vector3.Distance(transform.position, lastPos) > 0.001f)
//        {//editor
//            updateMarginOffset();
//        }
//        else
//        {
//            redressTransform();
//        }
//        lastPos = transform.position;
//    }
//#endif

//    void updateMarginOffset()
//    {
//        if (uiCamera == null) return;

//        mRect = uiCamera.pixelRect;
//        center = new Vector2((mRect.xMin + mRect.xMax) * 0.5f, (mRect.yMin + mRect.yMax) * 0.5f);
//        width = mRect.width;
//        height = mRect.height;

//        //update offset
//        v = uiCamera.WorldToScreenPoint(transform.position);

//        if (v.x >= center.x && v.y >= center.y)
//        {//1
//            vcenter = new Vector2(width, height);
//            quadrant = QuadrantLayout.Quadrant1;
//        }
//        else if (v.x >= center.x && v.y < center.y)
//        {//2
//            vcenter = new Vector2(width, 0);
//            quadrant = QuadrantLayout.Quadrant2;
//        }
//        else if (v.x < center.x && v.y < center.y)
//        {//3
//            vcenter = new Vector2(0, 0);
//            quadrant = QuadrantLayout.Quadrant3;
//        }
//        else if (v.x < center.x && v.y >= center.y)
//        {//4
//            vcenter = new Vector2(0, height);
//            quadrant = QuadrantLayout.Quadrant4;
//        }
//        margin = new Vector2((v.x - vcenter.x), (v.y - vcenter.y));
//    }

//    void redressTransform()
//    {
//        if (uiCamera == null) return;
//        mRect = uiCamera.pixelRect;
//        width = mRect.width;
//        height = mRect.height;

//        switch (quadrant)
//        {
//            case QuadrantLayout.Quadrant1:
//                sv = new Vector3(width + margin.x, height + margin.y, 0);
//                break;
//            case QuadrantLayout.Quadrant2:
//                sv = new Vector3(width + margin.x, margin.y, 0);
//                break;
//            case QuadrantLayout.Quadrant3:
//                sv = new Vector3(margin.x, margin.y, 0);
//                break;
//            case QuadrantLayout.Quadrant4:
//                sv = new Vector3(margin.x, height + margin.y, 0);
//                break;
//        }
//        sv.z = uiCamera.WorldToScreenPoint(transform.position).z;
//        sv = uiCamera.ScreenToWorldPoint(sv);
//        if (uiCamera.orthographic)
//        {
//            sv.x = Mathf.Round(sv.x);
//            sv.y = Mathf.Round(sv.y);
//        }
//        transform.position = sv;
//    }
//}
