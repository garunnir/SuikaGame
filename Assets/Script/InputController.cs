using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Transform left;
    Transform right;
    bool isUI=false;
    private void Start()
    {
        float[] lr = GameManager.Instance.gameController.CalLeftRight();
        left=new GameObject().transform;
        right=new GameObject().transform;
        right.parent=left.parent = GameManager.Instance.gameController.GetCreatePos();
        left.transform.position = Vector3.one* lr[0];
        right.transform.position = Vector3.one * lr[1];
        GameManager.Instance.resourceController.IsLoadDone += () =>
        {
            GameManager.Instance.gameController.Restart();

        };
    }
    private void Update()
    {
        if (!GameManager.isGameRunning) return;
        if (Input.GetMouseButtonDown(0))
        {
            isUI=GameManager.Instance.gameController.CalBlocKUIs();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isUI) 
            {
                isUI = false;
                return;
            }
            MergeSender cur = GameManager.Instance.mergeController.GetCurrent();
            if (cur == null) return;
            //cur.transform.position = GetPos();
            GameManager.Instance.mergeController.Release();
            GameManager.Instance.gameController.AddScore(20);
            GameManager.Instance.gameController.ShowScore();
            //cur.GetRb().AddForce(Vector2.down*200);
        }
        else if (Input.GetMouseButton(0))
        {
            if (isUI) return;
            MergeSender cur = GameManager.Instance.mergeController.GetCurrent();
            if (cur != null)
            {
                Transform target = GameManager.Instance.gameController.GetCloneParent();
                Vector3 ne = GetPos();
                ne.z = -1;
                ne=target.InverseTransformPoint(ne);
                float offset = cur.transform.localScale.x/2;
                if(ne.x< target.InverseTransformPoint(left.position).x+offset)ne.x=target.InverseTransformPoint(left.position).x + offset;
                else if(ne.x> target.InverseTransformPoint(right.position).x - offset)ne.x=target.InverseTransformPoint(right.position).x - offset;
                ne.y = target.InverseTransformPoint(GameManager.Instance.gameController.GetCreatePos().position).y;
                cur.transform.localPosition = ne;
            }
        }
        else
        {

        }
    }
    Vector3 GetPos()
    {
        Vector3 calpos = Input.mousePosition;
        calpos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(calpos);
    }
}
