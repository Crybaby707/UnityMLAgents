using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnArrow : MonoBehaviour
{
    public GameObject arrowPrefab;  // ������ �����
    public List<Transform> spawnPoints;    // �������, � ��� ���� ������������ �����
    public Transform player;        // ��������� �� ������
    [SerializeField] public int speed;
    [SerializeField] public int wind;
    public float MaxSpawnInterval; // �������� ������ ����
    public float MinSpawnInterval;
    private float currentSpawnInterval;
    public float timer = 0f; // ˳������� ��� ���������� ����
    public GeneralScript generalScript;
    public TrainingAgent _trainingAgent;

    private void Awake()
    {
       currentSpawnInterval = Random.Range(MinSpawnInterval, MaxSpawnInterval);
    }

    private void Update()
    {
        // �������� �������� ���� �� ������� ����
        timer += Time.deltaTime;
        // ����������, �� ������� ������������ �������� ����
        if (timer >= currentSpawnInterval)
        {
            // �������� �����
            ArrowSpawn();

            // ������� �������� ����
            timer = 0f;
            currentSpawnInterval = Random.Range(MinSpawnInterval, MaxSpawnInterval);
        }
    }

    public void ArrowSpawn()
    {
        // �������� ��������� ������� � ������ spawnPoints
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // ��������� ���� ����� � ������� �� ������� �������
        GameObject newArrow = Instantiate(arrowPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        // ������ ������ ��������� �������� �� �����
        ArrowCollision arrowCollision = newArrow.AddComponent<ArrowCollision>();
        arrowCollision.generalScript = generalScript;
        arrowCollision._trainingAgent = _trainingAgent;

        arrowCollision.SetSpawner(this);  // �������� ��������� �� �������� ������� ����
        Vector2 spawnPosition = player.position + Vector3.up * 2f;

        // ���������� �������� ���� ����� ���������� �� ������
        Vector2 direction = (player.position + Vector3.up * wind - randomSpawnPoint.position).normalized;

        // ��������� �����, ���� ���� �� ��������� Rigidbody
        Rigidbody2D arrowRigidbody = newArrow.GetComponent<Rigidbody2D>();
        if (arrowRigidbody != null)
        {
            arrowRigidbody.velocity = direction * speed;  // ������� �������� ���� �����

            
        }
        _trainingAgent._arrowtransform.Add(newArrow.transform);

    }
}

public class ArrowCollision : MonoBehaviour
{
    public GeneralScript generalScript;
    private SpawnArrow spawner;  // ��������� �� ������� ����
    public TrainingAgent _trainingAgent;

    private void Awake()
    {
        //_trainingAgent = FindObjectOfType<TrainingAgent>(); // ��������� ��������� �� JumpController
    }


    public void SetSpawner(SpawnArrow spawnArrow)
    {
        spawner = spawnArrow;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // ����������, �� ��������� ����� � �������
        if (collision.gameObject.CompareTag("Player"))
        {
            generalScript.PlayerLifes = generalScript.PlayerLifes - 1; //1;
            //GeneralScript.changeGui();
            if (generalScript.PlayerLifes == 0)
            {
                Destroy(gameObject);
                _trainingAgent.LastDeath();
                //Trigger.SceneReloader();

            }
            _trainingAgent.Death();

            if (generalScript.GameScore <= 3)
            {
                generalScript.GameScore = 0;
                //Debug.Log("Ok");
            }
            else
            {
                generalScript.GameScore = generalScript.GameScore - 3;
                //Debug.Log("Not ok");
            }


        }

        // ������� ����� ���� ��������
        Destroy(gameObject);
        _trainingAgent._arrowtransform.Remove(transform);
        _trainingAgent.ArrowAvoid();
    }
}