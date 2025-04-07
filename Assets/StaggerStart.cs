using Cysharp.Threading.Tasks;
using UnityEngine;

public class StaggerStart : MonoBehaviour {

    public Transform lookAt;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start() {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        transform.LookAt(lookAt, Vector3.up);

        await UniTask.Delay(Random.Range(250, 10001));
        animator.enabled = true;
    }
}
