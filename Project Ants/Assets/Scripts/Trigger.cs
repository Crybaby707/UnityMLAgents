using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger : MonoBehaviour
{
    public TrainingAgent _trainingAgent;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            // Код, що виконується при зіткненні об'єкта з тригером
            Debug.Log("Object entered the trigger.");
            _trainingAgent.LastDeath();
            //SceneReloader();

            // Завантажуємо поточну сцену знову


        }
        if (other.gameObject.CompareTag("Arrow"))
        {

            Destroy(other.gameObject);
            Transform arrowTransform = other.gameObject.transform;
            _trainingAgent._arrowtransform.Remove(arrowTransform);
            _trainingAgent.ArrowAvoid();

        }

    }

    static public void SceneReloader()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}
