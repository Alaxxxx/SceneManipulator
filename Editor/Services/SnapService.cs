using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Editor.Services
{
      public static class SnapService
      {
            public enum SnapDirection
            {
                  Down,
                  Up,
                  Forward,
                  Back,
                  Left,
                  Right
            }

            public static void SnapToGround(IReadOnlyList<GameObject> selectedObjects, bool alignToNormal)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Snap to Ground");

                  foreach (GameObject obj in selectedObjects)
                  {
                        Bounds bounds = GetObjectBounds(obj);

                        if (bounds.size == Vector3.zero)
                        {
                              continue;
                        }

                        Vector3 position = obj.transform.position;
                        float pivotToBottomOffset = position.y - bounds.min.y;

                        RaycastHit[] hits = Physics.RaycastAll(position + Vector3.up * 100f, Vector3.down, 200f);

                        System.Array.Sort(hits, static (h1, h2) => h1.distance.CompareTo(h2.distance));

                        RaycastHit? validHit = null;

                        foreach (RaycastHit hit in hits)
                        {
                              if (!hit.transform.IsChildOf(obj.transform))
                              {
                                    validHit = hit;

                                    break;
                              }
                        }

                        if (validHit.HasValue)
                        {
                              RaycastHit hit = validHit.Value;
                              Vector3 position1 = obj.transform.position;
                              var newPosition = new Vector3(position1.x, hit.point.y + pivotToBottomOffset, position1.z);
                              position1 = newPosition;
                              obj.transform.position = position1;

                              if (alignToNormal)
                              {
                                    float originalYRotation = obj.transform.eulerAngles.y;
                                    Quaternion surfaceAlignmentRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                                    obj.transform.rotation = surfaceAlignmentRotation * obj.transform.rotation;
                                    Vector3 euler = obj.transform.eulerAngles;
                                    euler.y = originalYRotation;
                                    obj.transform.eulerAngles = euler;
                              }
                        }
                        else
                        {
                              Vector3 pos = obj.transform.position;
                              pos.y = pivotToBottomOffset;
                              obj.transform.position = pos;
                        }
                  }
            }

            public static void SnapToFirstSelected(IReadOnlyList<GameObject> selectedObjects, SnapDirection direction)
            {
                  if (selectedObjects.Count < 2)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Skip(1).Select(static go => go.transform).ToArray<Object>(), "Snap to Selection");

                  GameObject referenceObject = selectedObjects[0];
                  Bounds referenceBounds = GetObjectBounds(referenceObject);

                  for (int i = 1; i < selectedObjects.Count; i++)
                  {
                        GameObject targetObject = selectedObjects[i];
                        Bounds targetBounds = GetObjectBounds(targetObject);
                        Vector3 newPosition = targetObject.transform.position;

                        switch (direction)
                        {
                              case SnapDirection.Down:
                                    newPosition.y = referenceBounds.min.y - targetBounds.extents.y;

                                    break;
                              case SnapDirection.Up:
                                    newPosition.y = referenceBounds.max.y + targetBounds.extents.y;

                                    break;
                              case SnapDirection.Left:
                                    newPosition.x = referenceBounds.min.x - targetBounds.extents.x;

                                    break;
                              case SnapDirection.Right:
                                    newPosition.x = referenceBounds.max.x + targetBounds.extents.x;

                                    break;
                              case SnapDirection.Forward:
                                    newPosition.z = referenceBounds.max.z + targetBounds.extents.z;

                                    break;
                              case SnapDirection.Back:
                                    newPosition.z = referenceBounds.min.z - targetBounds.extents.z;

                                    break;
                        }

                        targetObject.transform.position = newPosition;
                  }
            }

            public static void SnapToGrid(IReadOnlyList<GameObject> selectedObjects, float gridSize)
            {
                  if (selectedObjects.Count == 0 || gridSize <= 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Snap to Grid");

                  foreach (GameObject obj in selectedObjects)
                  {
                        Vector3 pos = obj.transform.position;

                        obj.transform.position = new Vector3(Mathf.Round(pos.x / gridSize) * gridSize, Mathf.Round(pos.y / gridSize) * gridSize,
                                    Mathf.Round(pos.z / gridSize) * gridSize);
                  }
            }

            public static void StackObjects(IReadOnlyList<GameObject> selectedObjects, float padding)
            {
                  if (selectedObjects.Count < 2)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Skip(1).Select(static go => go.transform).ToArray<Object>(), "Stack Objects");

                  GameObject baseObject = selectedObjects[0];
                  Bounds baseBounds = GetObjectBounds(baseObject);
                  float currentY = baseBounds.max.y + padding;

                  for (int i = 1; i < selectedObjects.Count; i++)
                  {
                        GameObject obj = selectedObjects[i];
                        Bounds bounds = GetObjectBounds(obj);

                        Vector3 pos = obj.transform.position;
                        pos.y = currentY + bounds.extents.y;
                        obj.transform.position = pos;

                        currentY = pos.y + bounds.extents.y + padding;
                  }
            }

            private static Bounds GetObjectBounds(GameObject go)
            {
                  Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

                  if (renderers.Length > 0)
                  {
                        Bounds bounds = renderers[0].bounds;

                        for (int i = 1; i < renderers.Length; i++)
                        {
                              bounds.Encapsulate(renderers[i].bounds);
                        }

                        return bounds;
                  }

                  return new Bounds(go.transform.position, Vector3.zero);
            }
      }
}