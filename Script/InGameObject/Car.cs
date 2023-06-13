using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Car : MonoBehaviour
{
    [SerializeField] SphereCollider NoiseRnage;
    [SerializeField] AudioSource AudioSources;
    [SerializeField] Rigidbody rigid;
    public bool IsNoise { get; private set; } = false;
    private float MaxRange = 10.0f;

    public void CallNoise()
    {
        if (IsNoise) return;

        AudioSources.clip = GameManager.Instance.FindAudio(AUDIOCLIP.Car_ALRAM);
        StartCoroutine(ContinueNoise());
    }

    IEnumerator ContinueNoise()
    {
        IsNoise = true;
        NoiseRnage.radius = MaxRange;
        AudioSources.Play();

        yield return new WaitForSeconds(5.0f);
        IsNoise = false;
        NoiseRnage.radius = 0.0f;
        AudioSources.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "bullet":
                Vector3 rectVec = transform.position - other.transform.position;  // ���� ��ġ���� �ǰ� ��ġ�� ���� ���ۿ� ���� ���ϱ�
                OnDamage(rectVec); // �˹�
                break;
            default:
                break;
        }

    }

    public void OnDamage(Vector3 rectVec)
    {
        rectVec = rectVec.normalized;
        Debug.Log("�̴� �� : " + rectVec * 5);
        rigid.AddForce(rectVec * 5, ForceMode.Impulse);
    }

}
