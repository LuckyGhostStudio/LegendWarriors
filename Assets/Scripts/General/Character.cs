using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Base Attributes")]
    public float maxHealth;
    public float currentHealth;

    [Header("Hurt Invulnerable")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;    // 血量改变事件
    public UnityEvent<Transform> OnTakeDamage;      // 受伤事件
    public UnityEvent OnDie;                        // 死亡事件

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
    }

    private void Update()
    {
        // 无敌计时
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 掉进水里
        if (collision.CompareTag("Water"))
        {
            currentHealth = 0;
            OnHealthChange?.Invoke(this);   // 更新血量
            OnDie?.Invoke();                // 死亡
        }
    }

    /// <summary>
    /// 计算受到的伤害
    /// </summary>
    /// <param name="attacker">攻击者</param>
    public void TakeDamage(AttackController attacker)
    {
        if (invulnerable) return;

        currentHealth -= attacker.damage;
        if(currentHealth > 0)
        {
            TriggerInvulnerable();  // 触发无敌

            OnTakeDamage?.Invoke(attacker.transform);   // 处理受伤事件
        }
        else
        {
            currentHealth = 0;

            OnDie?.Invoke();        // 处理死亡事件
        }

        OnHealthChange?.Invoke(this);   // 处理血量改变事件
    }

    /// <summary>
    /// 触发无敌
    /// </summary>
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
