using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offset;

	void Update ()
	{
        transform.position = new Vector3(target.position.x + offset, transform.position.y, transform.position.z);
	}
}
