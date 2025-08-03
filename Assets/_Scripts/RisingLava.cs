using UnityEngine;
using TMPro;
public class RisingLava : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    [Header("Gameplay")]
    [SerializeField] float _startSpeed = 1f;
    [SerializeField] float _maxSpeed = 1.5f;
    [SerializeField] float _timeToMaxSpeed = 30f;
    Vector3 _initialPosition;

    bool _isRising = false;
    float _elapsedTime = 0f;

    void Start()
    {
        _isRising = true;
        _initialPosition = transform.position;
    }

    void Update()
    {
        if (_isRising == false) return;

        _elapsedTime += Time.deltaTime;

        // Speed interpolation
        float t = Mathf.Clamp01(_elapsedTime / _timeToMaxSpeed);
        float currentSpeed = Mathf.Lerp(_startSpeed, _maxSpeed, t);

        // Moving the lava upwards
        transform.position += currentSpeed * Time.deltaTime * Vector3.up;

        // Update score
        float height = Mathf.Floor(transform.position.y);
        if (height < 0) height = 0;
        scoreText.text = "Score: " + height.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillableByLava"))
        {
            _isRising = false;

            GameManager.Instance.ChangeState(GameState.Lose);
        }
    }

    public void ResetLava()
    {
        _isRising = true;
         transform.position = _initialPosition;
    }
}
