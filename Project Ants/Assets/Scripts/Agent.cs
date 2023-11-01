using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UIElements;

public class TrainingAgent : Agent
{
    public GeneralScript generalScript;
    [SerializeField]
    private Transform _landTransform;
    public List<Transform> _arrowtransform = new();
    [SerializeField]
    private List<Transform> _arrowtransformCopy = new();
    public JumpController _jumpController;
    public SpawnArrow _spawnArrow;
    private Vector3 previousPosition;


    int i = 0;
    private void Awake()
    {
        //Time.timeScale = 0.5f;
    }

    public override void OnEpisodeBegin()
    {
        generalScript.PlayerLifes = 3;
        generalScript.GameScore = 0;
        transform.localPosition = new Vector3(1.53f, 0f, 0f);
        _spawnArrow.timer = 0;
        // Зупинка об'єкта
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        DeleteAllArrow();


    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(_landTransform.localPosition);
        sensor.AddObservation(generalScript.PlayerLifes);
        sensor.AddObservation(transform.localPosition);

        // Визначте поточну швидкість на основі розниці позицій
        Vector3 velocity = (transform.localPosition - previousPosition) / Time.deltaTime;

        // Додайте спостереження за швидкістю
        sensor.AddObservation(velocity);

        // Оновіть попередню позицію для наступного кроку
        previousPosition = transform.localPosition;


        foreach (Transform arrowTransform in _arrowtransform)
        {

            sensor.AddObservation(arrowTransform.localPosition);

        }

    }

    public void DeleteAllArrow()
    {
        for (int i = _arrowtransform.Count - 1; i >= 0; i--)
        {
            Transform arrowTransform = _arrowtransform[i];
            Destroy(arrowTransform.gameObject);
            _arrowtransform.RemoveAt(i);
        }
    }

    public void Update()
    {
        
        AddReward(generalScript.GameScore / 200);
        _arrowtransformCopy = _arrowtransform;


    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //base.OnActionReceived(actions); 
        int action = actions.DiscreteActions[0];
        i = action;
        //Debug.Log(action);

        if (action == 1)
        {
            //Debug.Log("Jump0");
            _jumpController.Jump();
            AddReward(0.5f);
        }
        else if (action == 2)
        {
            //Debug.Log("Jump1");
            _jumpController.RightJump();
        }
        else if (action == 3)
        {
            //Debug.Log("Jump2");
            _jumpController.LeftJump();
        }
    }

    public void Death() 
    {
        AddReward(-20f);
    }

    public void LastDeath()
    {
        AddReward(-60f);
        //generalScript.changeGui();
        EndEpisode();
    }
    
    public void ArrowAvoid()
    {
        AddReward(3f);
    }

    public void LifeTime()
    {
        
    }

    public void ArrowInFace()
    {
/*        AddReward(-10f);*/
    }
}
