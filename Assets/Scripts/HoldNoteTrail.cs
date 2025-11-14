using UnityEngine;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(SpriteRenderer))]
public class HoldNoteTrail : MonoBehaviour
{
    [Header("Input")]
    public KeyCode keyPress = KeyCode.DownArrow;

    [Header("Sprites")]
    public Sprite defaultSprite;
    public Sprite pressedSprite;

    [Header("Timing & Movement")]
    public float holdDuration = 3f;
    public float scrollSpeed = 4f;

    [Header("Behaviour")]
    public string activatorTag = "Activator";

    private SpriteRenderer SR;
    private TrailRenderer TR;
    private bool canBeActivated = false;
    private bool isHolding = false;
    private float holdTimer = 0f;
    private float originalTrailTime;
    private float frozenY;

    private Rigidbody2D rb2d;

    void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        TR = GetComponentInChildren<TrailRenderer>(); // get from child
        rb2d = GetComponent<Rigidbody2D>();

        if (TR != null)
        {
            TR.emitting = true;
            TR.autodestruct = false;
            TR.time = holdDuration;
            originalTrailTime = TR.time;

            // Ensure local space
            try
            {
                var prop = TR.GetType().GetProperty("useWorldSpace");
                if (prop != null)
                    prop.SetValue(TR, false, null);
            }
            catch { }
        }
    }

    void Update()
    {
        // Move downward if not holding
        if (!isHolding && rb2d == null)
            transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Activation logic
        if (canBeActivated && !isHolding)
        {
            if (Input.GetKeyDown(keyPress))
            {
                Collider2D activator = FindActivatorUnderneath();
                if (activator != null)
                {
                    BeginHold(activator);
                }
            }
        }

        // While holding
        if (isHolding)
        {
            if (Input.GetKey(keyPress))
            {
                holdTimer += Time.deltaTime;
                TR.time = Mathf.Lerp(originalTrailTime, 0f, holdTimer / Mathf.Max(0.0001f, holdDuration));

                if (holdTimer >= holdDuration * 0.55f)
                {
                    Debug.Log("Hold success.");
                    HoldSuccess();
                }
            }
            else if (Input.GetKeyUp(keyPress))
            {
                Debug.Log("Hold fail.");
                HoldFail();
            }
        }

        // Offscreen cleanup
        if (!isHolding && transform.position.y < -15f)
            Destroy(gameObject);
    }

    void LateUpdate()
    {
        if (isHolding)
            transform.position = new Vector3(transform.position.x, frozenY, transform.position.z);
    }

    private void BeginHold(Collider2D activator)
    {
        SR.sprite = pressedSprite;
        TR.emitting = true;
        isHolding = true;
        holdTimer = 0f;
        frozenY = transform.position.y;

        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.simulated = false;
        }
    }

    private void HoldSuccess()
    {
        isHolding = false;
        TR.emitting = false;
        SR.sprite = defaultSprite;
        Destroy(gameObject);
    }

    private void HoldFail()
    {
        isHolding = false;
        TR.emitting = false;
        SR.sprite = defaultSprite;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(activatorTag))
            canBeActivated = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(activatorTag))
            canBeActivated = false;
    }

    private Collider2D FindActivatorUnderneath()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.2f);
        foreach (var h in hits)
            if (h.CompareTag(activatorTag))
                return h;
        return null;
    }
}
