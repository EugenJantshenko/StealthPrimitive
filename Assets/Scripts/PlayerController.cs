using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region Private Members
	[SerializeField]
	private float speed=20f;
	private float h;
	private float v;
	#endregion

	#region Private Members
	private void Update()
	{
		v = Input.GetAxisRaw("Vertical");
		h = Input.GetAxisRaw("Horizontal");
		transform.position = new Vector3(transform.position.x + (h * speed) * Time.deltaTime, transform.position.y, transform.position.z + (v * speed) * Time.deltaTime);
	}
	#endregion
}
