using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerAnimator _animator;

    [Tooltip("Скорость передвижения персонажа. Не зависит от гравитации и прыжков.")]
    public float velocity = 5f;

    [Tooltip("Чем выше значение, тем выше сможет прыгнуть персонаж.")]
    public float jumpForce = 18f;

    [Tooltip("Время нахождения в воздухе. Чем выше значение, тем дольше персонаж будет зависать перед падением.")]
    public float jumpTime = 0.85f;

    [Space]

    [Tooltip("Сила притяжения, тянущая персонажа вниз. Изменение этого значения влияет на движение, прыжки и падение.")]
    public float gravity = 9.8f;

    float jumpElapsedTime = 0;
    bool isJumping = false;
    bool inputJump;

    private IInputService _input;

    void Start()
    {
        _input = AllServices.Single<IInputService>();
    }

    void Update()
    {
        inputJump = _input.JumpPressed;

        if (_characterController.isGrounded)        
            _animator.SetRun(_characterController.velocity.magnitude > 0.9f);        

        _animator.SetAir(_characterController.isGrounded == false);

        if (inputJump && _characterController.isGrounded)        
            isJumping = true;        

        HeadHittingDetect();
    }

    // После обработки ввода FixedUpdate отвечает за применение движения и действий персонажа
    private void FixedUpdate()
    {
        // Направление движения
        float directionX = _input.Axis.x * velocity * Time.deltaTime;
        float directionZ = _input.Axis.y * velocity * Time.deltaTime;
        float directionY = 0;

        // Обработка прыжка
        if (isJumping)
        {
            // Добавляем плавность и инерцию при подъёме.
            // При падении этого делать не нужно, так как гравитация сама обеспечивает плавность.
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

            // Таймер прыжка
            jumpElapsedTime += Time.deltaTime;

            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        // Добавляем влияние гравитации по оси Y
        directionY = directionY - gravity * Time.deltaTime;

        // ---------- Поворот персонажа ----------

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Связываем движение вперёд с осью Z, а движение вправо с осью X
        forward *= directionZ;
        right *= directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        // ---------- Конец обработки поворота ----------

        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = forward + right;

        Vector3 movement = verticalDirection + horizontalDirection;
        _characterController.Move(movement);
    }

    // Завершает прыжок, если персонаж ударился головой о препятствие
    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(_characterController.center);
        float hitCalc = _characterController.height / 2f * headHitDistance;

        // Раскомментируйте эту строку, чтобы увидеть луч проверки над головой персонажа
        // Debug.DrawRay(ccCenter, Vector3.up * headHeight, Color.red);

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }
}