using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelGoalTrigger : MonoBehaviour
{
    private bool completed;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (completed || !other.CompareTag("Player"))
            return;

        LevelManager manager = FindObjectOfType<LevelManager>();
        if (manager == null)
            return;

        completed = true;
        manager.CompleteLevel();
    }
}
