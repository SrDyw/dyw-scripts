using UnityEngine;
using System.Collections;

public enum Vector
{
    NONE,
    X,
    Y,
    Z
}

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private Transform focus;
    [SerializeField]
    private Transform followTo;
    [SerializeField]
    [Range(0f, 1f)] private float followIntensity = 1;
    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float speedMovement = 5;
    [SerializeField]
    private Vector axisToBloq;

    private Vector2 shakeForce;
    private float shakeTime = 0.5f;

    public static CameraFollower instance;
    [SerializeField]
    [Tooltip("2D Only")]
    private BoxCollider2D clampToCollider;
    [SerializeField]
    private Vector2 cameraSize;

    private Camera camera;
    public Vector3 targetVelocity;
    private float defaultZposition;
    //< Cinematic Options
    private bool inCinematic = false;
    private int currentCP = 0;
    private Coroutine cinematikCoroutine;
    private Coroutine cinematicNext;
    private Vector2[] cinematicPoints;
    private float cinematicSpeed = 1;
    private InputController controller;
    private float directionX = 0;

    public Vector3 TargetVelocity { get => targetVelocity; set => targetVelocity = value; }
    public BoxCollider2D ClampToCollider { get => clampToCollider; set => clampToCollider = value; }

    public bool InCinematic { get => inCinematic; set => inCinematic = value; }

    private void Awake()
    {
        instance = (instance == null) ? this : instance;
        camera = GetComponent<Camera>();
    }

    void Start()
    {
        defaultZposition = transform.position.z;
        controller = Depedencies.Get<InputController>();
    }

    void Update()
    {
        if (!inCinematic)
        {
            directionX = controller.MovementInput.x != 0 ? controller.MovementInput.x : directionX;
            if (focus != null)
            {
                FocusCenter();
            }
            else if (followTo != null)
            {
                Vector3 shakeVector = new Vector3(shakeForce.x * Random.Range(-1, 1), shakeForce.y * Random.Range(-1, 1));
                Vector3 positionTarget = GeneratePositionOffset(transform.position);
                Vector3 clampedPosition = positionTarget;

                if (clampToCollider)
                {
                    clampedPosition = ClampPosition(positionTarget) + shakeVector;
                }

                transform.position = Vector3.Lerp(transform.position, clampedPosition, 0.2f);
                // Vector3 moveDir = (clampedPosition - transform.position).normalized;
                // transform.position += speedMovement * moveDir * Vector3.Distance(clampedPosition, transform.position) * 0.1f;
                transform.position = new Vector3(transform.position.x, transform.position.y, defaultZposition);
            }
        }
        else if (shakeTime > 0)
        {
            Vector3 shakeVector = new Vector3(shakeForce.x * Random.Range(-1, 1), shakeForce.y * Random.Range(-1, 1));
            if (cinematikCoroutine == null)
            {
                transform.position = (Vector3)cinematicPoints[currentCP] + shakeVector + Vector3.forward * defaultZposition;
            }
        }


        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if (shakeTime <= 0) shakeForce = Vector2.zero;
        }

    }

    Vector3 ClampPosition(Vector3 positionTarget)
    {
        Vector3 clampedPosition = positionTarget;

        float cameraSizeX = Camera.main.orthographicSize * Screen.width / Screen.height;

        if (clampToCollider)
        {
            clampedPosition = new Vector3(
                Mathf.Clamp(positionTarget.x, clampToCollider.bounds.min.x + cameraSizeX, clampToCollider.bounds.max.x - cameraSizeX),
                Mathf.Clamp(positionTarget.y, clampToCollider.bounds.min.y + camera.orthographicSize, clampToCollider.bounds.max.y - camera.orthographicSize),
                positionTarget.z
            );
        }

        var colliderSizeX = Mathf.Abs(clampToCollider.bounds.max.x - clampToCollider.bounds.min.x);
        var colliderSizeY = Mathf.Abs(clampToCollider.bounds.max.y - clampToCollider.bounds.min.y);

        if (colliderSizeX <= cameraSizeX * 2) clampedPosition.x = clampToCollider.bounds.min.x + cameraSizeX;
        if (colliderSizeY <= camera.orthographicSize * 2) clampedPosition.x = clampToCollider.bounds.min.y + colliderSizeY;


        return clampedPosition;
    }

    void FocusCenter()
    {
        float x = ((int)axisToBloq != 1) ? focus.position.x : transform.position.x;
        float y = ((int)axisToBloq != 2) ? focus.position.y : transform.position.y;
        float z = ((int)axisToBloq != 3) ? focus.position.z : transform.position.z;

        Vector3 vecToFollow = new Vector3(x, y, z);


        transform.position = ClampPosition(GeneratePositionOffset(vecToFollow));
    }

    Vector3 GeneratePositionOffset(Vector3 position)
    {

        float x = ((int)axisToBloq != 1) ? (followTo.position.x + offsetX * directionX - position.x) * followIntensity : 0;
        float y = ((int)axisToBloq != 2) ? (followTo.position.y - position.y) * followIntensity : 0;
        float z = ((int)axisToBloq != 3) ? (followTo.position.z - position.z) * followIntensity : 0;

        Vector3 vecOffset = new Vector3(x, y, z);

        return Vector3.Lerp(transform.position, position + vecOffset, Time.deltaTime * speedMovement);
    }

    public void AddShakeEffect(Vector2 shakeForce, float time = 0.25f)
    {
        this.shakeForce = shakeForce;
        shakeTime = time;
    }

    public void AddShakeEffect(float intensity, float time = 0.25f)
    {
        Vector2 shakeForce = new Vector2(
            Random.Range(intensity * .5f, intensity),
            Random.Range(intensity * .5f, intensity)
        );

        this.shakeForce = shakeForce;
        shakeTime = time;
    }

    public void InitCinematic(Vector2[] points, float speed)
    {
        inCinematic = true;
        cinematicPoints = points;
        cinematicSpeed = speed;

        cinematikCoroutine = StartCoroutine(MoveBtwPoints());
    }

    IEnumerator MoveBtwPoints()
    {

        while (Vector2.Distance(transform.position, cinematicPoints[currentCP]) > 0.1f)
        {
            Vector3 positionTarget = Vector2.Lerp(transform.position, cinematicPoints[currentCP], Time.deltaTime * cinematicSpeed);

            positionTarget.z = defaultZposition;
            transform.position = positionTarget;

            yield return new WaitForSeconds(0.01f);
            if (cinematicPoints == null) break;
        }
        cinematikCoroutine = null;
    }

    public void Next()
    {
        print("CAlled");
        if (inCinematic)
        {
            currentCP++;
            if (currentCP < cinematicPoints.Length)
            {
                if (cinematikCoroutine != null) StopCoroutine(cinematikCoroutine);
                cinematikCoroutine = StartCoroutine(MoveBtwPoints());
            }
            else
            {
                inCinematic = false;
                cinematicPoints = null;
                currentCP = 0;
            }
        }
        else
        {
            print("There are not a cinematic in progress!");
        }
    }

    public void Next(float time)
    {
        if (cinematicNext != null) StopCoroutine(cinematicNext);
        cinematicNext = StartCoroutine(NextAtTime(time));
    }

    IEnumerator NextAtTime(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Next();
    }
}
