using UnityEngine;
using System;
using System.Collections;
using System.Linq;

[Serializable]
public class UnitStats
{
    public int maxHealth;
    public int attackDamage;
    public float attackInterval;
    public float speed;
    public int resourceCost;
}

[Serializable]
public class MovingStats
{
    public Vector3 startPos;
    public Vector3 destination;
    public float totalTime;
    public float startTime;
    public Node targetNode;
    public Node prevNode;
}

public class UnitBase : MonoBehaviour, IDamageable
{
    public UnitStats stats;
    public UnitType type;

    [Header("Used in test:")]
    public Node firstTargetNode;

    [Header("Used in code:")]
    public bool moveAlongPath = true;
    public bool isSlowedDown = false;
	public int numOfBoost = 0;
	public float lastBoostSpeed = 0f;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    public int health;
    public float currentSpeed;

    public int creationOrder;

    private Coroutine attackloop;
    private Coroutine changeSpeedCoroutine;

    private MovingStats movingStats;
    private SpriteAnimator sa;
    private SpriteRenderer spriteRend;

    public GameObject Target { get { return this.target; } }
    public int Health { get { return health; } }
    public int MaxHealth { get { return stats.maxHealth; } }

	// For Portal/Tunnel
	private static float portalAnimationDuration = 0.3f;
	private enum PortalAnimationState { none, disappearing, appearing };
	private PortalAnimationState portalAnimationState;
	private Vector3 originalScale;
	private float portalAnimationStartTime;
	private Node portalTarget;

