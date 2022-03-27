using UnityEngine;

public class Targets : MonoBehaviour
{

    public float health = 1f;

    public void TargetHit()
    {
        health -= 1f;

        if (health <= 0f)
        {
            Kill();
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
