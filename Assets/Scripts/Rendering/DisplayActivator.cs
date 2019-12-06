using UnityEngine;

[ExecuteAlways]
public class DisplayActivator : MonoBehaviour
{

	private void Start()
	{
		Debug.Log($"Display count: {Display.displays.Length}");
		foreach(Display curr in Display.displays)
		{
			curr.Activate();
		}
	}

}
