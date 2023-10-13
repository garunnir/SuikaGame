using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    List<MergeSender> colliders=new List<MergeSender>();
    //public static Dictionary<Collider2D,MergeSender> col2ms=new Dictionary<Collider2D,MergeSender>();
    [SerializeField]float time;
    public float GetTime()=>time;
    bool check=false;
    public int GetColCount() => colliders.Count;
    int curIn = 0;
    public void TEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "DeadLine") GameManager.Instance.gameController.GameOver();
        curIn++;
        //if(col2ms[collision].touchDowned)
        //colliders.Add(col2ms[collision]);
    }
    public void TExit2D(Collider2D collision) 
    {
        curIn--;
        //if(col2ms.ContainsKey(collision))
        //colliders.Remove(col2ms[collision]);
    }

    public void TStay2d(Collider2D collision)
    {
        //print("Stay");
        //check = true;
    }
    private void FixedUpdate()
    {
        if (curIn>0)
        {
            //print(curIn);
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
    }
}
