using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroInput : MonoBehaviour
{
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("useGyro",1) == 0) return;
        Input.gyro.enabled = true;
        print("gyro on");
    }
    // Start is called before the first frame update
    private void OnDisable()
    {
        Input.gyro.enabled = false;
        print("gyro off");
        transform.eulerAngles=Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,Input.gyro.rotationRateUnbiased.z);
    }
}
