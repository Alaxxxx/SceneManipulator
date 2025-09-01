using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Editor.Services
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

                  return GetAdvancedMeasurements(selectedObjects);
            }

            private static string GetAdvancedMeasurements(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects?.Count < 2)
                  {
                        return "Select at least 2 objects.";
                  }

                  var distances = new List<float>();
                  float totalDistance = 0f;

                  for (int i = 0; i < selectedObjects!.Count - 1; i++)
                  {
                        float dist = Vector3.Distance(selectedObjects[i].transform.position, selectedObjects[i + 1].transform.position);
                        distances.Add(dist);
                        totalDistance += dist;
                  }

                  float avgDistance = totalDistance / distances.Count;
                  float minDistance = distances.Min();
                  float maxDistance = distances.Max();

                  var boundsWithSize = new List<Bounds>();

                  foreach (GameObject obj in selectedObjects)
                  {
                        Bounds bounds = GetObjectBounds(obj);

                        if (bounds.size.magnitude > 0.001f)
                        {
                              boundsWithSize.Add(bounds);
                        }
                  }

                  if (boundsWithSize.Count > 0)
                  {
                        Bounds totalBounds = boundsWithSize[0];

                        for (int i = 1; i < boundsWithSize.Count; i++)
                        {
                              totalBounds.Encapsulate(boundsWithSize[i]);
                        }

                        float volume = totalBounds.size.x * totalBounds.size.y * totalBounds.size.z;
                        float area = 2 * (totalBounds.size.x * totalBounds.size.y + totalBounds.size.y * totalBounds.size.z + totalBounds.size.x * totalBounds.size.z);

                        float directDistance = Vector3.Distance(selectedObjects[0].transform.position, selectedObjects[^1].transform.position);

                        return $"Objects: {selectedObjects.Count}\n" + $"Chain: {totalDistance:F2}m\n" + $"Direct: {directDistance:F2}m\n" +
                               $"Avg Gap: {avgDistance:F2}m\n" + $"Range: {minDistance:F2}m - {maxDistance:F2}m\n" +
                               $"Bounds: {totalBounds.size.x:F1} × {totalBounds.size.y:F1} × {totalBounds.size.z:F1}\n" + $"Volume: {volume:F2}m³\n" +
                               $"Surface: {area:F2}m²";
                  }

                  return $"Objects: {selectedObjects.Count}\n" + $"Chain Distance: {totalDistance:F2}m\n" + $"Average: {avgDistance:F2}m\n" +
                         $"Range: {minDistance:F2}m - {maxDistance:F2}m";
            }

            private static Bounds GetObjectBounds(GameObject go)
            {
                  Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

                  if (renderers.Length == 0)
                  {
                        return new Bounds(go.transform.position, Vector3.zero);
                  }

                  Bounds bounds = renderers[0].bounds;

                  for (int i = 1; i < renderers.Length; i++)
                  {
                        bounds.Encapsulate(renderers[i].bounds);
                  }

                  return bounds;
            }
      }
}