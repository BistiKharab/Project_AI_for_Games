using UnityEngine;

public class PlayerBehaviourScript : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField][Range(10.0f, 1000.0f)] float moveSpeed;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         float moveX = Input.GetAxis("Horizontal") * moveSpeed;
         float moveZ = Input.GetAxis("Vertical") * moveSpeed;

        transform.position += new Vector3(moveX * Time.deltaTime, 0, moveZ * Time.deltaTime);
    }
}
