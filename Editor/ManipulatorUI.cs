using System;
using System.Collections.Generic;
using OpalStudio.SceneManipulator.Editor.Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace OpalStudio.SceneManipulator.Editor
{
      public sealed class ManipulatorUI
      {
            // Events for Positioning
            public event Action<Vector3> OnSetPositionClicked;
            public event Action OnCenterClicked;
            public event Action OnMoveToOriginClicked;

            // Events for alignment
            public event Action<AlignmentService.Axis, AlignmentService.Target> OnAlignClicked;

            // Events for Rotation
            public event Action<Vector3> OnSetRotationClicked;
            public event Action OnResetRotationClicked;
            public event Action<Vector3> OnAddRotationClicked;
            public event Action OnRandomizeYRotationClicked;

            // Events for Arrangement
            public event Action<Vector3> OnSpacingChanged;
            public event Action<Vector3> OnArrangeInLineClicked;
            public event Action OnArrangeInGridClicked;
            public event Action OnArrangeInCircleClicked;
            public event Action OnArrangeRandomlyClicked;

            // Events for Snapping
            public event Action<bool> OnSnapToGroundAdvancedClicked;
            public event Action<SnapService.SnapDirection> OnSnapToBoundsClicked;
            public event Action<float> OnSnapToGridClicked;
            public event Action<float> OnStackObjectsClicked;

            // Events for Utilities
            public event Action OnDuplicateClicked;
            public event Action OnDeleteClicked;
            public event Action OnGroupClicked;

            private VisualElement _root;
            private Label _measurementInfoLabel;

            public VisualElement CreatePanel()
            {
                  _root = new VisualElement
                              { name = "ManipulatorRoot", style = { minWidth = 220, paddingRight = 4, paddingLeft = 4, paddingTop = 4, paddingBottom = 4 } };

                  CreatePositioningSection();
                  CreateAlignmentSection();
                  CreateRotationSection();
                  CreateArrangementSection();
                  CreateSnapSection();
                  CreateMeasurementSection();
                  CreateUtilitySection();

                  return _root;
            }

            public void UpdateMeasurementInfo(string info)
            {
                  if (_measurementInfoLabel != null)
                  {
                        _measurementInfoLabel.text = info;
                  }
            }

            private void CreatePositioningSection()
            {
                  Foldout section = CreateSectionFoldout("Positioning");
                  _root.Add(section);

                  var posField = new Vector3Field("Position") { tooltip = "Set the absolute world position for all selected objects." };
                  posField.RegisterValueChangedCallback(evt => OnSetPositionClicked?.Invoke(evt.newValue));
                  section.Add(posField);

                  VisualElement quickPosRow = CreateButtonRow();
                  quickPosRow.Add(CreateButton("Center", "Move the center of the selection to the world origin", () => OnCenterClicked?.Invoke()));
                  quickPosRow.Add(CreateButton("Ground", "Snap objects to the nearest surface below", () => OnSnapToGroundAdvancedClicked?.Invoke(false)));
                  quickPosRow.Add(CreateButton("Origin", "Move all selected objects to (0,0,0)", () => OnMoveToOriginClicked?.Invoke()));
                  section.Add(quickPosRow);
            }

            private void CreateAlignmentSection()
            {
                  Foldout section = CreateSectionFoldout("Alignment", false);
                  _root.Add(section);

                  VisualElement rowX = CreateButtonRow();
                  rowX.Add(new Label("X") { style = { width = 12 } });

                  rowX.Add(CreateAlignmentPreviewButton("d_align_horizontally_left_active", "Align Min X",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.X, AlignmentService.Target.Min),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.X, AlignmentService.Target.Min)));

                  rowX.Add(CreateAlignmentPreviewButton("d_align_horizontally_center_active", "Align Center X",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.X, AlignmentService.Target.Center),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.X, AlignmentService.Target.Center)));

                  rowX.Add(CreateAlignmentPreviewButton("d_align_horizontally_right_active", "Align Max X",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.X, AlignmentService.Target.Max),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.X, AlignmentService.Target.Max)));
                  section.Add(rowX);

                  VisualElement rowY = CreateButtonRow();
                  rowY.Add(new Label("Y") { style = { width = 12 } });

                  rowY.Add(CreateAlignmentPreviewButton("d_align_vertically_bottom_active", "Align Min Y",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.Y, AlignmentService.Target.Min),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.Y, AlignmentService.Target.Min)));

                  rowY.Add(CreateAlignmentPreviewButton("d_align_vertically_center_active", "Align Center Y",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.Y, AlignmentService.Target.Center),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.Y, AlignmentService.Target.Center)));

                  rowY.Add(CreateAlignmentPreviewButton("d_align_vertically_top_active", "Align Max Y",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.Y, AlignmentService.Target.Max),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.Y, AlignmentService.Target.Max)));
                  section.Add(rowY);

                  VisualElement rowZ = CreateButtonRow();
                  rowZ.Add(new Label("Z") { style = { width = 12 } });

                  rowZ.Add(CreateAlignmentPreviewButton("d_align_horizontally_left_active", "Align Min Z (Forward)",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.Z, AlignmentService.Target.Min),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.Z, AlignmentService.Target.Min)));

                  rowZ.Add(CreateAlignmentPreviewButton("d_align_horizontally_center_active", "Align Center Z",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.Z, AlignmentService.Target.Center),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.Z, AlignmentService.Target.Center)));

                  rowZ.Add(CreateAlignmentPreviewButton("d_align_horizontally_right_active", "Align Max Z (Back)",
                              () => OnAlignClicked?.Invoke(AlignmentService.Axis.Z, AlignmentService.Target.Max),
                              static objects => PreviewService.PreviewAlign(objects, AlignmentService.Axis.Z, AlignmentService.Target.Max)));
                  section.Add(rowZ);
            }

            private void CreateRotationSection()
            {
                  Foldout section = CreateSectionFoldout("Rotation", false);
                  _root.Add(section);

                  var rotField = new Vector3Field("Rotation") { tooltip = "Set the absolute world rotation for all selected objects." };
                  rotField.RegisterValueChangedCallback(evt => OnSetRotationClicked?.Invoke(evt.newValue));
                  section.Add(rotField);

                  VisualElement quickRotRow1 = CreateButtonRow();
                  quickRotRow1.Add(CreateButton("Reset", "Reset rotation to (0,0,0)", () => OnResetRotationClicked?.Invoke()));
                  quickRotRow1.Add(CreateButton("Random Y", "Randomize Y rotation (0-360)", () => OnRandomizeYRotationClicked?.Invoke()));
                  section.Add(quickRotRow1);

                  VisualElement quickRotRow2 = CreateButtonRow();

                  quickRotRow2.Add(CreateRotationPreviewButton("+90° Y", "Add 90 degrees to Y rotation", () => OnAddRotationClicked?.Invoke(new Vector3(0, 90, 0)),
                              static objects => PreviewService.PreviewAddRotation(objects, new Vector3(0, 90, 0))));

                  quickRotRow2.Add(CreateRotationPreviewButton("-90° Y", "Subtract 90 degrees from Y rotation",
                              () => OnAddRotationClicked?.Invoke(new Vector3(0, -90, 0)),
                              static objects => PreviewService.PreviewAddRotation(objects, new Vector3(0, -90, 0))));
                  section.Add(quickRotRow2);
            }

            private void CreateArrangementSection()
            {
                  Foldout section = CreateSectionFoldout("Arrangement", false);
                  _root.Add(section);

                  var spacingField = new Vector3Field("Spacing") { value = Vector3.right * 2f };
                  spacingField.RegisterValueChangedCallback(evt => OnSpacingChanged?.Invoke(evt.newValue));
                  section.Add(spacingField);

                  VisualElement arrangeRow1 = CreateButtonRow();

                  arrangeRow1.Add(CreateArrangementPreviewButton("Line X", "Arrange along the X axis", () => OnArrangeInLineClicked?.Invoke(Vector3.right),
                              static (objects, spacing) => PreviewService.PreviewArrangeInLine(objects, Vector3.right, spacing.magnitude)));

                  arrangeRow1.Add(CreateArrangementPreviewButton("Line Y", "Arrange along the Y axis", () => OnArrangeInLineClicked?.Invoke(Vector3.up),
                              static (objects, spacing) => PreviewService.PreviewArrangeInLine(objects, Vector3.up, spacing.magnitude)));

                  arrangeRow1.Add(CreateArrangementPreviewButton("Line Z", "Arrange along the Z axis", () => OnArrangeInLineClicked?.Invoke(Vector3.forward),
                              static (objects, spacing) => PreviewService.PreviewArrangeInLine(objects, Vector3.forward, spacing.magnitude)));
                  section.Add(arrangeRow1);

                  VisualElement arrangeRow2 = CreateButtonRow();

                  arrangeRow2.Add(CreateArrangementPreviewButton("Grid", "Arrange in a grid on the XZ plane", () => OnArrangeInGridClicked?.Invoke(),
                              static (objects, spacing) => PreviewService.PreviewArrangeInGrid(objects, spacing)));

                  arrangeRow2.Add(CreateArrangementPreviewButton("Circle", "Arrange in a circle on the XZ plane", () => OnArrangeInCircleClicked?.Invoke(),
                              static (objects, spacing) => PreviewService.PreviewArrangeInCircle(objects, spacing.magnitude)));
                  arrangeRow2.Add(CreateButton("Random", "Arrange randomly around the first object", () => OnArrangeRandomlyClicked?.Invoke()));
                  section.Add(arrangeRow2);
            }

            private void CreateSnapSection()
            {
                  Foldout section = CreateSectionFoldout("Snap", false);
                  _root.Add(section);

                  // --- Snap to Ground ---
                  VisualElement groundRow = CreateButtonRow();
                  groundRow.Add(CreateButton("Ground", "Snap objects to ground below", () => OnSnapToGroundAdvancedClicked?.Invoke(false)));
                  groundRow.Add(CreateButton("Ground+Align", "Snap to ground and align to surface normal", () => OnSnapToGroundAdvancedClicked?.Invoke(true)));
                  section.Add(groundRow);

                  // --- Snap to Grid ---
                  VisualElement gridRow = CreateButtonRow();
                  var gridField = new FloatField("Grid Size") { value = 1f, tooltip = "Grid size for snapping", style = { flexGrow = 1 } };
                  gridRow.Add(gridField);
                  gridRow.Add(CreateButton("Snap", "Snap positions to grid points", () => OnSnapToGridClicked?.Invoke(gridField.value)));
                  section.Add(gridRow);

                  // --- Snap to Selection ---
                  var boundsLabel = new Label("Snap to First Selected Object:") { style = { marginTop = 8, fontSize = 11 } };
                  section.Add(boundsLabel);

                  VisualElement boundsRow1 = CreateButtonRow();
                  boundsRow1.Add(CreateButton("↓ Down", "Snap below reference", () => OnSnapToBoundsClicked?.Invoke(SnapService.SnapDirection.Down)));
                  boundsRow1.Add(CreateButton("↑ Up", "Snap above reference", () => OnSnapToBoundsClicked?.Invoke(SnapService.SnapDirection.Up)));
                  boundsRow1.Add(CreateButton("← Left", "Snap to left of reference", () => OnSnapToBoundsClicked?.Invoke(SnapService.SnapDirection.Left)));
                  boundsRow1.Add(CreateButton("→ Right", "Snap to right of reference", () => OnSnapToBoundsClicked?.Invoke(SnapService.SnapDirection.Right)));
                  section.Add(boundsRow1);

                  VisualElement boundsRow2 = CreateButtonRow();
                  boundsRow2.Add(CreateButton("Forward", "Snap in front of reference", () => OnSnapToBoundsClicked?.Invoke(SnapService.SnapDirection.Forward)));
                  boundsRow2.Add(CreateButton("Back", "Snap behind reference", () => OnSnapToBoundsClicked?.Invoke(SnapService.SnapDirection.Back)));
                  section.Add(boundsRow2);

                  // --- Stack ---
                  VisualElement otherRow = CreateButtonRow();
                  otherRow.style.marginTop = 8;
                  var stackField = new FloatField("Stack Padding") { value = 0f, tooltip = "Vertical padding between stacked objects", style = { flexGrow = 1 } };
                  otherRow.Add(stackField);
                  otherRow.Add(CreateButton("Stack", "Stack selected objects vertically (first = base)", () => OnStackObjectsClicked?.Invoke(stackField.value)));
                  section.Add(otherRow);
            }

            private void CreateMeasurementSection()
            {
                  Foldout section = CreateSectionFoldout("Measurement", false);
                  _root.Add(section);

                  VisualElement toggleRow = CreateButtonRow();

                  var gizmosToggle = new Toggle("Show Distance Gizmos")
                  {
                              value = VisualizationService.ShowDistanceGizmos,
                              tooltip = "Display distance measurements between objects in the scene view"
                  };

                  gizmosToggle.RegisterValueChangedCallback(static evt =>
                  {
                        VisualizationService.ShowDistanceGizmos = evt.newValue;

                        if (evt.newValue)
                        {
                              GameObject[] selectedObjects = Selection.gameObjects;
                              VisualizationService.UpdateConnections(selectedObjects);
                        }
                        else
                        {
                              VisualizationService.ClearConnections();
                        }
                  });
                  toggleRow.Add(gizmosToggle);
                  section.Add(toggleRow);

                  _measurementInfoLabel = new Label("Select at least 2 objects.") { style = { whiteSpace = WhiteSpace.Normal } };
                  section.Add(_measurementInfoLabel);
            }

            private void CreateUtilitySection()
            {
                  Foldout section = CreateSectionFoldout("Utilities", false);
                  _root.Add(section);

                  VisualElement utilRow1 = CreateButtonRow();
                  utilRow1.Add(CreateButton("Duplicate", "Create a duplicate of the selection", () => OnDuplicateClicked?.Invoke()));
                  utilRow1.Add(CreateButton("Delete", "Delete the selected objects", () => OnDeleteClicked?.Invoke()));
                  utilRow1.Add(CreateButton("Group", "Group selected objects under a new parent", () => OnGroupClicked?.Invoke()));
                  section.Add(utilRow1);
            }

            private static Button CreateButton(string text, string tooltip, Action onClick)
            {
                  return new Button(onClick) { text = text, tooltip = tooltip, style = { flexGrow = 1, marginLeft = 1, marginRight = 1 } };
            }

            private static Button CreateAlignmentPreviewButton(string iconName, string tooltip, Action onClick, Action<IReadOnlyList<GameObject>> onPreview)
            {
                  var btn = new Button(onClick)
                  {
                              tooltip = tooltip + " (Hover to preview)",
                              style = { flexGrow = 1, marginLeft = 1, marginRight = 1 }
                  };
                  btn.Add(new Image { image = EditorGUIUtility.IconContent(iconName).image });

                  btn.RegisterCallback<MouseEnterEvent>(_ =>
                  {
                        GameObject[] selectedObjects = Selection.gameObjects;

                        if (selectedObjects is { Length: > 1 })
                        {
                              onPreview?.Invoke(selectedObjects);
                        }
                  });
                  btn.RegisterCallback<MouseLeaveEvent>(static _ => PreviewService.CancelPreview());

                  return btn;
            }

            private static Button CreateArrangementPreviewButton(string text, string tooltip, Action onClick, Action<IReadOnlyList<GameObject>, Vector3> onPreview)
            {
                  var btn = new Button(onClick)
                  {
                              text = text,
                              tooltip = tooltip + " (Hover to preview)",
                              style = { flexGrow = 1, marginLeft = 1, marginRight = 1 }
                  };

                  btn.RegisterCallback<MouseEnterEvent>(_ =>
                  {
                        GameObject[] selectedObjects = Selection.gameObjects;

                        if (selectedObjects is { Length: > 0 })
                        {
                              Vector3 currentSpacing = Vector3.right * 2f;
                              onPreview?.Invoke(selectedObjects, currentSpacing);
                        }
                  });
                  btn.RegisterCallback<MouseLeaveEvent>(static _ => PreviewService.CancelPreview());

                  return btn;
            }

            private static Button CreateRotationPreviewButton(string text, string tooltip, Action onClick, Action<IReadOnlyList<GameObject>> onPreview)
            {
                  var btn = new Button(onClick)
                  {
                              text = text,
                              tooltip = tooltip + " (Hover to preview)",
                              style = { flexGrow = 1, marginLeft = 1, marginRight = 1 }
                  };

                  btn.RegisterCallback<MouseEnterEvent>(_ =>
                  {
                        GameObject[] selectedObjects = Selection.gameObjects;

                        if (selectedObjects is { Length: > 0 })
                        {
                              onPreview?.Invoke(selectedObjects);
                        }
                  });
                  btn.RegisterCallback<MouseLeaveEvent>(static _ => PreviewService.CancelPreview());

                  return btn;
            }

            private static VisualElement CreateButtonRow()
            {
                  return new VisualElement { style = { flexDirection = FlexDirection.Row, marginTop = 5 } };
            }

            private static Foldout CreateSectionFoldout(string title, bool startExpanded = true)
            {
                  var foldout = new Foldout
                  {
                              text = title, value = startExpanded,
                              style =
                              {
                                          backgroundColor = new Color(0.15f, 0.15f, 0.15f, 0.5f), borderTopLeftRadius = 3, borderTopRightRadius = 3,
                                          borderBottomLeftRadius = 3, borderBottomRightRadius = 3, marginBottom = 8, paddingBottom = 2
                              }
                  };
                  foldout.contentContainer.style.paddingLeft = 10;
                  foldout.contentContainer.style.paddingTop = 5;
                  foldout.contentContainer.style.paddingRight = 5;

                  return foldout;
            }
      }
}