    void Awake()
    {
        SetUnitStats();
        //ApplyUpgrades();
        health = stats.maxHealth;
        movingStats = new MovingStats();
        sa = GetComponent<SpriteAnimator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void Start() {
        //firstTargetNode = Spawn.S.firstNode;
        currentSpeed = stats.speed;
        StartMove(firstTargetNode);
		portalAnimationState = PortalAnimationState.none;
		originalScale = transform.localScale;
    }

    void FixedUpdate()
    {
        if (moveAlongPath)
        {
            MoveAlongPath();
        }

		// Portal Animation
		switch (portalAnimationState) {
		case PortalAnimationState.disappearing:
			float u = (Time.time - portalAnimationStartTime) / portalAnimationDuration;
			if (u > 1) {
				portalAnimationState = PortalAnimationState.appearing;
				transform.position = portalTarget.transform.position;
				StartMove (portalTarget);
				u = 1;
			}
			transform.localScale = originalScale * (1 - u);
			break;
		case PortalAnimationState.appearing:
			u = (Time.time - portalAnimationStartTime) / portalAnimationDuration - 1;
			if (u > 1) {
				portalAnimationState = PortalAnimationState.none;
				u = 1;
			}
			transform.localScale = originalScale * (u);
			break;
		default:
			break;
		}
    }

    public event Action<GameObject> Attack;

    public void TargetEnemy(GameObject enemy)
    {
        //if (this.HasTarget) // If already have a target
        //{
        //    float currDist = (this.target.transform.position - this.transform.position).magnitude;
        //    float newDist = (enemy.transform.position - this.transform.position).magnitude;
        //    if (currDist < newDist)
        //        return;
        //}
        if (enemy == this.target)
            return;

        this.target = enemy;

        if (this.attackloop == null)
        {
            this.attackloop = StartCoroutine(AttackLoop());
        }
    }

    public void UntargetEnemy()
    {
        if (this.attackloop != null)
        {
            StopCoroutine(attackloop);
        }

        DoStopAttacking();
    }

    public void StartMove(Node targetNode)
    {
        movingStats.prevNode = movingStats.targetNode;

        movingStats.targetNode = targetNode;
        movingStats.startTime = Time.time;
        movingStats.startPos = transform.position;
        movingStats.destination = targetNode.gameObject.transform.position;
        //movingStats.totalTime = (movingStats.destination - movingStats.startPos).magnitude / stats.speed;
        movingStats.totalTime = (movingStats.destination - movingStats.startPos).magnitude / currentSpeed;
        moveAlongPath = true;
    }

	public void PortalTo (Node targetNode) {
		portalTarget = targetNode;
		portalAnimationState = PortalAnimationState.disappearing;
		portalAnimationStartTime = Time.time;
	}

    public void changeSpeed(float amountOfChange, float duration) {
		if (amountOfChange <= 0) {
			spriteRend.color = Color.green;
			this.changeSpeedCoroutine = StartCoroutine (speedRoutine (amountOfChange, duration, "PoisonParticle"));
		} else {
			spriteRend.color = Color.red;
			if (numOfBoost != 0) {
				currentSpeed -= lastBoostSpeed;
			}
			numOfBoost++;
			lastBoostSpeed = amountOfChange;
			this.changeSpeedCoroutine = StartCoroutine (speedRoutine (amountOfChange, duration, "BoostParticle"));
		}
    }

	private IEnumerator speedRoutine(float amountOfChange, float duration, string particle) {
        currentSpeed += amountOfChange;
        //print("Changed speed by: " + amountOfChange);
        PauseMove();
        ContinueMove();
		if (amountOfChange <= 0) {
			isSlowedDown = true;
		}
        yield return new WaitForSeconds(duration);
		if (amountOfChange <= 0) {
			currentSpeed -= amountOfChange;
			spriteRend.color = Color.white;
			isSlowedDown = false;
		} else {
			if (--numOfBoost == 0) {
				currentSpeed -= amountOfChange;
				spriteRend.color = Color.white;
			}
		}
        PauseMove();
        ContinueMove();
        
		Destroy(this.gameObject.transform.Find(particle).gameObject);
        //print("Changed speed by: " + -amountOfChange);
    }

    public void PauseMove()
    {
        moveAlongPath = false;
    }

    public void ContinueMove()
    {
        // If already moving
        if (this.moveAlongPath == true)
            return;
        
        // Pause slightly if you are on top of someone in front of you
        if (NeedsPause() && this.type != UnitType.Balloon)
        {
            Invoke("ContinueMove", 0.25f);
            return;
        }

        movingStats.startTime = Time.time;
        movingStats.startPos = transform.position;
        //movingStats.totalTime = (movingStats.destination - movingStats.startPos).magnitude / stats.speed;
        movingStats.totalTime = (movingStats.destination - movingStats.startPos).magnitude / currentSpeed;
        moveAlongPath = true;
    }

    private void MoveAlongPath()
    {
        float u = (Time.time - movingStats.startTime) / movingStats.totalTime;
        if (u >= 1f)
        {
            transform.position = movingStats.destination;
            Node nextNode = movingStats.targetNode.GetNextNode();
            if (nextNode != null)
            {
                StartMove(nextNode);
            }
            else
            {
                // Walk back the way it came, so it doesn't get stuck at dead ends
                if (movingStats.prevNode != null && !movingStats.targetNode.isFinish)
                    StartMove(movingStats.prevNode);
            }
        }
        else
        {
            Vector3 newPos = movingStats.startPos * (1f - u) + movingStats.destination * u;
            if (sa != null)
            {
                sa.lastMove = movingStats.destination - movingStats.startPos;
            }
            transform.position = newPos;
        }
    }

    private IEnumerator AttackLoop()
    {
        //yield return new WaitForSeconds(this.stats.attackInterval);
        while (HasTarget)
        {
            FireAttack();

            if (!HasTarget) // If we killed the target with the last attack maybe
                break;

            yield return new WaitForSeconds(this.stats.attackInterval);
        }

        DoStopAttacking();
    }

    private void FireAttack()
    {
        Action<GameObject> temp = Attack;
        if (temp != null)
        {
            temp(this.target);
        }
    }

    private void DoStopAttacking()
    {
        this.target = null;
        ContinueMove();
        this.attackloop = null;
    }
    public void Heal(int amount)
    {
        if(health + amount > MaxHealth)
        {
            health = MaxHealth;
        }
        else
        {
            health = health + amount;
        }
    }
    public void AttackBoost(int amount)
    {
        stats.attackDamage += amount;
        spriteRend.color = Color.red;
    }
    public void UndoAttackBoost(int amount)
    {
        stats.attackDamage -= amount;
        spriteRend.color = Color.white;
    }
    public void receiveDamage(int amount)
    {
        if(type == UnitType.Balloon)
        {
            UI.S.PlaySound("FlyingHurt");
        }
        else if(type == UnitType.Archer)
        {
            UI.S.PlaySound("FemaleHurt");
        }
        else
        {
            UI.S.PlaySound("MaleHurt");
        }
        this.health -= amount;
        HitText.Create(this.transform.position, amount);
        if (this.health <= 0)
        {
            //Death
            Destroy(this.gameObject);
        }
    }

    private bool HasTarget
    {
        get { return target != null && target.activeInHierarchy; }
    }

    private void ApplyUpgrades()
    {
        int level = Upgrades.GetLevel(this.type);
        this.stats.maxHealth += 5 * level;
        this.stats.attackDamage += 3 * level;
    }

    private void SetUnitStats() {
        if (Upgrade.S == null)
            return;

        if (type == UnitType.UnitBasic || type == UnitType.Archer) {
            stats.maxHealth = Upgrade.S.getUpgradedHealth(type);
            stats.attackDamage = Upgrade.S.getUpgradedDamage(type);
        }
        if (type == UnitType.Balloon) {
            stats.maxHealth = Upgrade.S.getUpgradedHealth(type);
            stats.speed = Upgrade.S.getUpgradedSpeed(type);
        }
    }

    // Uses Physics.OverlapBox to get the other units its overlapping with. Returns
    //      true if it is overlapping with units that were before it
    private bool NeedsPause()
    {
        Collider coll = this.GetComponent<Collider>();
        if (coll == null)
            return false;

        int unitLayer = LayerMask.GetMask("Units");
        Collider[] otherColliders = Physics.OverlapBox(coll.bounds.center, coll.bounds.extents, Quaternion.identity, unitLayer);

        if (otherColliders.Length == 0)
            return false;

        var units = otherColliders.Select(c => c.GetComponent<UnitBase>()).Where(ub => ub != null);
        return units.Any(ub => ub.creationOrder < this.creationOrder);
    }


}
