using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class TEST : MonoBehaviour
{
    public SplineContainer splineContainer;

    void Update()
    {
        if (splineContainer == null) return;

        for (int i = 0; i < splineContainer.Splines.Count; i++)
        {
            Spline spline = splineContainer.Splines[i];
            for (int j = 0; j < spline.Count; j++)
            {
                BezierKnot knot = spline[j];
                Vector3 worldPosition = transform.TransformPoint(knot.Position);
                Quaternion worldRotation = transform.rotation * knot.Rotation;

                knot.Position = transform.InverseTransformPoint(worldPosition);
                knot.Rotation = Quaternion.Inverse(transform.rotation) * worldRotation;

                spline[j] = knot;
            }
        }
    }
}
