using UnityEngine;

public class TryOnceMore : MonoBehaviour
{
	#region Private Members
	[SerializeField]
	private Transform[] spawnPoints;
	private PlayerController playerController;
	#endregion

	#region Private Methods
	private void Start()
	{
		playerController=GetComponent<PlayerController>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (playerController.enabled == false)
			{
				RespawnPlayer();
			}
		}
	}

	private void RespawnPlayer()
	{
		Renderer renderer = GetComponent<Renderer>();
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		renderer.material.color = Color.green;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
		transform.rotation = Quaternion.Euler(0,0,0);
		gameObject.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
		playerController.enabled = true;
	}
	#endregion
}
