using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Editor.Services
{
      public static class PreviewService
      {
            private readonly static Dictionary<GameObject, Vector3> OriginalPositions = new();
            private readonly static Dictionary<GameObject, Quaternion> OriginalRotations = new();
            private static bool isPreviewActive;

            public static void PreviewArrangeInLine(IReadOnlyList<GameObject> selectedObjects, Vector3 direction, float spacing)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  StartPreview(selectedObjects);
                  Vector3 startPos = OriginalPositions[selectedObjects[0]];

                  for (int i = 0; i < selectedObjects.Count; i++)
                  {
                        selectedObjects[i].transform.position = startPos + direction * spacing * i;
                  }

                  SceneView.RepaintAll();
            }

            public static void PreviewArrangeInGrid(IReadOnlyList<GameObject> selectedObjects, Vector3 spacing)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  StartPreview(selectedObjects);
                  int gridSize = Mathf.CeilToInt(Mathf.Sqrt(selectedObjects.Count));
                  Vector3 startPos = OriginalPositions[selectedObjects[0]];

                  for (int i = 0; i < selectedObjects.Count; i++)
                  {
                        int x = i % gridSize;
                        int z = i / gridSize;
                        selectedObjects[i].transform.position = startPos + new Vector3(x * spacing.x, 0, z * spacing.z);
                  }

                  SceneView.RepaintAll();
            }

            public static void PreviewArrangeInCircle(IReadOnlyList<GameObject> selectedObjects, float radius)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  StartPreview(selectedObjects);
                  Vector3 center = OriginalPositions[selectedObjects[0]];
                  float angleStep = 360f / selectedObjects.Count;

                  for (int i = 0; i < selectedObjects.Count; i++)
                  {
                        float angle = i * angleStep * Mathf.Deg2Rad;
                        Vector3 pos = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                        selectedObjects[i].transform.position = pos;
                  }

                  SceneView.RepaintAll();
            }

            public static void PreviewAlign(IReadOnlyList<GameObject> selectedObjects, AlignmentService.Axis axis, AlignmentService.Target target)
            {
                  if (selectedObjects.Count < 2)
                  {
                        return;
                  }

                  StartPreview(selectedObjects);

                  var totalBounds = new Bounds();
                  bool boundsInitialized = false;

                  foreach (GameObject go in selectedObjects)
                  {
                        Bounds objectBounds = GetObjectBounds(go);

                        if (objectBounds.size == Vector3.zero)
                        {
                              continue;
                        }

                        if (!boundsInitialized)
                        {
                              totalBounds = objectBounds;
                              boundsInitialized = true;
                        }
                        else
                        {
                              totalBounds.Encapsulate(objectBounds);
                        }
                  }

                  if (!boundsInitialized)
                  {
                        return;
                  }

                  foreach (GameObject go in selectedObjects)
                  {
                        Bounds objectBounds = GetObjectBounds(go);

                        if (objectBounds.size == Vector3.zero)
                        {
                              continue;
                        }

                        float targetValue = target switch
                        {
                                    AlignmentService.Target.Min => totalBounds.min[(int)axis],
                                    AlignmentService.Target.Center => totalBounds.center[(int)axis],
                                    AlignmentService.Target.Max => totalBounds.max[(int)axis],
                                    _ => 0
                        };

                        float sourceValue = target switch
                        {
                                    AlignmentService.Target.Min => objectBounds.min[(int)axis],
                                    AlignmentService.Target.Center => objectBounds.center[(int)axis],
                                    AlignmentService.Target.Max => objectBounds.max[(int)axis],
                                    _ => 0
                        };

                        float delta = targetValue - sourceValue;
                        Vector3 movement = Vector3.zero;
                        movement[(int)axis] = delta;

                        go.transform.position = OriginalPositions[go] + movement;
                  }

                  SceneView.RepaintAll();
            }

            public static void PreviewAddRotation(IReadOnlyList<GameObject> selectedObjects, Vector3 eulerAngles)
            {
                  if (selectedObjects.Count == 0)
                  {
                        return;
                  }

                  StartPreview(selectedObjects);

                  foreach (GameObject obj in selectedObjects)
                  {
                        Quaternion addRotation = Quaternion.Euler(eulerAngles);
                        obj.transform.rotation = OriginalRotations[obj] * addRotation;
                  }

                  SceneView.RepaintAll();
            }

            public static void CancelPreview()
            {
                  if (!isPreviewActive)
                  {
                        return;
                  }

                  foreach (KeyValuePair<GameObject, Vector3> kvp in OriginalPositions)
                  {
                        if (kvp.Key != null)
                        {
                              kvp.Key.transform.position = kvp.Value;
                        }
                  }

                  foreach (KeyValuePair<GameObject, Quaternion> kvp in OriginalRotations)
                  {
                        if (kvp.Key != null)
                        {
                              kvp.Key.transform.rotation = kvp.Value;
                        }
                  }

                  OriginalPositions.Clear();
                  OriginalRotations.Clear();
                  isPreviewActive = false;

                  SceneView.RepaintAll();
            }

            private static void StartPreview(IEnumerable<GameObject> selectedObjects)
            {
                  if (isPreviewActive)
                  {
                        return;
                  }

                  OriginalPositions.Clear();
                  OriginalRotations.Clear();

                  foreach (GameObject obj in selectedObjects)
                  {
                        OriginalPositions[obj] = obj.transform.position;
                        OriginalRotations[obj] = obj.transform.rotation;
                  }

                  isPreviewActive = true;
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