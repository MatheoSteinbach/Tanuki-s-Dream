using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Color = UnityEngine.Color;

public class AIController : MonoBehaviour
{
    [SerializeField] GameObject shadow;
    [Header("Audio")]
    [SerializeField] private AudioSource attackSFX;
    [Header("AnimHandler")]
    [SerializeField] private AIAnimHandler animHandler;
    private bool isFront = true;

    [Header("States")]
    [SerializeField] private AIState currentState;
    [SerializeField] private AIState remainState;
    [Header("Speed")]
    [SerializeField] private float speed = 5f;
    private bool isMoving = false;

    [Header("Patrol Points")]
    [SerializeField] private GameObject[] wanderPoints;

    private Transform player;
    private Vector3 nextDestination;

    [Header("OnHit")]
    [SerializeField] private bool isKnockable = true;
    [SerializeField] private float knockbackDistance = 2;
    [SerializeField] private float invunerabilityTime = 1f;
    private float hurtTimer;

    [Header("VFX")]
    [SerializeField] private Animator getHitVFX;
    [SerializeField] private Transform attackPivot;
    [SerializeField] private Transform rangedAttackPivot;
    [SerializeField] private Animator rangedAttackVFX;
    [SerializeField] private Animator attackVFX;
    [SerializeField] private Animator deathVFX;
    [SerializeField] private Animator spawnVFX;

    [Header("Art")]
    [SerializeField]
    [Range(0f, 1f)]
    float lerpTime;
    [SerializeField] private Color32 chargeColor;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Sprite deadSprite;
    //[SerializeField] private SpriteRenderer sprite;
    //[SerializeField] private SpriteRenderer attackSprite;
    [SerializeField] private GameObject frontModel;
    [SerializeField] private GameObject backModel;
    [SerializeField] private GameObject attackModel;
    private SpriteRenderer[] frontSprites;
    private SpriteRenderer[] backSprites;
    private Material originalMaterial;

    [Header("Waiting")]
    [SerializeField] private float waitingTime = 2f;
    private float waitingTimer;
    private bool isWaiting = false;

    [Header("Chase")]
    [SerializeField] private float distanceToChase = 6f;

