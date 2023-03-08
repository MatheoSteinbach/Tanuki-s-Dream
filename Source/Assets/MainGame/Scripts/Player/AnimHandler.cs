using UnityEngine;
using UnityEngine.U2D.Animation;

public class AnimHandler : MonoBehaviour
{
    private enum State { Normal, Rolling, Attacking }
    private enum Skin { Normal, Froggo, Kappa}

    [Header("Normal Tanuki")]
    [SerializeField] GameObject tanukiFront;
    [SerializeField] GameObject tanukiBack;

    [Header("Froggo Tanuki")]
    [SerializeField] GameObject froggoFront;

    [Header("Kappa Tanuki")]
    [SerializeField] GameObject kappaFront;

    [Header("Sprite Resolver")] 
    [SerializeField] SpriteResolver spriteResolver;

    private Skin skin;

    private Animator tanukiFrontAnimator;
    private Animator tanukiBackAnimator;

    private bool isFront = false;
    private Vector3 lookDir = Vector3.zero;
    private string currentState;

    private bool isMoving = false;
    private Vector3 moveDir;
    private Vector3 rollDir;
    private Vector3 lastMoveDir;
    
    private bool canMove = true;

    const string FRONT_IDLE = "Tanuki_Idle";
    const string FRONT_WALK = "Tanuki_Walk";
    const string FRONT_ATTACK = "Tanuki_Attack";
    const string FRONT_THROW = "Tanuki_Throw";

    const string BACK_IDLE = "TanukiBack_Idle";
    const string BACK_WALK = "TanukiBack_Walk";
    const string BACK_ATTACK = "TanukiBack_Attack";
    const string BACK_THROW = "TanukiBack_Throw";

    const string FRONT_HURT = "Tanuki_Hurt";
    const string BACK_HURT = "TanukiBack_Hurt";

    const string FRONT_DEAD = "Tanuki_Death";
    const string BACK_DEAD = "TanukiBack_Death";

    private bool isAttacking = false;

    private void Awake()
    {
        tanukiFrontAnimator = tanukiFront.GetComponent<Animator>();
        tanukiBackAnimator = tanukiBack.GetComponent<Animator>();
    }
    private void Start()
    {
        tanukiBack.SetActive(false);
        isFront = true;
        skin = Skin.Normal;
        spriteResolver.SetCategoryAndLabel("Leaf", "Normal");
        spriteResolver.ResolveSpriteToSpriteRenderer();
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        if(isFront)
        {
            tanukiFrontAnimator.Play(newState);
        }
        else
        {
            tanukiBackAnimator.Play(newState);
        }
    }

