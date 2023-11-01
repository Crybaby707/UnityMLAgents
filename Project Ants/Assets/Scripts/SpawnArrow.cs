using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnArrow : MonoBehaviour
{
    public GameObject arrowPrefab;  // Префаб стріли
    public List<Transform> spawnPoints;    // Позиція, з якої буде створюватися стріла
    public Transform player;        // Посилання на гравця
    [SerializeField] public int speed;
    [SerializeField] public int wind;
    public float MaxSpawnInterval; // Інтервал спавну стріл
    public float MinSpawnInterval;
    private float currentSpawnInterval;
    public float timer = 0f; // Лічильник для відстеження часу
    public GeneralScript generalScript;
    public TrainingAgent _trainingAgent;

    private void Awake()
    {
       currentSpawnInterval = Random.Range(MinSpawnInterval, MaxSpawnInterval);
    }

    private void Update()
    {
        // Збільшуємо лічильник часу на кожному кадрі
        timer += Time.deltaTime;
        // Перевіряємо, чи пройшов встановлений інтервал часу
        if (timer >= currentSpawnInterval)
        {
            // Спавнимо стрілу
            ArrowSpawn();

            // Скидаємо лічильник часу
            timer = 0f;
            currentSpawnInterval = Random.Range(MinSpawnInterval, MaxSpawnInterval);
        }
    }

    public void ArrowSpawn()
    {
        // Вибираємо випадкову позицію зі списку spawnPoints
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Створюємо нову стрілу з префабу на вибраній позиції
        GameObject newArrow = Instantiate(arrowPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        // Додаємо скрипт виявлення зіткнення до стріли
        ArrowCollision arrowCollision = newArrow.AddComponent<ArrowCollision>();
        arrowCollision.generalScript = generalScript;
        arrowCollision._trainingAgent = _trainingAgent;

        arrowCollision.SetSpawner(this);  // Передаємо посилання на поточний спавнер стріл
        Vector2 spawnPosition = player.position + Vector3.up * 2f;

        // Обчислюємо напрямок руху стріли спрямовано до гравця
        Vector2 direction = (player.position + Vector3.up * wind - randomSpawnPoint.position).normalized;

        // Запускаємо стрілу, якщо вона має компонент Rigidbody
        Rigidbody2D arrowRigidbody = newArrow.GetComponent<Rigidbody2D>();
        if (arrowRigidbody != null)
        {
            arrowRigidbody.velocity = direction * speed;  // Приклад швидкості руху стріли

            
        }
        _trainingAgent._arrowtransform.Add(newArrow.transform);

    }
}

public class ArrowCollision : MonoBehaviour
{
    public GeneralScript generalScript;
    private SpawnArrow spawner;  // Посилання на спавнер стріл
    public TrainingAgent _trainingAgent;

    private void Awake()
    {
        //_trainingAgent = FindObjectOfType<TrainingAgent>(); // Отримайте посилання на JumpController
    }


    public void SetSpawner(SpawnArrow spawnArrow)
    {
        spawner = spawnArrow;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Перевіряємо, чи зіткнулася стріла з гравцем
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

        // Знищуємо стрілу після зіткнення
        Destroy(gameObject);
        _trainingAgent._arrowtransform.Remove(transform);
        _trainingAgent.ArrowAvoid();
    }
}