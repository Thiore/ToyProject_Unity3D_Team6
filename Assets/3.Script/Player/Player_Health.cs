using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    [SerializeField] private int StartHealth = 100;  // 시작 체력
    private int CurrentHealth;  // 현재 체력
    private Animator playerAnimator;
    public bool isDie = false;

    private float behittime = 3f;
    private float belasthit;

    private void Start()
    {
        belasthit = 0;
        CurrentHealth = StartHealth;  // 시작할 때 현재 체력을 시작 체력으로 설정
        playerAnimator = GetComponentInChildren<Animator>();
    }

    public void OnDamage(int damage)
    {
        CurrentHealth -= damage;  // 데미지를 받으면 현재 체력을 감소
        Debug.Log("데미지");
        if (CurrentHealth <= 0)
        {
            Die();  // 체력이 0 이하가 되면 Die 메서드 호출
        }
    }

    private void Die()
    {
        // 플레이어가 죽었을 때 처리
        Debug.Log("Player is dead!");
        // 여기서 플레이어 사망 시 처리를 추가할 수 있습니다. 예: 게임 오버 화면 표시, 플레이어 비활성화 등
        playerAnimator.SetTrigger("Die");
        isDie = true;

    }

    private void OnTriggerStay(Collider other)
    {
        if(!isDie && Time.time >= behittime + belasthit)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                belasthit = Time.time;
                Debug.Log(belasthit);
                Debug.Log(":맞");
                OnDamage(1);
            }
        }

    }
}
