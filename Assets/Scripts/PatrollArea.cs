using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatrollArea : MonoBehaviour
{
	#region Private Members
	[SerializeField]
	private Transform[] destinationPoints;
	private NavMeshAgent agent;
	private Coroutine patroll;
	public int index;
	#endregion

	#region Coroutines
	IEnumerator StartPatrolling()
	{
		while (true)
		{
			if (index < destinationPoints.Length - 1 && Vector3.Distance(agent.transform.position, destinationPoints[index].transform.position) < 1f)
			{
				index++;
			}
			else if (index == destinationPoints.Length - 1 && Vector3.Distance(agent.transform.position, destinationPoints[index].transform.position) < 1f)
			{
				index = 0;
			}
			agent.SetDestination(destinationPoints[index].position);
			transform.LookAt(destinationPoints[index]);
			yield return null;
		}
	}
	#endregion

	#region Private Methods
	private void OnEnable()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.autoBraking = false;
		patroll = StartCoroutine(StartPatrolling());
	}

	private void OnDisable()
	{
		StopCoroutine(patroll);
	}
	#endregion
}
