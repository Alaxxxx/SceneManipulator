using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Services
{
      public static class RotationService
      {
            public static void SetRotation(IReadOnlyList<GameObject> selectedObjects, Vector3 eulerAngles)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Set Rotation");

                  foreach (GameObject obj in selectedObjects)
                  {
                        obj.transform.rotation = Quaternion.Euler(eulerAngles);
                  }
            }

            public static void ResetRotation(IReadOnlyList<GameObject> selectedObjects)
            {
                  SetRotation(selectedObjects, Vector3.zero);
            }

            public static void AddRotation(IReadOnlyList<GameObject> selectedObjects, Vector3 eulerAngles)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Add Rotation");

                  foreach (GameObject obj in selectedObjects)
                  {
                        obj.transform.Rotate(eulerAngles, Space.World);
                  }
            }

            public static void RandomizeRotation(IReadOnlyList<GameObject> selectedObjects, Vector3 minEuler, Vector3 maxEuler)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Randomize Rotation");

                  foreach (GameObject obj in selectedObjects)
                  {
                        float x = Random.Range(minEuler.x, maxEuler.x);
                        float y = Random.Range(minEuler.y, maxEuler.y);
                        float z = Random.Range(minEuler.z, maxEuler.z);
                        obj.transform.rotation = Quaternion.Euler(x, y, z);
                  }
            }
      }
}