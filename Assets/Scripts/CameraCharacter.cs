using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class CameraCharacter : MonoBehaviour
{
    public float SpawnHelper { get; set; } = 4.5f;
    public GameObject ObjPrefab { get; set; }
    public float BallForce { get; set; } = 700;
    public float BallCount { get; set; }

    private Camera cam;
    private bool playState;
    private Rigidbody rb;
    private BallManager ballManager;
    private int hitCount;
    
    public Image CursorImage { get; set; }
    public TextMeshProUGUI BallCountText { get; set; }
    public TextMeshProUGUI MainText { get; set; }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (playState)
        {
            UpdateCursorPosition();
            UpdateCameraMovement();
            TryInstantiateBall();
        }
    }
    
    void UpdateCursorPosition()
    {
        cursor.transform.position = Input.mousePosition;
    }
    
    void UpdateCameraMovement()
    {
        rb.velocity = new Vector3(0, 0, camMove);
    }
    
    void TryInstantiateBall()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        Vector3 ballInstantiatePoint = cam.ScreenToWorldPoint(new Vector3(mouseX, mouseY, cam.nearClipPlane + spawnHelper));
    
        if (Input.GetMouseButtonDown(0) && ballCount > 0)
        {
            InstantiateBall(ray.direction, ballInstantiatePoint);
        }
    }
    
    void InstantiateBall(Vector3 direction, Vector3 instantiatePoint)
    {
        ballCount--;
        GameObject ballInstance = Instantiate(objPrefab, instantiatePoint, Quaternion.identity);
        ballInstance.GetComponent<Rigidbody>().AddForce(direction * ballForce);
        ballCountText.text = ballCount.ToString();
    }

    private void Initialize()
    {
        ballManager = FindObjectOfType<BallManager>();
        cam = GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;

        playState = true;
        BallCountText.text = BallCount.ToString();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            playState = false;

            if (hitCount >= 10)
            {
                mainText.text = "Success!";
                mainText.transform.DOScale(1, 1).SetEase(Ease.OutBack);
            }
            else
            {
                mainText.text = "You Failed!";
                mainText.transform.DOScale(1, 1).SetEase(Ease.OutBack);
            }

            rb.isKinematic = true;
            Invoke(nameof(RestartLevel), 4);
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
}
