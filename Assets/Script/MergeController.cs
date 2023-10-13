using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeController : MonoBehaviour
{
    private Guid check;
    MergeSender m_currentHolded;
    MergeSender m_current;
    //[SerializeField]CircleCollider2D m_fakeCol;

    public MergeSender GetCurrent() => m_currentHolded;
    private void FixedUpdate()
    {
        if (m_currentHolded != null) return;
        
        int val = 0;
        foreach (var item in GameManager.Instance.resourceController.GetBallList())
        {
            if (item == m_current&&!m_current.touchDowned) val++;
            else if (item.GetDelta().magnitude > 0.02f) val++;
        }
        if (val == 0) 
        { 
            if(m_current != null)
            {
                GameManager.Instance.gameController.HeavyShot(false);
            }
            GetBall(UnityEngine.Random.Range(0,4));
        }
        //print("delta: " + val);
    }
    public void GetBall(int idx)
    {
        if (!GameManager.isGameRunning) return;

        m_currentHolded =GameManager.Instance.resourceController.GetBall(idx);
        m_current = m_currentHolded;
        m_currentHolded.transform.position = GameManager.Instance.gameController.GetCreatePos().position;
        Hold();
    }
    public void Hold()
    {
        //m_fakeCol.enabled = true;
        //m_fakeCol.isTrigger = true;
        //m_fakeCol.transform.parent = m_currentHolded.transform;
        //m_fakeCol.radius = m_currentHolded.GetCol().radius;
        //m_fakeCol.transform.position = m_currentHolded.transform.position;
        //m_fakeCol.transform.localScale = Vector3.one;
        m_currentHolded.GetCol().enabled = false;
        m_currentHolded.GetRb().bodyType = RigidbodyType2D.Static;
    }
    private void OnDrawGizmos()
    {
        if (m_currentHolded == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_currentHolded.transform.position, m_currentHolded.transform.localScale.x);
    }
    public void Release()
    {

        //m_fakeCol.enabled = false;
        //m_fakeCol.transform.parent = null;
        if (m_currentHolded == null) return;

        m_currentHolded.GetCol().enabled = true;
        m_currentHolded.GetRb().bodyType = RigidbodyType2D.Dynamic;
        m_currentHolded = null;
    }
    public void Merge(MergeSender one, MergeSender two)
    {
        StartCoroutine(Cor_Merge(one, two));
        //�������� �ִϸ��̼�
        //if (EventManager.col2ms.ContainsValue(one))
        //{
        //    EventManager.col2ms.Remove(EventManager.col2ms.First(x => x.Value == one).Key);
        //}
        //if (EventManager.col2ms.ContainsValue(two))
        //{
        //    EventManager.col2ms.Remove(EventManager.col2ms.First(x => x.Value == two).Key);
        //}

    }
    public void ReceiveColl(MergeSender send,Collision2D collision)
    {
        GameManager.Instance.soundController.OneShot(SoundController.Audio.creek);
        if (collision.gameObject.name == "Wall") return;//���� ����
        //if (send == m_current|| m_current == null)
        //{
            //print("���ݰ��浹");
            //send.touchDowned = true;
            //GameManager.Instance.mergeController.GetBall(UnityEngine.Random.Range(0, 5));
        //}
        //else
        {
            send.touchDowned = true;
            send.InitRB();

            //print("�ߺ��˻�");
            if (send.GetGuid() == check) return;//�ߺ�üũ
            //print("���浹");
            //if (send == m_current)//������ ù�浹
            //{
            //    //Ʈ���� �ɷ��ִ»��¿������� �� ������Ʈ�� �����ִµ� �� �����ִ� ������Ʈ�� ���������� �� ������ ä����¿��� �� ä��� ���Ӿƿ�
            //    if (GameManager.Instance.eventManager.GetTime() > 0)
            //    {
            //        if (m_tmp == null) m_tmp = m_current;
            //        else print("gameover");
            //    }
            //    else if (GameManager.Instance.eventManager.GetTime() == 0) m_tmp = null;
            //    send.touchDowned = true;
            //    GameManager.Instance.mergeController.GetBall(UnityEngine.Random.Range(0, 5));
            //}
            if (send.Getlv() == GameManager.Instance.resourceController.GetBallCount() - 1) return;//������ �ε������� ���������� ������
            MergeSender othersender;
            if (collision.gameObject.TryGetComponent(out othersender))
            {
                if (send.Getlv() == othersender.Getlv())
                {
                    GameManager.Instance.mergeController.Merge(send, othersender);
                }
                check = othersender.GetGuid();
            }
        }
    }
    IEnumerator Cor_Merge(MergeSender one, MergeSender two)
    {
        int nextlv = one.Getlv() + 1;
        MergeSender obj = GameManager.Instance.resourceController.GetBall(nextlv);
        int dividvalue = 10;
        Vector3 scale = obj.transform.localScale;
        Vector3 originOne = one.transform.localScale;
        Vector3 originTwo = two.transform.localScale;
        obj.transform.position = (one.transform.position + two.transform.position) / 2;
        obj.transform.localScale = Vector2.one;
        one.GetCol().enabled = false;
        two.GetCol().enabled = false;
        one.GetRb().bodyType = RigidbodyType2D.Static;
        two.GetRb().bodyType = RigidbodyType2D.Static;
        
        for (int curLevel=1; curLevel<=dividvalue;curLevel++)
        {
            one.transform.localScale = Vector3.Lerp(originOne, Vector3.zero, (float)curLevel / dividvalue);
            one.transform.position = Vector3.Lerp(one.transform.position, obj.transform.position,(float)curLevel / dividvalue);
            two.transform.localScale = Vector3.Lerp(originTwo, Vector3.zero, (float)curLevel / dividvalue);
            two.transform.position = Vector3.Lerp(one.transform.position, obj.transform.position, (float)curLevel / dividvalue);
            obj.transform.localScale = Vector3.Lerp(Vector2.zero, scale, (float)curLevel / dividvalue);
            yield return new WaitForFixedUpdate();
        }
        GameManager.Instance.resourceController.GetBallList().Remove(one);
        GameManager.Instance.resourceController.GetBallList().Remove(two);
        Destroy(one.gameObject);
        Destroy(two.gameObject);
    }
}
