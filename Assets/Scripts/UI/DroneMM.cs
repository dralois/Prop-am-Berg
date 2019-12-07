using UnityEngine;

public class DroneMM : MonoBehaviour
{
	public float maxHeight;
	public float minHeight;

	void Update()
	{
		float hoverHeight = (maxHeight + minHeight) / 2.0f;
		float hoverRange = maxHeight - minHeight;
		float hoverSpeed = 10.0f;

		this.transform.position = Vector3.up * (hoverHeight + Mathf.Cos(Time.time * hoverSpeed / 10f) * hoverRange) + new Vector3(0f, 0, -2f);
	}

}
