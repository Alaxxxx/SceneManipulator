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

            public static void SnapToGround(IReadOnlyList<GameObject> selectedObjects, float maxDistance = 100f, bool alignToNormal = false)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Snap to Ground");

                  foreach (GameObject obj in selectedObjects)
                  {
                        Bounds bounds = GetObjectBounds(obj);
                        Vector3 rayStart = obj.transform.position + Vector3.up * maxDistance;

                        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, maxDistance * 2))
                        {
                              float objectBottomOffset = bounds.size.y * 0.5f;
                              Vector3 newPosition = hit.point + Vector3.up * objectBottomOffset;
                              obj.transform.position = newPosition;

                              if (alignToNormal && hit.normal != Vector3.up)
                              {
                                    Vector3 forward = Vector3.Cross(hit.normal, obj.transform.right).normalized;

                                    if (forward.magnitude < 0.1f)
                                    {
                                          forward = Vector3.Cross(hit.normal, Vector3.forward).normalized;
                                    }

                                    float currentYRotation = obj.transform.eulerAngles.y;
                                    obj.transform.rotation = Quaternion.LookRotation(forward, hit.normal);

                                    Vector3 euler = obj.transform.eulerAngles;
                                    euler.y = currentYRotation;
                                    obj.transform.eulerAngles = euler;
                              }
                        }
                        else
                        {
                              Vector3 pos = obj.transform.position;
                              pos.y = bounds.size.y * 0.5f;
                              obj.transform.position = pos;
                        }
                  }
            }

            public static void SnapToBounds(IReadOnlyList<GameObject> selectedObjects, SnapDirection direction, bool snapToSurface = false)
            {
                  if (selectedObjects.Count < 2)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Snap to Bounds");

                  GameObject reference = selectedObjects[0];
                  Bounds referenceBounds = GetObjectBounds(reference);

                  for (int i = 1; i < selectedObjects.Count; i++)
                  {
                        GameObject target = selectedObjects[i];
                        Bounds targetBounds = GetObjectBounds(target);

                        Vector3 newPosition = target.transform.position;

                        if (snapToSurface)
                        {
                              switch (direction)
                              {
                                    case SnapDirection.Down:
                                          newPosition.y = referenceBounds.min.y - targetBounds.size.y * 0.5f;

                                          break;
                                    case SnapDirection.Up:
                                          newPosition.y = referenceBounds.max.y + targetBounds.size.y * 0.5f;

                                          break;
                                    case SnapDirection.Left:
                                          newPosition.x = referenceBounds.min.x - targetBounds.size.x * 0.5f;

                                          break;
                                    case SnapDirection.Right:
                                          newPosition.x = referenceBounds.max.x + targetBounds.size.x * 0.5f;

                                          break;
                                    case SnapDirection.Forward:
                                          newPosition.z = referenceBounds.max.z + targetBounds.size.z * 0.5f;

                                          break;
                                    case SnapDirection.Back:
                                          newPosition.z = referenceBounds.min.z - targetBounds.size.z * 0.5f;

                                          break;
                              }
                        }
                        else
                        {
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
                        }

                        target.transform.position = newPosition;
                  }
            }

            public static void SnapToGrid(IReadOnlyList<GameObject> selectedObjects, float gridSize = 1f)
            {
                  if (selectedObjects.Count == 0 || gridSize <= 0)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Snap to Grid");

                  foreach (GameObject obj in selectedObjects)
                  {
                        Vector3 pos = obj.transform.position;

                        var snappedPos = new Vector3(Mathf.Round(pos.x / gridSize) * gridSize, Mathf.Round(pos.y / gridSize) * gridSize,
                                    Mathf.Round(pos.z / gridSize) * gridSize);
                        obj.transform.position = snappedPos;
                  }
            }

            public static void SnapBetweenObjects(IReadOnlyList<GameObject> selectedObjects)
            {
                  if (selectedObjects.Count < 3)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Skip(2).Select(static go => go.transform).ToArray<Object>(), "Snap Between Objects");

                  Vector3 pointA = selectedObjects[0].transform.position;
                  Vector3 pointB = selectedObjects[1].transform.position;
                  Vector3 midPoint = Vector3.Lerp(pointA, pointB, 0.5f);

                  for (int i = 2; i < selectedObjects.Count; i++)
                  {
                        selectedObjects[i].transform.position = midPoint;
                  }
            }

            public static void StackObjects(IReadOnlyList<GameObject> selectedObjects, float padding = 0f)
            {
                  if (selectedObjects.Count < 2)
                  {
                        return;
                  }

                  Undo.RecordObjects(selectedObjects.Skip(1).Select(static go => go.transform).ToArray<Object>(), "Stack Objects");

                  GameObject baseObject = selectedObjects[0];
                  float currentY = GetObjectBounds(baseObject).max.y + padding;

                  for (int i = 1; i < selectedObjects.Count; i++)
                  {
                        GameObject obj = selectedObjects[i];
                        Bounds bounds = GetObjectBounds(obj);

                        Vector3 pos = obj.transform.position;
                        pos.y = currentY + bounds.size.y * 0.5f;
                        obj.transform.position = pos;

                        currentY = pos.y + bounds.size.y * 0.5f + padding;
                  }
            }

            private static Bounds GetObjectBounds(GameObject go)
            {
                  Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

                  if (renderers.Length == 0)
                  {
                        return new Bounds(go.transform.position, Vector3.one);
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