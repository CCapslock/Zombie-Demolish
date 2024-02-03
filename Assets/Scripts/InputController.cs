using UnityEngine;

public class InputController : MonoBehaviour
{
	//следит за пальцем на экране
	public Vector3 TouchPosition;
	[HideInInspector] public bool DragingStarted = false;
	private void Start()
	{
		TouchPosition = new Vector3(0, 0);
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			DragingStarted = true;
			TouchPosition.z = 0f;
			TouchPosition.x = Input.mousePosition.x / 40f;
			TouchPosition.y = Input.mousePosition.y / 40f;
		}
		else
		{
			DragingStarted = false;
		}
	}
}
