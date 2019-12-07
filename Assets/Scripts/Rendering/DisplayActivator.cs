using UnityEngine;

[ExecuteAlways]
public class DisplayActivator : MonoBehaviour
{

	private void Start()
	{
		foreach(Display curr in Display.displays)
		{
			curr.Activate();
		}
	}

}