    [Header("Attack")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float distanceToMeleeAttack = 4f;
    [SerializeField] private float distanceToRangedAttack = 6f;
    [SerializeField] private float attackRadius = 4f;
    [SerializeField] private float attackDistance = 14f;
    [SerializeField] private float attackTimer = 1f;
    private float attackingTimer;
    [SerializeField] private float attackRate = 2f;
    private float chargeTimer;
    private bool isAttacking = false;
    private Vector3 jumpPos = Vector3.zero;
    private bool hitOnPlayer = false;

    private Collider2D physicsCollider;
    private Rigidbody2D rb;
    private Vector3 lastDir = Vector3.zero;
    private bool gotHit = false;

    private Vector3 originalScale = Vector3.zero;

    [Header("Drops")]
    [SerializeField] private GameObject dropPrefab;
    private bool doOnce = false;

    public bool IsDead { get; private set; }

    private void Awake()
    {
        physicsCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        frontSprites = frontModel.GetComponentsInChildren<SpriteRenderer>();
        backSprites = backModel.GetComponentsInChildren<SpriteRenderer>();
        originalMaterial = frontSprites[0].material;
    }
    private void Start()
    {
        originalScale = frontModel.transform.localScale;
        if(wanderPoints.Length > 0)
        {
            FindNextDestination();
        }
        
        attackModel.SetActive(false);

    }

    private void Update()
    {
        currentState.RunState(this);
        // sprite flip logic
        
        if(lastDir.y > transform.position.y && !IsDead)
        {
            isFront = false;
            frontModel.SetActive(false);
            backModel.SetActive(true);
        }
        else if(lastDir.y < transform.position.y && !IsDead)
        {
            isFront = true;
            frontModel.SetActive(true);
            backModel.SetActive(false);
        }
        if(lastDir.x < transform.position.x)
        {
            frontModel.transform.localScale = originalScale;
            backModel.transform.localScale = originalScale;
        }
        else if(lastDir.x > transform.position.x)
        {
            frontModel.transform.localScale = new Vector3(- originalScale.x, originalScale.y, originalScale.z);
            backModel.transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }
    public void ChangeState(AIState newState)
    {
        if(newState != remainState)
        {
            currentState = newState;
        }
    }

    #region Actions
    public void FindNextDestination()
    {
        isWaiting = true;
        int randomIndex = Random.Range(0, wanderPoints.Length);
        nextDestination = wanderPoints[randomIndex].transform.position;
    }

    public void MoveTowardsDestination()
    {
        
        Vector2 newPosition = Vector2.MoveTowards(transform.position, nextDestination, Time.deltaTime * speed);
        lastDir = nextDestination;
        animHandler.PlayWalkAnim(isFront);
        rb.MovePosition(newPosition);

    }

    public void ChaseButKeepDistance()
    {
        // check if distance is lower than 6 -> if it is then move to the opposite direction until it isnt
        float distanceToAttack = Vector2.Distance(transform.position, nextDestination);
        if(distanceToAttack < 6f)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, nextDestination, Time.deltaTime * speed);
            rb.MovePosition(newPosition);
        }
        else
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, -nextDestination, Time.deltaTime * speed);
            rb.MovePosition(newPosition);
        }
        

    }

    public void UpdateNextDestinationTowardsPlayer()
    {
        if (player != null)
        {
            nextDestination = player.position;
        }
    }

    public void Waiting()
    {
        isWaiting = true;
        animHandler.PlayIdleAnim(isFront);
        rb.velocity = Vector2.zero;
        waitingTimer += Time.deltaTime;
        if(waitingTimer > waitingTime)
        {
            isWaiting = false;
            waitingTimer = 0;
        }
    }

    // ///////////////////////////////// MELEE ATTACK ////////////////////////////////////////
    public void MeleeAttack()
    {
        SetToOriginalMaterial();
        DetectColliders();
        attackingTimer += Time.deltaTime;
        SetColorToDefault();
        
        if (attackingTimer > attackTimer)
        {
            isAttacking = false;
            hitOnPlayer = false;
            attackingTimer = 0;
        }
    }
    // ///////////////////////////////// CHARGE MELEE ATTACK ////////////////////////////////////////
    public void ChargeMeleeAttack()
    {
        SetToOriginalMaterial();
        animHandler.PlayAttackAnim(isFront);
        chargeTimer += Time.deltaTime;
        if(!gotHit)
        {
            rb.velocity = Vector2.zero;
        }
        
        if(chargeTimer < 0.5f)
        {
            jumpPos = player.position;
            //if(player.position.x > transform.position.x)
            //{
            //    sprite.flipX = true;
            //    attackSprite.flipX = true;
            //}
            //else
            //{
            //    sprite.flipX = false;
            //    attackSprite.flipX = false;
            //}
        }
        if (chargeTimer > 0.5f && chargeTimer < attackRate)
        {
            if(isFront)
            {
                foreach (var sprite in frontSprites)
                {
                    sprite.color = Color.Lerp(sprite.color, chargeColor, lerpTime);
                }
            }
            else
            {
                foreach (var sprite in backSprites)
                {
                    sprite.color = Color.Lerp(sprite.color, chargeColor, lerpTime);
                }
            }
            

            attackModel.SetActive(true);

            var dirToAttack = (jumpPos - transform.position).normalized;
            var distance = dirToAttack * 6;
            attackModel.transform.localPosition = Vector2.MoveTowards(attackModel.transform.localPosition, distance, (speed*2)  * Time.deltaTime);

        }
        if (chargeTimer > attackRate)
        {
            hitOnPlayer = false;

            var dirToAttack = (jumpPos - transform.position).normalized;
            var distance = dirToAttack * attackDistance;
            
            rb.AddForce(distance, ForceMode2D.Impulse);

            float AngleRad = Mathf.Atan2(-jumpPos.y + attackPivot.position.y, -jumpPos.x + attackPivot.position.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            attackPivot.rotation = Quaternion.Euler(0, 0, AngleDeg);
            attackVFX.Play("DodgeAnim");
            attackSFX.Play();

            isAttacking = true;
            chargeTimer = 0;

            attackModel.transform.position = frontModel.transform.position;
            attackModel.SetActive(false);
        }
        else
        {
            SetColorToDefault();
        }
    }
    // ///////////////////////////////// CHARGE RANGED ATTACK ////////////////////////////////////////
    public void ChargeRangedAttack()
    {
        
        chargeTimer += Time.deltaTime;

        if (!gotHit)
        {
            rb.velocity = Vector2.zero;
        }
        
        if (chargeTimer < 1)
        {
            animHandler.PlayIdleAnim(isFront);
            jumpPos = player.position;
        }
        if (chargeTimer > 1f && chargeTimer < attackRate)
        {
            animHandler.PlayAttackAnim(isFront);

            if (isFront)
            {
                foreach (var sprite in frontSprites)
                {
                    sprite.color = Color.Lerp(sprite.color, chargeColor, lerpTime);
                }
            }
            else
            {
                foreach (var sprite in backSprites)
                {
                    sprite.color = Color.Lerp(sprite.color, chargeColor, lerpTime);
                }
            }
            
            var dirToAttack = (jumpPos - transform.position).normalized;

            float AngleRad = Mathf.Atan2(-jumpPos.y + rangedAttackPivot.position.y, -jumpPos.x + rangedAttackPivot.position.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            rangedAttackPivot.rotation = Quaternion.Euler(0, 0, AngleDeg + 90);
            rangedAttackVFX.Play("AttackIndicatorAnim");
            attackSFX.Play();

            attackModel.gameObject.SetActive(true);
            var distance = dirToAttack * 1.5f;
            //rangedAttackPivot.transform.localPosition = Vector2.MoveTowards(rangedAttackPivot.transform.localPosition, distance, (speed) * Time.deltaTime);
            rangedAttackPivot.transform.localPosition = distance;
        }

        if (chargeTimer > attackRate)
        {
            hitOnPlayer = false;
            var bullet = Instantiate(projectile, transform.position, transform.rotation);
            bullet.GetComponent<Projectile>().ShootNormalShot(jumpPos);
            isAttacking = true;
            chargeTimer = 0;

            attackModel.transform.position = frontModel.transform.position;
            attackModel.SetActive(false);
        }
    }
    // ///////////////////////////////// RANGED ATTACK ////////////////////////////////////////
    public void RangedAttack()
    {
        attackingTimer += Time.deltaTime;
        SetColorToDefault();
        

        if (attackingTimer > attackTimer)
        {
            isAttacking = false;
            attackingTimer = 0;
            //physicsCollider.enabled = true;

        }
    }


    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, attackRadius))
        {
            PlayerMovement2D health;
            if (health = collider.GetComponent<PlayerMovement2D>())
            {
                if(!hitOnPlayer)
                {
                    health.GetHit(16.67f, transform.gameObject);
                    hitOnPlayer = true;
                }
                
            }
        }
    }
    
    public void Hurt()
    {

        hurtTimer += Time.deltaTime;

        //sprite.material = flashMaterial;

        if (hurtTimer > invunerabilityTime)
        {
            hurtTimer = 0;
            gotHit = false;
            //sprite.material = originalMaterial;
            SetToOriginalMaterial();
            rb.velocity = Vector2.zero;
        }
    }

    public void Death()
    {
        if(!doOnce)
        {
            SetToOriginalMaterial();
            animHandler.PlayDeathAnim(isFront);
            StartCoroutine(StartDeathSequence());
            doOnce = true;
        }

        SetToOriginalMaterial();
        attackModel.SetActive(false);
        shadow.SetActive(false);
        rb.velocity = Vector3.zero;
        physicsCollider.enabled = false;

        
    }
    IEnumerator StartDeathSequence()
    {
        yield return new WaitForSeconds(2f);
        if (dropPrefab != null)
        {
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
        frontModel.SetActive(false);
        backModel.SetActive(false);
        deathVFX.Play("DeathAnim");
    }

    public void GetHit(GameObject other)
    {
        //animHandler.PlayHurtAnim(isFront);
        SetToFlashMaterial();
        gotHit = true;
        getHitVFX.Play("HitAnim");
        // knockback
        if(isKnockable)
        {
            Vector3 dirFromPlayer = (transform.position - other.transform.position).normalized;
            rb.AddForce(dirFromPlayer * knockbackDistance, ForceMode2D.Impulse);
        }
    }

    public void SetIsDeadToTrue()
    {
        IsDead = true;

    }

    public void SetColorToDefault()
    {
        //sprite.color = Color.Lerp(sprite.color, Color.white, lerpTime);
        if(frontModel)
        {
            foreach (var sprite in frontSprites)
            {
                sprite.color = Color.Lerp(sprite.color, Color.white, lerpTime);
            }
            foreach (var sprite in backSprites)
            {
                sprite.color = Color.Lerp(sprite.color, Color.white, lerpTime);
            }
        }
        else
        {

        }
        
    }
    #endregion

    #region Decisions
    public bool CloseToDestination()
    {
        float distance = Vector3.Distance(transform.position, nextDestination);
        if (distance <= 2f)
        {
            return true;
        }

        return false;
    }

    public bool CheckIfIsWaiting()
    {
        if(isWaiting)
        {
            return true;
        }

        return false;
    }

    public bool CheckIfIsAttacking()
    {
        if (isAttacking)
        {
            return true;
        }

        return false;
    }
    public bool CheckIfGotHit()
    {
        if(gotHit)
        {
            return true;
        }

        return false;
    }


    public bool CheckIfIsDead()
    {
        if(IsDead)
        {
            return true;
        }

        return false;
    }
    public bool PlayerInRangeToChase()
    {
        return PlayerInRangeTo(distanceToChase);
    }

    public bool PlayerInRangeToMeleeAttack()
    {
        return PlayerInRangeTo(distanceToMeleeAttack);
    }

    public bool PlayerInRangeToRangedAttack()
    {
        return PlayerInRangeTo(distanceToRangedAttack);
    }
    #endregion

    private bool PlayerInRangeTo(float range)
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer <= range)
        {
            return true;
        }

        return false;
    }

    private void HandleLookDir()
    {
        
    }

    public void Spawn()
    {
        spawnVFX.Play("SpawnVFX");
    }

    private void SetToFlashMaterial()
    {
        if (isFront)
        {
            foreach (var sprite in frontSprites)
            {
                sprite.material = flashMaterial;
            }
        }
        else
        {
            foreach (var sprite in backSprites)
            {
                sprite.material = flashMaterial;
            }
        }
    }
    private void SetToOriginalMaterial()
    {
        if (isFront)
        {
            foreach (var sprite in frontSprites)
            {
                sprite.material = originalMaterial;
            }
        }
        else
        {
            foreach (var sprite in backSprites)
            {
                sprite.material = originalMaterial;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = transform == null ? Vector3.zero : transform.position;
        Gizmos.DrawWireSphere(position, attackRadius);
    }

}
