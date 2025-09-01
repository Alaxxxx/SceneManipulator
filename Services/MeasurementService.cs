using System.Collections.Generic;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Services
{
      public static class MeasurementService
      {
            public static string GetMeasurementInfo(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects == null || selectedObjects.Count < 2)
                  {
                        return "Select at least 2 objects.";
                  }

                  if (selectedObjects.Count == 2)
                  {
                        float distance = Vector3.Distance(selectedObjects[0].transform.position, selectedObjects[1].transform.position);

                        return $"Distance: {distance:F2}m";
                  }

                  float totalDistance = 0f;

                  for (int i = 0; i < selectedObjects.Count - 1; i++)
                  {
                        totalDistance += Vector3.Distance(selectedObjects[i].transform.position, selectedObjects[i + 1].transform.position);
                  }

                  return $"Chain Distance ({selectedObjects.Count} objects): {totalDistance:F2}m";
            }
      }
}