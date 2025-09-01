using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Editor.Services
{
      public static class UtilityService
      {
            public static void DuplicateSelected(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  var duplicates = new List<GameObject>();

                  foreach (GameObject obj in selectedObjects)
                  {
                        GameObject duplicate = Object.Instantiate(obj);
                        duplicate.name = obj.name + " (Clone)";
                        Undo.RegisterCreatedObjectUndo(duplicate, "Duplicate Objects");
                        duplicates.Add(duplicate);
                  }

                  Selection.objects = duplicates.ToArray();
            }

            public static void DeleteSelected(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  foreach (GameObject obj in selectedObjects)
                  {
                        Undo.DestroyObjectImmediate(obj);
                  }
            }

            public static void GroupSelected(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects.Count < 2)
                  {
                        return;
                  }

                  var parent = new GameObject("Group");
                  Undo.RegisterCreatedObjectUndo(parent, "Group Objects");

                  Vector3 center = Vector3.zero;

                  foreach (GameObject obj in selectedObjects)
                  {
                        center += obj.transform.position;
                  }

                  center /= selectedObjects.Count;
                  parent.transform.position = center;

                  foreach (GameObject obj in selectedObjects)
                  {
                        Undo.SetTransformParent(obj.transform, parent.transform, "Group Objects");
                  }

                  Selection.activeGameObject = parent;
            }
      }
}