    private void Update()
    {
        if (!isAttacking)
        {
            if (lastMoveDir.x < 0f)
            {
                lookDir = new Vector3(2, 2, 2);
                switch (skin)
                {
                    case Skin.Normal:
                        tanukiFront.transform.localScale = lookDir;
                        tanukiBack.transform.localScale = lookDir;
                        break;
                    case Skin.Froggo:
                        froggoFront.transform.localScale = lookDir;
                        tanukiBack.transform.localScale = lookDir;
                        break;
                    case Skin.Kappa:
                        kappaFront.transform.localScale = lookDir;
                        tanukiBack.transform.localScale = lookDir;
                        break;
                    default:
                        break;
                }
            }
            else if(lastMoveDir.x > 0f)
            {
                lookDir = new Vector3(-2, 2, 2);
                switch (skin)
                {
                    case Skin.Normal:
                        tanukiFront.transform.localScale = lookDir;
                        tanukiBack.transform.localScale = lookDir;
                        break;
                    case Skin.Froggo:
                        froggoFront.transform.localScale = lookDir;
                        tanukiBack.transform.localScale = lookDir;
                        break;
                    case Skin.Kappa:
                        kappaFront.transform.localScale = lookDir;
                        tanukiBack.transform.localScale = lookDir;
                        break;
                    default:
                        break;
                }
            }

            if (lastMoveDir.y > 0f)
            {
                tanukiBack.SetActive(true);
                tanukiFront.SetActive(false);
                froggoFront.SetActive(false);
                kappaFront.SetActive(false);
                switch (skin)
                {
                    case Skin.Normal:
                        //tanukiBackAnimator.enabled = false;
                        spriteResolver.SetCategoryAndLabel("Leaf", "Normal");
                        spriteResolver.ResolveSpriteToSpriteRenderer();
                        //tanukiBackAnimator.enabled = true;
                        break;
                    case Skin.Froggo:

                        spriteResolver.SetCategoryAndLabel("Leaf", "Froggo");
                        spriteResolver.ResolveSpriteToSpriteRenderer();
                        break;
                    case Skin.Kappa:
                        
                        spriteResolver.SetCategoryAndLabel("Leaf", "Kappa");
                        spriteResolver.ResolveSpriteToSpriteRenderer();
                        break;
                    default:
                        break;
                }

                isFront = false;
            }
            else
            {
                switch (skin)
                {
                    case Skin.Normal:
                        tanukiFrontAnimator = tanukiFront.GetComponent<Animator>();
                        tanukiBack.SetActive(false);
                        tanukiFront.SetActive(true);
                        froggoFront.SetActive(false);
                        kappaFront.SetActive(false);
                        isFront = true;
                        break;
                    case Skin.Froggo:
                        tanukiFrontAnimator = froggoFront.GetComponent<Animator>();
                        tanukiBack.SetActive(false);
                        tanukiFront.SetActive(false);
                        froggoFront.SetActive(true);
                        kappaFront.SetActive(false);
                        isFront = true;
                        break;
                    case Skin.Kappa:
                        tanukiFrontAnimator = kappaFront.GetComponent<Animator>();
                        tanukiBack.SetActive(false);
                        tanukiFront.SetActive(false);
                        froggoFront.SetActive(false);
                        kappaFront.SetActive(true);
                        isFront = true;
                        break;
                    default:
                        break;
                }

            }
        }
    }
    public void SetLastMoveDir(Vector3 _lastMoveDir)
    {
        lastMoveDir = _lastMoveDir;
    }
    
    public void PlayIdle(Vector3 _lastMoveDir)
    {
        lastMoveDir = _lastMoveDir;

        if (isFront)
        {
            ChangeAnimationState(FRONT_IDLE);
        }
        else
        {
            ChangeAnimationState(BACK_IDLE);
        }
    }
    public void PlayWalk(Vector3 _lastMoveDir)
    {
        lastMoveDir = _lastMoveDir;
        if (isFront)
        {
            ChangeAnimationState(FRONT_WALK);
        }
        else
        {
            ChangeAnimationState(BACK_WALK);
        }
    }
    public void PlayDead(Vector3 _lastMoveDir)
    {
        lastMoveDir = _lastMoveDir;
        if(isFront)
        {
            tanukiFrontAnimator.Play(FRONT_DEAD);
        }
        else
        {
            tanukiBackAnimator.Play(BACK_DEAD);
        }
    }
    public void PlayRoll(Vector3 _lastMoveDir)
    {
        lastMoveDir = _lastMoveDir;
        if (isFront)
        {
            tanukiFrontAnimator.Play("Tanuki_Roll");
        }
        else
        {
            tanukiBackAnimator.Play("TanukiBack_Roll");
        }
    }
    public void StopAttacking(Vector3 _lastMoveDir)
    {
        lastMoveDir = _lastMoveDir;
        isAttacking = false;
    }
    public void PlayAttack(Vector3 _lastMoveDir)
    {
        lastMoveDir = _lastMoveDir;
        isAttacking = true;
        if (lastMoveDir.x < 0f)
        {
            lookDir = new Vector3(2, 2, 2);
            switch (skin)
            {
                case Skin.Normal:
                    tanukiFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                case Skin.Froggo:
                    froggoFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                case Skin.Kappa:
                    kappaFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                default:
                    break;
            }
            
        }
        else if(lastMoveDir.x > 0f)
        {
            lookDir = new Vector3(-2, 2, 2);
            switch (skin)
            {
                case Skin.Normal:
                    tanukiFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                case Skin.Froggo:
                    froggoFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                case Skin.Kappa:
                    kappaFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                default:
                    break;
            }
            
        }

        if (lastMoveDir.y > 0f)
        {
            tanukiBack.SetActive(true);
            tanukiFront.SetActive(false);
            froggoFront.SetActive(false);
            kappaFront.SetActive(false);
            isFront = false;
        }
        else
        {
            switch (skin)
            {
                case Skin.Normal:
                    tanukiFrontAnimator = tanukiFront.GetComponent<Animator>();
                    tanukiBack.SetActive(false);
                    tanukiFront.SetActive(true);
                    froggoFront.SetActive(false);
                    kappaFront.SetActive(false);
                    isFront = true;
                    break;
                case Skin.Froggo:
                    tanukiFrontAnimator = froggoFront.GetComponent<Animator>();
                    tanukiBack.SetActive(false);
                    tanukiFront.SetActive(false);
                    froggoFront.SetActive(true);
                    kappaFront.SetActive(false);
                    isFront = true;
                    break;
                case Skin.Kappa:
                    tanukiFrontAnimator = kappaFront.GetComponent<Animator>();
                    tanukiBack.SetActive(false);
                    tanukiFront.SetActive(false);
                    froggoFront.SetActive(false);
                    kappaFront.SetActive(true);
                    isFront = true;
                    break;
                default:
                    break;
            }
        }

        
        if (isFront)
        {
            ChangeAnimationState(FRONT_ATTACK);
        }
        else
        {
            ChangeAnimationState(BACK_ATTACK);
        }
    }

