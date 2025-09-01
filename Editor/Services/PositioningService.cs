using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Editor.Services
{
      public static class PositioningService
      {
            public static void SetPosition(IReadOnlyList<GameObject> selectedObjects, Vector3 position)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Set Position");

                  foreach (GameObject obj in selectedObjects)
                  {
                        obj.transform.position = position;
                  }
            }

            public static void CenterObjects(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Vector3 center = selectedObjects.Aggregate(Vector3.zero, static (acc, go) => acc + go.transform.position) / selectedObjects.Count;
                  Vector3 offset = Vector3.zero - center;

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Center Objects");

                  foreach (GameObject obj in selectedObjects)
                  {
                        obj.transform.position += offset;
                  }
            }

            public static void SnapToGround(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Snap to Ground");

                  foreach (GameObject obj in selectedObjects)
                  {
                        if (Physics.Raycast(obj.transform.position + Vector3.up * 100, Vector3.down, out RaycastHit hit))
                        {
                              obj.transform.position = hit.point;
                        }
                        else
                        {
                              Vector3 position = obj.transform.position;
                              obj.transform.position = new Vector3(position.x, 0, position.z);
                        }
                  }
            }

            public static void MoveToOrigin(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Move to Origin");

                  foreach (GameObject obj in selectedObjects)
                  {
                        obj.transform.position = Vector3.zero;
                  }
            }
      }
}