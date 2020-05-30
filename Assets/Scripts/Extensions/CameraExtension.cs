using UnityEngine;
using System.Collections;

public static class CameraExtension {

	public enum CameraAxis { X, Y, Z, XY, XZ, YZ, ALL};

	public static IEnumerator LinearShake(this Camera cam, float duration, float magnitude, CameraAxis axis)
	{
		for (float time = 0; time < duration; time += Time.deltaTime) {
			float x,y,z;
			Vector3 offset;
			x = Random.Range(-magnitude, magnitude);
			y = Random.Range(-magnitude, magnitude);
			z = Random.Range(-magnitude, magnitude);

			switch (axis) {
			case CameraAxis.X:
				offset = new Vector3(x,0,0);
				break;
			case CameraAxis.Y:
				offset = new Vector3(0,y,0);
				break;
			case CameraAxis.Z:
				offset = new Vector3(0,0,z);
				break;
			case CameraAxis.XY:
				offset = new Vector3(x,y,0);
				break;
			case CameraAxis.XZ:
				offset = new Vector3(x,0,z);
				break;
			case CameraAxis.YZ:
				offset = new Vector3(0,y,z);
				break;
			case CameraAxis.ALL:
				offset = new Vector3(x,y,z);
				break;
			default:
				offset = Vector3.zero;
				break;
			}

			cam.transform.position += offset;
			yield return new WaitForEndOfFrame();
			cam.transform.position -= offset;
		}

	}

}
