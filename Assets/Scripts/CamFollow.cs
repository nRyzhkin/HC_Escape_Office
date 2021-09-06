using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform headTransform;
    public int lookMode;
    [Range(0f, 20f)]public float followDelay = 1f;
    public float rotationForce = 180f;
    public Vector3 runPosOffset;
    public Vector3 runRot;
    public float runVelocity;
    public Vector3 fightPosOffset;
    public Vector3 fightRot;
    public Vector3 deadPosOffset;
    public Vector3 deadRot;

    private Vector3 _currentPos;
    private Vector3 _currentRot;
    private Vector3 _currentRotLerp;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (lookMode)
        {
            case 0:
                // run
                _currentPos = headTransform.position + runPosOffset;
                _currentRot = runRot + Vector3.forward * runVelocity;
                break;

            case 1:
                // fight
                _currentPos = headTransform.position + fightPosOffset;
                _currentRot = fightRot;
                break;

            case 2:
                // dead
                _currentPos = headTransform.position + deadPosOffset;
                _currentRot = deadRot;
                break;
        }

        transform.position = Vector3.Lerp(transform.position, _currentPos, Time.unscaledDeltaTime / followDelay);
        //transform.position = _currentPos;
        _currentRotLerp = Vector3.Lerp(_currentRotLerp, _currentRot, Time.unscaledDeltaTime / followDelay);
        transform.rotation = Quaternion.Euler(_currentRotLerp);
    }


    public void StartNewLevel(float distance)
    {
        _currentPos.z -= distance;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
    }
}
