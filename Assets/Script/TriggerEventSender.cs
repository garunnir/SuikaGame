using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventSender : MonoBehaviour
{
    private void Awake()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision) => GameManager.Instance.eventManager.TEnter2D(collision);
    private void OnTriggerExit2D(Collider2D collision)=>GameManager.Instance.eventManager.TExit2D(collision);
    private void OnTriggerStay2D(Collider2D collision) => GameManager.Instance.eventManager.TStay2d(collision);
}
