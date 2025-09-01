using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Editor.Services
{
      public static class AlignmentService
      {
            public enum Axis
            {
                  X,
                  Y,
                  Z
            }

            public enum Target
            {
                  Min,
                  Center,
                  Max
            }

            public static void AlignObjects(IReadOnlyList<GameObject> selectedObjects, Axis axis, Target target)
            {
                  if (selectedObjects.Count < 2)
                  {
                        return;
                  }

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

                  Undo.RecordObjects(selectedObjects.Select(static go => go.transform).ToArray<Object>(), "Align Objects");

                  foreach (GameObject go in selectedObjects)
                  {
                        Bounds objectBounds = GetObjectBounds(go);

                        if (objectBounds.size == Vector3.zero)
                        {
                              continue;
                        }

                        float targetValue = 0;
                        float sourceValue = 0;

                        switch (target)
                        {
                              case Target.Min:
                                    targetValue = totalBounds.min[(int)axis];
                                    sourceValue = objectBounds.min[(int)axis];

                                    break;
                              case Target.Center:
                                    targetValue = totalBounds.center[(int)axis];
                                    sourceValue = objectBounds.center[(int)axis];

                                    break;
                              case Target.Max:
                                    targetValue = totalBounds.max[(int)axis];
                                    sourceValue = objectBounds.max[(int)axis];

                                    break;
                        }

                        float delta = targetValue - sourceValue;

                        Vector3 movement = Vector3.zero;
                        movement[(int)axis] = delta;

                        go.transform.position += movement;
                  }
            }

            private static Bounds GetObjectBounds(GameObject go)
            {
                  Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

                  if (renderers.Length == 0)
                  {
                        return new Bounds();
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