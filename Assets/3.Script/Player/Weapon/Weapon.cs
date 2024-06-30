using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private enum Type { Melee, Range }
    [SerializeField] private Type type;
    [SerializeField] private int damage;
    [SerializeField] public float rate;
    [SerializeField] private BoxCollider meleeArea;
    [SerializeField] private GameObject trailEffect;
    [SerializeField] private ParticleSystem HammerEffect;

    public Transform bulletPoistion;
    public GameObject bullet;
    public Transform bulletCasePosition;
    public GameObject bulletCase;

    private Coroutine Swing_Coroutine = null;

    public int Damage { get => damage; }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletCasePrefab;
    private GameObject[] bulletPool;
    private GameObject[] bulletCasePool;
    private int poolSize = 10;
    private int nextBullet = 0;
    private int nextBulletCase = 0;

    private void Start()
    {
        //불렛 초기화
        bulletPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab);
            bulletPool[i].SetActive(false);
        }
        // 탄피 초기화
        bulletCasePool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            bulletCasePool[i] = Instantiate(bulletCasePrefab);
            bulletCasePool[i].SetActive(false);
        }
    }

    public void Use()
    {
        if (type == Type.Melee)
        {
            if(Swing_Coroutine != null)
            {
                StopCoroutine("Swing");
                meleeArea.enabled = false;
                trailEffect.SetActive(false);
                Swing_Coroutine = null;
            }

            Swing_Coroutine = StartCoroutine("Swing");
        }
        else if (type == Type.Range)
        {
            StartCoroutine("Shot");
        }
    }

    private IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;
        trailEffect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        meleeArea.enabled = false;
        trailEffect.SetActive(false);
    }

    //private IEnumerator Shot()
    //{
    //    //총알 발사
    //    GameObject instantBullet = Instantiate(bullet, bulletPoistion.position, bulletPoistion.rotation);
    //        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
    //    bulletRigid.velocity = bulletPoistion.forward * 50;
    //    yield return null;

    //    //탄피
    //    GameObject instantCase = Instantiate(bulletCase, bulletCasePosition.position, bulletCasePosition.rotation);
    //    Rigidbody caseaRigid = instantCase.GetComponent<Rigidbody>();
    //    Vector3 caseVec = bulletCasePosition.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
    //    caseaRigid.AddForce(caseVec, ForceMode.Impulse);
    //    caseaRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    //}
    private IEnumerator Shot()
    {
        // 오브젝트 풀에서 총알 가져오기
        GameObject bullet = GetNextBulletFromPool();
        GameObject bulletCase = GetNextBulletCaseFromPool();
        if (bullet != null)
        {
            bullet.transform.position = bulletPoistion.position;
            bullet.transform.rotation = bulletPoistion.rotation;
            bullet.SetActive(true);

            // 총알 발사 로직
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = bullet.transform.forward * 50;
        }
        if (bulletCase != null)
        {
            bulletCase.transform.position = bulletCasePosition.position;
            bulletCase.transform.rotation = bulletCasePosition.rotation;
            bulletCase.SetActive(true);

        }
        // 탄피 발사 로직
        //bulletCase = Instantiate(bulletCasePrefab, bulletCasePosition.position, bulletCasePosition.rotation);
        Rigidbody caseRigidbody = bulletCase.GetComponent<Rigidbody>();
        Vector3 caseForce = bulletCasePosition.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigidbody.AddForce(caseForce, ForceMode.Impulse);
        caseRigidbody.AddTorque(Vector3.up * 10, ForceMode.Impulse);

        yield return null;
    }

    private GameObject GetNextBulletFromPool()
    {
        // 다음 사용할 총알을 풀에서 가져오기
        GameObject bullet = bulletPool[nextBullet];
        nextBullet = (nextBullet + 1) % poolSize;
        return bullet;
    }
    private GameObject GetNextBulletCaseFromPool()
    {
        // 다음 사용할 총알을 풀에서 가져오기
        GameObject bulletCase = bulletCasePool[nextBulletCase];
        nextBulletCase = (nextBulletCase + 1) % poolSize;
        return bulletCase;
    }
    
}
