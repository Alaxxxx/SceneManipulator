using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Services
{
      public static class ArrangementService
      {
            public static void ArrangeInLine(IReadOnlyList<GameObject> selectedObjects, Vector3 direction, float spacing)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Arrange in Line");
                  Vector3 startPos = selectedObjects[0].transform.position;

                  for (int i = 0; i < selectedObjects.Count; i++)
                  {
                        selectedObjects[i].transform.position = startPos + direction * spacing * i;
                  }
            }

            public static void ArrangeInGrid(IReadOnlyList<GameObject> selectedObjects, Vector3 spacing)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Arrange in Grid");
                  int gridSize = Mathf.CeilToInt(Mathf.Sqrt(selectedObjects.Count));
                  Vector3 startPos = selectedObjects[0].transform.position;

                  for (int i = 0; i < selectedObjects.Count; i++)
                  {
                        int x = i % gridSize;
                        int z = i / gridSize;
                        selectedObjects[i].transform.position = startPos + new Vector3(x * spacing.x, 0, z * spacing.z);
                  }
            }

            public static void ArrangeInCircle(IReadOnlyList<GameObject> selectedObjects, float radius)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Arrange in Circle");
                  Vector3 center = selectedObjects[0].transform.position;
                  float angleStep = 360f / selectedObjects.Count;

                  for (int i = 0; i < selectedObjects.Count; i++)
                  {
                        float angle = i * angleStep * Mathf.Deg2Rad;
                        Vector3 pos = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                        selectedObjects[i].transform.position = pos;
                  }
            }

            public static void ArrangeRandomly(IReadOnlyList<GameObject> selectedObjects, float range)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Arrange Randomly");
                  Vector3 basePos = selectedObjects[0].transform.position;

                  foreach (GameObject obj in selectedObjects)
                  {
                        var randomOffset = new Vector3(Random.Range(-range, range), Random.Range(-range / 2, range / 2), Random.Range(-range, range));
                        obj.transform.position = basePos + randomOffset;
                  }
            }
      }
}