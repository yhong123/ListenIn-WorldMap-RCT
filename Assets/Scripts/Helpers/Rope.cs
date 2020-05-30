using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rope : MonoBehaviour
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public GameObject RopeSection;
        public bool BothSidesConnected;

        public GameObject ConnectedObject;

        // TODO: Figure this out from gameobject and not hardcoded
        private float _size;
		private float _step;

		private int segmentCount;

        private List<GameObject> _ropeSegments;

        void Awake()
        {
			//Size = RopeSection.GetComponent<Renderer>().bounds.extents.y;
			_size = RopeSection.GetComponent<Renderer>().bounds.extents.y;
			_step = 0.1f;//_size; //* .5f;
            _ropeSegments = new List<GameObject>();

			float dist = Vector3.Distance(EndPosition,StartPosition);
			segmentCount = (int)Math.Round(dist/_step,0);
			for (var i = 0; i < segmentCount; i++)
            {
                var section = (GameObject)Instantiate(RopeSection);

				if (i == 0 || (BothSidesConnected && i == segmentCount - 1)) section.GetComponent<Rigidbody2D>().isKinematic = true;
                if(i != 0) section.GetComponent<HingeJoint2D>().connectedBody = _ropeSegments[i - 1].GetComponent<Rigidbody2D>();
				if (ConnectedObject != null && i == segmentCount - 1)
                {
                    var hinge = ConnectedObject.AddComponent<HingeJoint2D>();
                    hinge.connectedBody = section.GetComponent<Rigidbody2D>();
                    hinge.useLimits = true;
                    hinge.limits = new JointAngleLimits2D { min = 0, max = 0 };
                };

				section.transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, _step * i);
				section.transform.SetParent(transform,false);
				//section.transform.localScale = new Vector3(6.0f,2.5f);
                //section.transform.SetParent(transform);
				//section.transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, _step * i);
                _ropeSegments.Add(section);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position + StartPosition, transform.position + EndPosition);
        }
    }
}