    public void PlayThrow(Vector3 _lastMoveDir)
    {
        lastMoveDir = _lastMoveDir;
        isAttacking = true;
        if (lastMoveDir.x < 0f)
        {
            lookDir = new Vector3(2, 2, 2);
            switch (skin)
            {
                case Skin.Normal:
                    tanukiFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                case Skin.Froggo:
                    froggoFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                case Skin.Kappa:
                    kappaFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                default:
                    break;
            }

        }
        else if (lastMoveDir.x > 0f)
        {
            lookDir = new Vector3(-2, 2, 2);
            switch (skin)
            {
                case Skin.Normal:
                    tanukiFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                case Skin.Froggo:
                    froggoFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                case Skin.Kappa:
                    kappaFront.transform.localScale = lookDir;
                    tanukiBack.transform.localScale = lookDir;
                    break;
                default:
                    break;
            }

        }

        if (lastMoveDir.y > 0f)
        {
            tanukiBack.SetActive(true);
            tanukiFront.SetActive(false);
            froggoFront.SetActive(false);
            kappaFront.SetActive(false);
            isFront = false;
        }
        else
        {
            switch (skin)
            {
                case Skin.Normal:
                    tanukiFrontAnimator = tanukiFront.GetComponent<Animator>();
                    tanukiBack.SetActive(false);
                    tanukiFront.SetActive(true);
                    froggoFront.SetActive(false);
                    kappaFront.SetActive(false);
                    isFront = true;
                    break;
                case Skin.Froggo:
                    tanukiFrontAnimator = froggoFront.GetComponent<Animator>();
                    tanukiBack.SetActive(false);
                    tanukiFront.SetActive(false);
                    froggoFront.SetActive(true);
                    kappaFront.SetActive(false);
                    isFront = true;
                    break;
                case Skin.Kappa:
                    tanukiFrontAnimator = kappaFront.GetComponent<Animator>();
                    tanukiBack.SetActive(false);
                    tanukiFront.SetActive(false);
                    froggoFront.SetActive(false);
                    kappaFront.SetActive(true);
                    isFront = true;
                    break;
                default:
                    break;
            }
        }


        if (isFront)
        {
            ChangeAnimationState(FRONT_THROW);
        }
        else
        {
            ChangeAnimationState(BACK_THROW);
        }
    }

    public void ActivateNormal()
    {
        skin = Skin.Normal;
    }

    public void ActivateFroggo()
    {
        skin = Skin.Froggo;
    }

    public void ActivateKappa()
    {
        skin = Skin.Kappa;
    }


    //private void OnAttack()
    //{
    //    state = State.Attacking;
    //    canMove = false;
    //    StartCoroutine(EndOfAttack());
    //}
    //private IEnumerator EndOfAttack()
    //{
    //    yield return new WaitForSeconds(1f);
    //    canMove = true;
    //    state = State.Normal;
    //}
}
