using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator.Editor.Services
{
      public static class VisualizationService
      {
            public static bool ShowDistanceGizmos { get; set; }
            private readonly static List<(Vector3 start, Vector3 end, float distance)> Connections = new();
            private readonly static List<GameObject> CurrentSelection = new();

            public static void UpdateConnections(IReadOnlyList<GameObject> objects)
            {
                  Connections.Clear();
                  CurrentSelection.Clear();

                  if (!ShowDistanceGizmos || objects == null || objects.Count < 2)
                  {
                        return;
                  }

                  CurrentSelection.AddRange(objects);

                  int maxConnections = Mathf.Min(15, objects.Count - 1);

                  for (int i = 0; i < maxConnections; i++)
                  {
                        Vector3 start = objects[i].transform.position;
                        Vector3 end = objects[i + 1].transform.position;
                        float distance = Vector3.Distance(start, end);

                        Connections.Add((start, end, distance));
                  }

                  SceneView.RepaintAll();
            }

            [DrawGizmo(GizmoType.Active | GizmoType.Selected)]
            private static void DrawDistanceGizmos(Transform transform, GizmoType gizmoType)
            {
                  if (!ShowDistanceGizmos || Connections.Count == 0)
                  {
                        return;
                  }

                  if (!CurrentSelection.Contains(transform.gameObject))
                  {
                        return;
                  }

                  Gizmos.color = Color.cyan;
                  Handles.color = Color.cyan;

                  foreach ((Vector3 start, Vector3 end, float distance) connection in Connections)
                  {
                        Gizmos.DrawLine(connection.start, connection.end);

                        Vector3 direction = (connection.end - connection.start).normalized;
                        Vector3 midPoint = Vector3.Lerp(connection.start, connection.end, 0.5f);

                        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;

                        if (perpendicular.magnitude < 0.1f)
                        {
                              perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;
                        }

                        const float arrowSize = 0.3f;
                        Vector3 arrowTip = midPoint + direction * arrowSize;
                        Vector3 arrowLeft = arrowTip - direction * arrowSize * 0.5f + perpendicular * arrowSize * 0.3f;
                        Vector3 arrowRight = arrowTip - direction * arrowSize * 0.5f - perpendicular * arrowSize * 0.3f;

                        Gizmos.DrawLine(arrowTip, arrowLeft);
                        Gizmos.DrawLine(arrowTip, arrowRight);

                        var labelStyle = new GUIStyle
                        {
                                    normal =
                                    {
                                                textColor = Color.cyan
                                    },
                                    fontSize = 12,
                                    fontStyle = FontStyle.Bold
                        };

                        Vector3 labelPos = midPoint + Vector3.up * 0.5f;
                        Handles.Label(labelPos, $"{connection.distance:F1}m", labelStyle);
                  }
            }

            public static void ClearConnections()
            {
                  Connections.Clear();
                  CurrentSelection.Clear();
                  SceneView.RepaintAll();
            }
      }
}