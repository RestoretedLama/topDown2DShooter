using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class EnemyTestDamage : MonoBehaviour
{
    public float testDamage = 50f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetComponent<BaseEnemy>().TakeDamage(testDamage);
        }
    }
}
