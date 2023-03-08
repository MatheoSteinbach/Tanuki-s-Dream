using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private Animator abilityChangeVFX;
    [SerializeField] private Animator abilityRechargeVFX;

    [SerializeField] private CameraShake camShake;
    [SerializeField] private CinemachineVirtualCamera camBlackout;
    private enum State { Idle, Walking, Rolling, Attacking, Dead }

    private Rigidbody2D rb;
    private Vector3 movementInput;
    private Vector3 rollDir;
    private Vector3 lastMoveDir;
    [SerializeField] private float rollSpeedEditor = 65f;
    [SerializeField] private float moveSpeed = 300f;
    private float rollSpeed;
    [SerializeField] float rollCooldown = 2f;
    private float rollTime;

    private State state;

    private bool canMove = true;
    private AnimHandler animHandler;
    private AttackHandler attackHandler;

    // take this to the attackHandler afterwards
    [SerializeField] Transform attackPosition;
    [SerializeField] float distanceToAttack = 0.4f;
    [SerializeField] private Transform attackPivot;
    [SerializeField] Animator dashVFX;
    private float attackTime;
    private bool isAttacking = false;
    [SerializeField] float attackCooldown = 0.65f;
    private bool attackBlocked;
    Vector2 pointerInput;
    private Collider2D collision2D;
    private PlayerWeapon weapon;
    [SerializeField] private Animator clawVFX;
    //[SerializeField] private ParticleSystem walkVFX;
    //[SerializeField] private Animator walkVFXAnimator;
    // take this to the sfxHandler afterwards
    [Header("AUDIO")]
    [SerializeField] AudioSource abilityChangeSFX;
    [SerializeField] AudioSource rangedAttackSFX;
    [SerializeField] AudioSource deathSFX;
    [SerializeField] AudioSource walkingSound;
    [SerializeField] AudioSource RollSound;
    // HEALTH ON OTHER SCRPT PLS
    [SerializeField] private float currentHealth, maxHealth;
    [SerializeField] private Animator HitVFX;
    [SerializeField] private AudioSource HitSFX;
    [SerializeField] private float knockbackDistance;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image[] extraHeartsFill;
    [SerializeField] private Image[] extraHeartsBackground;
    [SerializeField] private Image heart1Fill;
    [SerializeField] private Image heart1Background;
    // Special Attack on its own pls
    [SerializeField] GameObject projectile;
    private bool isSpecialOn = false;
    private float specialTime;
    [SerializeField] float specialCooldown = 2f;

    private bool isDashOn = false;

    private bool hasHitOnDash = false;

    [SerializeField] Animator spawnVFX;
    [SerializeField] Animator healVFX;

    [SerializeField] GameObject ability;
    [SerializeField] Image icon;
    [SerializeField] Sprite dashAbilityIcon;
    [SerializeField] Sprite rangedAbilityIcon;

    [Header("Avatar")]
    [SerializeField] Image avatar;
    [SerializeField] Sprite normalAvatar;
    [SerializeField] Sprite dashAvatar;
    [SerializeField] Sprite rangedAvatar;

    [Header("Particle System")]
    [SerializeField] ParticleSystem walkVFX1;
    [SerializeField] ParticleSystem walkVFX2;

    private int heartsCollected = 0;

    private bool resetAnim = false;

    private bool abilityRecharged = false;

    private float walkTimer = 0f;
    private void Awake()
    {
        weapon = GetComponentInChildren<PlayerWeapon>();

        animHandler = GetComponent<AnimHandler>();
        rb = GetComponent<Rigidbody2D>();
        state = State.Idle;
        collision2D = GetComponent<Collider2D>();
    }
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        pointerInput = Camera.main.ScreenToWorldPoint(mousePos);

        ManageHealth();
        PlayAnimations();
        HandleAbilityRechargeVFX();
        

        rollTime += Time.deltaTime;
        attackTime += Time.deltaTime;
        specialTime += Time.deltaTime;
        if (attackBlocked) { return; }
        weapon.PointerPosition = pointerInput;

    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                rb.velocity = Vector2.zero;
                break;
            case State.Walking:
                if(canMove)
                {
                    rb.velocity = movementInput * moveSpeed * Time.deltaTime;
                    walkTimer += Time.deltaTime;
                    if (walkTimer > 0.5f)
                    {
                        //walkVFX1.Emit(1);
                        //walkVFX2.Emit(1);
                        walkTimer = 0f;
                    }
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
                
                break;
            case State.Rolling:
                rb.velocity = rollDir * rollSpeed;
                break;
            case State.Attacking:
                if(canMove)
                {
                    rb.velocity = movementInput * (moveSpeed / 2) * Time.deltaTime;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
                break;
            default:
                break;
        }

    }

    public void AttackEnded()
    {
        animHandler.StopAttacking(lastMoveDir);
        isAttacking = false;
        canMove = true;

        if (movementInput.x != 0 || movementInput.y != 0)
        {
            resetAnim = false;
            state = State.Walking;
        }
        else
        {
            resetAnim = false;
            state = State.Idle;
        }
    }

    private void PlayAnimations()
    {
        switch (state)
        {
            case State.Idle:
                if (movementInput != Vector3.zero)
                {
                    state = State.Walking;
                }
                if(!isAttacking)
                {
                    animHandler.PlayIdle(lastMoveDir);
                }
                break;

            case State.Walking:
                if (movementInput == Vector3.zero)
                {
                    state = State.Idle;
                }
                if(!isAttacking)
                {
                    if(movementInput != Vector3.zero)
                    {
                        lastMoveDir = movementInput;
                        animHandler.PlayWalk(lastMoveDir);
                    }
                    
                }
               
                if (!walkingSound.isPlaying)
                {
                    //change pitch here
                    walkingSound.Play();
                }
                break;
            case State.Rolling:
                RollSound.Play();

                // find angle to where is moving and dash to it ONCE
                var dir = lastMoveDir * 6000;
                float AngleRad = Mathf.Atan2(-dir.y + attackPivot.position.y, -dir.x + attackPivot.position.x);
                float AngleDeg = (180 / Mathf.PI) * AngleRad;
                attackPivot.rotation = Quaternion.Euler(0, 0, AngleDeg);
                dashVFX.Play("DodgeAnim");
                CheckColliders();
                
                camShake.ShakeCamera(5f, 0.2f);
                float rollSpeedDropMultiplier = 5f;
                rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                float rollSpeedMinimum = 50f;
                if (rollSpeed < rollSpeedMinimum)
                {
                    hasHitOnDash = false;
                    if (movementInput != Vector3.zero)
                    {
                        state = State.Walking;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                }

                break;
            case State.Attacking:
                

                break;
            case State.Dead:
                
                    break;
            default:
                break;
        }
    }
    private void CheckColliders()
    {
        
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 1))
        {
            if (!hasHitOnDash)
            {
                EnemyHealth enemyHealth;
                if (enemyHealth = collider.GetComponent<EnemyHealth>())
                {
                    hasHitOnDash = true;
                    camShake.ShakeCamera(5f, 0.2f);
                    if (enemyHealth.GetComponent<AISpikeyAnimHandler>() != null)
                    {
                        enemyHealth.GetHit(100f, transform.gameObject);
                    }
                    else
                    {
                        enemyHealth.GetHit(33.34f, transform.gameObject);
                    }

                }
                
            }
            GrassTall grass;
            if (grass = collider.GetComponent<GrassTall>())
            {
                camShake.ShakeCamera(5f, 0.2f);
                grass.GetHit(1f, gameObject);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = transform.position == null ? Vector3.zero : transform.position;
        Gizmos.DrawWireSphere(position, 1);
    }

    private void OnMove(InputValue movementValue)
    {
        if (canMove)
        {
            movementInput = movementValue.Get<Vector2>();
            if (movementInput.x != 0 || movementInput.y != 0)
            {
                lastMoveDir = movementInput;

            }
        }
        

    }
    private void OnPointerPosition(InputValue pointerValue)
    {
        //Vector3 mousePos = pointerValue.Get<Vector2>();
        //mousePos.z = Camera.main.nearClipPlane;
        //pointerInput = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnAttack()
    {
        if(attackTime > attackCooldown && canMove)
        {
            
            // 
            //var dirToAttack = (attackPosition.position - transform.position).normalized;
            //transform.position += dirToAttack * distanceToAttack;

            state = State.Attacking;
            StartCoroutine(StopAttackOnTime());

            var dirToAttack = (attackPosition.position - transform.position).normalized;
            lastMoveDir = dirToAttack;
            animHandler.PlayAttack(lastMoveDir);
            //canMove = false;
            attackTime = 0;
            isAttacking = true;

        }
    }
    private void OnRoll()
    {
        if (isSpecialOn)
        {
            if (specialTime > specialCooldown)
            {
                // play sound
                rangedAttackSFX.Play();
                var dirToAttack = (attackPosition.position - transform.position).normalized;
                lastMoveDir = dirToAttack;
                animHandler.PlayThrow(lastMoveDir);
                var projectile1 = Instantiate(projectile, transform.position, Quaternion.identity);

                projectile1.GetComponent<ProjectilePlayer>().SetTargetPosition(pointerInput);
                specialTime = 0;
                isAttacking = true;
                abilityRecharged = false;
                //StartCoroutine(StopAttackOnTime());
            }
        }
        if (isDashOn)

            if (rollTime > rollCooldown)
            {
                switch (state)
                {
                    case State.Idle:
                        abilityRecharged = false;
                        rollTime = 0;
                        rollDir = lastMoveDir;
                        rollSpeed = rollSpeedEditor;
                        state = State.Rolling;
                        break;
                    case State.Walking:
                        abilityRecharged = false;
                        rollTime = 0;
                        rollDir = lastMoveDir;
                        rollSpeed = rollSpeedEditor;
                        state = State.Rolling;
                        break;
                    default:
                        break;
                }
            }
        {

        }
    }

    private void OnSpecialAttack()
    {
        //if(isSpecialOn)
        //{
        //    if(specialTime > specialCooldown)
        //    {
        //        // play sound
        //        animHandler.PlayAttack(lastMoveDir);
        //        Instantiate(projectile, transform.position, Quaternion.identity);
        //        specialTime = 0;
        //    }
        //}
    }
    public void ActivateSpecial()
    {
        abilityChangeSFX.Play();
        spawnVFX.Play("SpawnVFX");
        animHandler.ActivateKappa();
        ability.SetActive(true);
        icon.sprite = rangedAbilityIcon;
        avatar.sprite = rangedAvatar;
        if(!isDashOn && !isSpecialOn)
        {
            abilityChangeVFX.Play("TanukiToKappaAnim");
        }
        else if(isDashOn)
        {
            abilityChangeVFX.Play("FroggoToKappaAnim");
        }
        isDashOn = false;
        isSpecialOn = true;
        //specialImage.SetActive(true);
    }
 
    public void ActivateDash()
    {
        abilityChangeSFX.Play();
        spawnVFX.Play("SpawnVFX");
        animHandler.ActivateFroggo();
        ability.SetActive(true);
        icon.sprite = dashAbilityIcon;
        avatar.sprite = dashAvatar;
        if (!isDashOn && !isSpecialOn)
        {
            abilityChangeVFX.Play("TanukiToFroggoAnim");
        }
        else if (isSpecialOn)
        {
            abilityChangeVFX.Play("KappaToFroggoAnim");
        }
        isSpecialOn = false;
        isDashOn = true;
        //specialImage.SetActive(true);
    }


    // REMOVE COOLDOWN
    // DEACTIVATE ON DMG
    private IEnumerator StopAttackOnTime()
    {
        yield return new WaitForSeconds(0.45f);
        
        if(isAttacking)
        {
            animHandler.StopAttacking(lastMoveDir);
            isAttacking = false;
            canMove = true;

            if (movementInput.x != 0 || movementInput.y != 0)
            {
                resetAnim = false;
                state = State.Walking;
            }
            else
            {
                resetAnim = false;
                state = State.Idle;
            }
        }
        
    }
    public void GetHit(float amount, GameObject sender)
    {
        if (sender.layer == gameObject.layer) { return; }
        camShake.ShakeCamera(5f, 0.2f);
        currentHealth -= amount;
        Knockback(sender);
        HitSFX.Play();
        HitVFX.Play("HitAnim");

        animHandler.ActivateNormal();
        avatar.sprite = normalAvatar;
        ability.SetActive(false);
        if(isSpecialOn)
        {
            abilityChangeVFX.Play("KappaToTanukiAnim");
            spawnVFX.Play("SpawnVFX");
            abilityChangeSFX.Play();
            isSpecialOn = false;
        }
        if(isDashOn)
        {
            abilityChangeVFX.Play("FroggoToTanukiAnim");
            abilityChangeSFX.Play();
            spawnVFX.Play("SpawnVFX");
            isDashOn = false;
        }
        
        

        if (currentHealth <= 0)
        {
            state = State.Dead;
            canMove = false;
            movementInput = Vector3.zero;
            lastMoveDir = Vector3.zero;
            rb.velocity = Vector2.zero;
            collision2D.enabled = false;
            animHandler.PlayDead(lastMoveDir);
            StartCoroutine(StartDeathSequence());
            deathSFX.Play();
        }
    }
    IEnumerator StartDeathSequence()
    {
        
        yield return new WaitForSeconds(3f);
        camBlackout.m_Lens.OrthographicSize = 6f;
        camBlackout.transform.position = transform.position;
        camBlackout.gameObject.SetActive(true);
        StartCoroutine(ResetLevel());
    }
    IEnumerator ResetLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }
    private void Knockback(GameObject other)
    {
        Vector3 dirFromPlayer = (transform.position - other.transform.position).normalized;

        transform.position += dirFromPlayer * knockbackDistance;
    }
    private void HandleAbilityRechargeVFX()
    {
        if (isSpecialOn)
        {
            if (specialTime > specialCooldown && !abilityRecharged)
            {
                abilityRechargeVFX.Play("AbilityRechargeAnim");
                abilityRecharged = true;
            }
        }
        if (isDashOn)
        {
            if (rollTime > rollCooldown && !abilityRecharged)
            {
                abilityRechargeVFX.Play("AbilityRechargeAnim");
                abilityRecharged = true;
            }
        }
    }
    private void ManageHealth()
    {
        hpBar.fillAmount = currentHealth / 100f;
        if (currentHealth > 100f)
        {
            extraHeartsFill[0].gameObject.SetActive(true);
        }
        else
        {
            extraHeartsFill[0].gameObject.SetActive(false);
        }
        if (currentHealth > 116.67f)
        {
            extraHeartsFill[1].gameObject.SetActive(true);
        }
        else
        {
            extraHeartsFill[1].gameObject.SetActive(false);
        }
        if (currentHealth > 133.34f)
        {
            extraHeartsFill[2].gameObject.SetActive(true);
        }
        else
        {
            extraHeartsFill[2].gameObject.SetActive(false);
        }
    }

    public void HealFullHealth()
    {
        currentHealth = maxHealth;
        healVFX.Play("Heal");
        

    }

    public void DisableMovement()
    {
        canMove = false;
        movementInput = Vector3.zero;
        lastMoveDir = Vector3.zero;
        rb.velocity = Vector2.zero;
        
    }

    public void HeartCollected()
    {
        heartsCollected++;
        maxHealth += 16.67f;
        currentHealth += 16.67f;
        extraHeartsBackground[0].gameObject.SetActive(true);

        if(heartsCollected == 2)
        {
            extraHeartsBackground[1].gameObject.SetActive(true);
        }

        if (heartsCollected == 3)
        {
            extraHeartsBackground[2].gameObject.SetActive(true);
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }
}
