using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PursuitePlayer : MonoBehaviour
{
	#region Private Members
	private NavMeshAgent agent;
	[HideInInspector]
	public Vector3 destinationPoint;
	private Coroutine pursuiteCoroutine;
	private Renderer renderer;
	#endregion

	#region Public Members
	public Vector3 LastVisiblePoint;
	#endregion

	#region Coroutines
	IEnumerator StartPursuite()
	{
		while (true)
		{
			agent.SetDestination(destinationPoint);
			transform.LookAt(destinationPoint);
			yield return null;
		}
	}
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		renderer = GetComponent<Renderer>();
		renderer.material.color = Color.red;
		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.autoBraking = false;
		pursuiteCoroutine = StartCoroutine(StartPursuite());
	}

	private void OnDisable()
	{
		StopCoroutine(pursuiteCoroutine);
		renderer.material.color = Color.blue;
	}
	#endregion
}
