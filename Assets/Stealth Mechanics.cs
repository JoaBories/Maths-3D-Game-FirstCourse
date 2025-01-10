using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StealthMechanics : MonoBehaviour
{
    [SerializeField] private float enemyDetectionTreshold;
    [SerializeField] private float behindEnemyDetectionTreshold;
    [SerializeField] private float behindEnemyDetectionDistance;
    [SerializeField] private float sameDirectionDetectionTreshold;
    [SerializeField] private GameObject detectionFeedback;
    [SerializeField] private GameObject backstabFeedback;
    private int numberOfEnemyDetecting = 0;
    private bool canBackstab;

    private void Update()
    {

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            Vector3 distance = enemy.transform.position - transform.position;
            float dotProduct = DotProduct(distance.normalized, enemy.transform.forward.normalized);
            if (dotProduct <= -1+enemyDetectionTreshold && dotProduct >= -1)
            {
                numberOfEnemyDetecting++;
                Debug.Log("yo");
            }
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            if (numberOfEnemyDetecting == 0)
            {
                Vector3 distance = enemy.transform.position - transform.position;
                float dotProduct = DotProduct(distance.normalized, enemy.transform.forward.normalized);
                if (dotProduct >= 1 - behindEnemyDetectionTreshold && dotProduct <= 1 && distance.sqrMagnitude <= behindEnemyDetectionDistance * behindEnemyDetectionDistance)
                {
                    dotProduct = DotProduct(transform.forward, enemy.transform.forward);
                    if (dotProduct >= 1 - sameDirectionDetectionTreshold && dotProduct <= 1)
                    {
                        canBackstab = true;
                    }
                    else
                    {
                        canBackstab = false;
                    }
                } 
                else
                {
                    canBackstab = false;
                }
            } 
            else
            {
                canBackstab = false;
            }
        }
        
        detectionFeedback.SetActive(false);
        backstabFeedback.SetActive(false);

        if (numberOfEnemyDetecting > 0)
        {
            detectionFeedback.SetActive(true);
        }

        if (canBackstab)
        {
            backstabFeedback.SetActive(true);
        }
    }

    private float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
}
