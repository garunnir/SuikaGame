using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Rigidbody2D))]
public class MergeSender : MonoBehaviour
{
    public Guid GetGuid() { return m_guid; }
    public void Setlv(int lv)=>m_lv = lv;
    public int Getlv()=>m_lv;
    public Rigidbody2D GetRb() {  return m_rb; }
    private Material m_mat;
    public Material GetMat()=>m_mat;
    public bool touchDowned = false;

    private Guid m_guid = Guid.NewGuid();
    private Rigidbody2D m_rb;
    private CircleCollider2D m_circleCollider;
    public CircleCollider2D GetCol() => m_circleCollider;
    [SerializeField] private int m_lv = -1;
    private Vector3 prevPos;
    private Vector3 m_delta;
    public Vector3 GetDelta() { return m_delta; }
    private float m_originMass=0;
    //public bool releaseable = true;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_circleCollider = GetComponent<CircleCollider2D>();
        m_mat= GetComponentInChildren<MeshRenderer>().material;
        m_originMass = m_rb.mass;
        //EventManager.col2ms.Add(GetComponent<Collider2D>(), this);
    }
    public void InitRB()
    {
        m_rb.mass = m_originMass;
    }
    private void FixedUpdate()
    {
        m_delta= transform.position - prevPos;
        prevPos = transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision) => GameManager.Instance.mergeController.ReceiveColl(this,collision);
    //private void OnTriggerEnter(Collider other)
    //{
    //    print("enter");
    //    releaseable = false;
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    print("exit");
    //    releaseable= true;
    //}
}
