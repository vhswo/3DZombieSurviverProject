using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Camera myCam;
    public Transform myTransform;
    public Transform target;

    public Vector3 pivotOffset = new();
    public Vector3 camOffset = new();

    public float Movingsmooth;
    public float HorizontalAimSpeed;
    public float VerticalAimSpeed;
    public float VerticalMaxAngle;
    public float VerticalMinAngle;
    public float angleH;
    public float angleV;

    private Vector3 FromTargetToCamPos;
    private float FromTargetToCamMag;
    private Vector3 smoothPivotOffset;
    private Vector3 smoothCamOffset;
    private Vector3 targetPivotOffset;
    private Vector3 targetCamOffset;

    private float defalutFov;
    private float targetFov;

    public float bounds = 0f;

    //기즈모 체크
    public Vector3 CheckGizmo;
    public bool GizmoOn = true;
    private void Awake()
    {
        myCam = GetComponent<Camera>();
        myTransform = transform;

        myTransform.position = target.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        myTransform.rotation = Quaternion.identity;

        FromTargetToCamPos = myTransform.position - target.position;
        FromTargetToCamMag = FromTargetToCamPos.magnitude - 0.5f;

        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;
        defalutFov = myCam.fieldOfView;
        angleH = target.eulerAngles.y;

        ResetTargetOffset();
        ResetFOV();
    }

    public void ResetTargetOffset()
    {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
        CheckGizmo = camOffset;
    }

    public void ResetFOV()
    {
        targetFov = defalutFov;
    }

    public void SetFOV(float fov)
    {
        targetFov = fov;
    }

    public void SetTargetOffset(Vector3 newPivotOffset, Vector3 newCamOffset)
    {
        targetPivotOffset = newPivotOffset;
        targetCamOffset = newCamOffset;
        CheckGizmo = newCamOffset;
    }

    bool FromCamToCheckPos(Vector3 CheckPos,float TargetHeight)
    {
        Vector3 target = this.target.position + (Vector3.up * TargetHeight);
        
        if(Physics.SphereCast(CheckPos,0.2f,target - CheckPos,out RaycastHit hit, FromTargetToCamMag))
        {
            if(hit.transform != this.target && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }

        return true;
    }

    bool FromTargetToCheckPos(Vector3 CheckPos,float TargetHeight, float maxdis)
    {
        Vector3 target = this.target.position + (Vector3.up * TargetHeight);

        if(Physics.SphereCast(target,0.2f,CheckPos - target,out RaycastHit hit, maxdis))
        {
            if (hit.transform != this.target && !hit.transform.GetComponent<Collider>().isTrigger)
            {
                return false;
            }
        }
        return true;
    }

    bool DoubleCheckPos(Vector3 CheckPos, float maxdis)
    {
        float TargetHeight = target.GetComponent<CapsuleCollider>().height * 0.7f;
        if (FromCamToCheckPos(CheckPos,TargetHeight) && FromTargetToCheckPos(CheckPos,TargetHeight,maxdis)) return true;

        return false;
    }

    
    private void OnDrawGizmos()
    {
        if (!GizmoOn) return;
        Gizmos.color = Color.cyan;
        Vector3 d = target.position + Quaternion.Euler(0.0f, angleH, 0.0f) * targetPivotOffset;
        Gizmos.DrawSphere(d + myTransform.rotation * CheckGizmo, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position + (Vector3.up * 0.7f),0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(d + myCam.transform.forward,0.2f);
    }

    private void Update()
    {
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * HorizontalAimSpeed;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * VerticalAimSpeed;

        angleV = Mathf.Clamp(angleV, VerticalMinAngle, VerticalMaxAngle);
        angleV = Mathf.LerpAngle(angleV, angleV + bounds, 10f * Time.deltaTime);

        Quaternion CamYRotation = Quaternion.Euler(0.0f, angleH, 0.0f);
        Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0.0f);

        myTransform.rotation = aimRotation;

        myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, targetFov, Time.deltaTime);

        Vector3 TargetRotatedPos = target.position + CamYRotation * targetPivotOffset;
        Vector3 CheckPos = targetCamOffset;

        if(CheckPos.z > 0f)
        {
            if(Physics.SphereCast(target.position + Quaternion.Euler(0.0f, angleH, 0.0f) * targetPivotOffset, 0.2f, target.position + Quaternion.Euler(0.0f, angleH, 0.0f) * targetPivotOffset - myCam.transform.forward, out RaycastHit hit, 0.5f))
            {
                CheckPos.z = -1;
            }
        }

        for (float z = targetCamOffset.z; z <= 0; z +=0.5f)
        {
            CheckPos.z = z;
            if (DoubleCheckPos(TargetRotatedPos + aimRotation * CheckPos, Mathf.Abs(z)) || z == 0f)
            {
                break;
            }
        }

        smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, Movingsmooth * Time.deltaTime);
        smoothCamOffset = Vector3.Lerp(smoothCamOffset, CheckPos, Movingsmooth * Time.deltaTime);

        myCam.transform.position = target.position + CamYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

        bounds = bounds > 0 ? bounds -= 5 * Time.deltaTime : 0f;

    }
}

/*
 * 크로스헤어 커짐 과 반동률 연관성, 미스률 연관성 함수 따로 빼기
 * 홀 만들기
 * 총들면 허리 굽히기 가능
 * 총알 없애고 그냥 레이로 끝내기, 이펙트로 대처?
 * 레이로 다 체크하기 쏘는 순간 맞은걸로
 * 에임일때 앞에 체크하기 에임풀림
 * 인터페이스 만들어서 레이에 맞으면, 인터페이스 정보를 얻어와, 그에 맞는 이펙트, 홀 생성
 */