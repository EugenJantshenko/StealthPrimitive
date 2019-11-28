using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	#region Private Members
	[SerializeField]
	private int rays = 8;
	[SerializeField]
	private int distance = 30;
	[SerializeField]
	private float angle = 40;
	[SerializeField]
	private float distanceToPlayer = 4f;
	[SerializeField]
	private float killDistance = 5f;
	[SerializeField]
	private Transform target;
	private bool playerKilled;
	private int currentWayPoint;
	private PursuitePlayer pursuitePlayer;
	private PatrollArea patrollArea;
	private Coroutine visiblePlayerCoroutine;
	#endregion

	#region Coroutines
	IEnumerator CheckPlayerVisible()
	{
		while (true)
		{
			currentWayPoint = patrollArea.index;
			if (Vector3.Distance(transform.position, target.position) < distance)
			{
				if (RayToScan() && target.GetComponent<PlayerController>().enabled)
				{
					playerKilled = false;
					StartPursuite();
				}
				else
				{
					if (Vector3.Distance(transform.position, pursuitePlayer.LastVisiblePoint) > distanceToPlayer)
					{
						GoToLastVisiblePoint();
					}
					else
					{
						ReturnToPatroll();
					}
				}
			}
			else if (transform != target && Vector3.Distance(transform.position, pursuitePlayer.LastVisiblePoint) < distanceToPlayer)
			{
				ReturnToPatroll();
			}
			yield return new WaitForFixedUpdate();
		}
	}
	#endregion

	#region Private Methods
	private void Start()
	{
		pursuitePlayer = this.GetComponent<PursuitePlayer>();
		patrollArea = this.GetComponent<PatrollArea>();
		playerKilled = false;
		visiblePlayerCoroutine = StartCoroutine(CheckPlayerVisible());
	}
	private bool GetRaycast(Vector3 dir)
	{
		bool result = false;
		RaycastHit hit = new RaycastHit();
		Vector3 pos = transform.position;
		if (Physics.Raycast(pos, dir, out hit, distance))
		{
			if (hit.transform == target)
			{
				result = true;
				Debug.DrawLine(pos, hit.point, Color.green);
			}
			else
			{
				Debug.DrawLine(pos, hit.point, Color.blue);
			}
		}
		else
		{
			Debug.DrawRay(pos, dir * distance, Color.red);
		}
		return result;
	}

	private bool RayToScan()
	{
		bool result = false;
		bool a = false;
		bool b = false;
		float j = 0;
		for (int i = 0; i < rays; i++)
		{
			var x = Mathf.Sin(j);
			var y = Mathf.Cos(j);

			j += angle * Mathf.Deg2Rad / rays;

			Vector3 dir = transform.TransformDirection(new Vector3(x, 0, y));
			if (GetRaycast(dir))
			{
				a = true;
			}

			if (x != 0)
			{
				dir = transform.TransformDirection(new Vector3(-x, 0, y));
				if (GetRaycast(dir))
				{
					b = true;
				}
			}
		}

		if (a || b)
		{
			result = true;
		}

		return result;
	}

	private void KillPlayer()
	{
		Renderer renderer = target.GetComponent<Renderer>();
		PlayerController contorller = target.GetComponent<PlayerController>();
		Rigidbody rigidbody = target.GetComponent<Rigidbody>();
		renderer.material.color = Color.gray;
		contorller.enabled = false;
		rigidbody.constraints = RigidbodyConstraints.None;
		target.transform.Rotate(new Vector3(90, 0, 0));
		playerKilled = true;
		ReturnToPatroll();
	}

	private void StartPursuite()
	{
		currentWayPoint = patrollArea.index;
		patrollArea.enabled = false;
		pursuitePlayer.destinationPoint = target.transform.position;
		pursuitePlayer.LastVisiblePoint = target.transform.position;
		pursuitePlayer.enabled = true;
		if (Vector3.Distance(transform.position, target.position) < killDistance)
		{
			if (!playerKilled)
			{

				KillPlayer();
			}
		}
	}

	private void ReturnToPatroll()
	{
		pursuitePlayer.enabled = false;
		patrollArea.index = currentWayPoint;
		patrollArea.enabled = true;
	}

	private void GoToLastVisiblePoint()
	{
		pursuitePlayer.destinationPoint = pursuitePlayer.LastVisiblePoint;
	}
	#endregion
}
