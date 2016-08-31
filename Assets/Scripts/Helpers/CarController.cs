using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

    private float xHalfWidth;
    private float yExtent;
    /// <summary>
    /// 1 is to right, -1 is to left
    /// </summary>
    public int direction = 0;
    public bool isActive = false;
    public int index = -1;
    private LevelSeventeenManager levelController;
    private Rigidbody2D body;

    public float ySpawnPos = 3.0f;
    public float xOffset = 2.0f;
    public float xOutOfBoundOffset = 2.5f;

    // Use this for initialization
    void Start () {

        levelController = gameObject.transform.parent.gameObject.GetComponent<LevelSeventeenManager>();
        Vector3 screenRes = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        xHalfWidth = screenRes.x;
        body = gameObject.GetComponent<Rigidbody2D>();
        yExtent = GetComponent<Collider2D>().bounds.extents.y;

    }

    public void SpawnCar()
    {
        float xPos = 0.0f;
        float yPos = ySpawnPos;

        if (direction == 1)
        {
            xPos = xHalfWidth + xOffset;
        }
        else if (direction == -1)
        {
            xPos = -xHalfWidth - xOffset;
        }

        gameObject.transform.position = new Vector3(xPos, yPos, 0);

        body.isKinematic = false;
        isActive = true;

    }

    private void ReturnObjectToPool()
    {
        body.isKinematic = true;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.isKinematic = true;
        isActive = false;
        gameObject.transform.rotation = Quaternion.identity;
        levelController.ReturnToPool(index);
    }

	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            if (transform.position.x > xHalfWidth + xOutOfBoundOffset || transform.position.x < -xHalfWidth - xOutOfBoundOffset)
            {
                ReturnObjectToPool();
            }
        }
    }
}
