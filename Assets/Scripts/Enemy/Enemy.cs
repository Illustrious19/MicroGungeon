using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;


    [Header("Blocked info")]
    public float blockDuration = 1;
    public Vector2 blockDirection = new Vector2(2,12);
    protected bool canBeBlocked;
    [SerializeField] protected GameObject counterImage;
    
    [Header("Stunned info")]
    public float stunDuration = 0.1f;
    public Vector2 stunDirection = new Vector2(2,4);
    public bool stunned = false;

    [Header("Move info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 2;
    public float battleTime = 7;
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float agroDistance = 2;
    public float attackDistance = 2;
    public float attackCooldown;
    public float minAttackCooldown = 1;
    public float maxAttackCooldown= 2;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFX fx { get; private set; }
    private Player player;
    public string lastAnimBoolName {  get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();

        fx = GetComponent<EntityFX>();
    }

    protected override void Update()
    {
        base.Update();


        stateMachine.currentState.Update();


    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;


    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimerCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeBlocked = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeBlocked = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeBlocked()
    {
        if (canBeBlocked)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }
    public virtual bool CanBeStunned(int AttackCount)
    {
        if (AttackCount == 2)
        {
            stunned = true;
        }
        else
        {
            stunned = false;
        }
        return stunned;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AnimationSpecialAttackTrigger()
    {

    }

    public virtual RaycastHit2D IsPlayerDetected()
    {
        RaycastHit2D playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
        RaycastHit2D wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsGround);

        if (wallDetected)
        {
            if (wallDetected.distance < playerDetected.distance)
                return default(RaycastHit2D);
        }

        return playerDetected;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
