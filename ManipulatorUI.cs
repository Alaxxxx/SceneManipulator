using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace OpalStudio.SceneManipulator
{
      public sealed class ManipulatorUI
      {
            // Events for Positioning
            public event Action<Vector3> OnSetPositionClicked;
            public event Action OnCenterClicked;
            public event Action OnSnapToGroundClicked;
            public event Action OnMoveToOriginClicked;

            // Events for alignment
            public event Action<Services.AlignmentService.Axis, Services.AlignmentService.Target> OnAlignClicked;

            // Events for Rotation
            public event Action<Vector3> OnSetRotationClicked;
            public event Action OnResetRotationClicked;
            public event Action<Vector3> OnAddRotationClicked;
            public event Action OnRandomizeYRotationClicked;

            // Events for Arrangement and Utilities
            public event Action<Vector3> OnSpacingChanged;
            public event Action<Vector3> OnArrangeInLineClicked;
            public event Action OnArrangeInGridClicked;
            public event Action OnArrangeInCircleClicked;
            public event Action OnArrangeRandomlyClicked;
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
                  quickPosRow.Add(CreateButton("Ground", "Snap objects to the nearest surface below", () => OnSnapToGroundClicked?.Invoke()));
                  quickPosRow.Add(CreateButton("Origin", "Move all selected objects to (0,0,0)", () => OnMoveToOriginClicked?.Invoke()));
                  section.Add(quickPosRow);
            }

            private void CreateAlignmentSection()
            {
                  Foldout section = CreateSectionFoldout("Alignment", false);
                  _root.Add(section);

                  VisualElement rowX = CreateButtonRow();
                  rowX.Add(new Label("X") { style = { width = 12 } });

                  rowX.Add(CreateIconButton("d_align_horizontally_left_active", "Align Min X",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.X, Services.AlignmentService.Target.Min)));

                  rowX.Add(CreateIconButton("d_align_horizontally_center_active", "Align Center X",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.X, Services.AlignmentService.Target.Center)));

                  rowX.Add(CreateIconButton("d_align_horizontally_right_active", "Align Max X",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.X, Services.AlignmentService.Target.Max)));
                  section.Add(rowX);

                  VisualElement rowY = CreateButtonRow();
                  rowY.Add(new Label("Y") { style = { width = 12 } });

                  rowY.Add(CreateIconButton("d_align_vertically_bottom_active", "Align Min Y",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.Y, Services.AlignmentService.Target.Min)));

                  rowY.Add(CreateIconButton("d_align_vertically_center_active", "Align Center Y",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.Y, Services.AlignmentService.Target.Center)));

                  rowY.Add(CreateIconButton("d_align_vertically_top_active", "Align Max Y",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.Y, Services.AlignmentService.Target.Max)));
                  section.Add(rowY);

                  VisualElement rowZ = CreateButtonRow();
                  rowZ.Add(new Label("Z") { style = { width = 12 } });

                  rowZ.Add(CreateIconButton("d_align_horizontally_left_active", "Align Min Z (Forward)",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.Z, Services.AlignmentService.Target.Min)));

                  rowZ.Add(CreateIconButton("d_align_horizontally_center_active", "Align Center Z",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.Z, Services.AlignmentService.Target.Center)));

                  rowZ.Add(CreateIconButton("d_align_horizontally_right_active", "Align Max Z (Back)",
                              () => OnAlignClicked?.Invoke(Services.AlignmentService.Axis.Z, Services.AlignmentService.Target.Max)));
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
                  quickRotRow2.Add(CreateButton("+90° Y", "Add 90 degrees to Y rotation", () => OnAddRotationClicked?.Invoke(new Vector3(0, 90, 0))));
                  quickRotRow2.Add(CreateButton("-90° Y", "Subtract 90 degrees from Y rotation", () => OnAddRotationClicked?.Invoke(new Vector3(0, -90, 0))));
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
                  arrangeRow1.Add(CreateButton("Line X", "Arrange along the X axis", () => OnArrangeInLineClicked?.Invoke(Vector3.right)));
                  arrangeRow1.Add(CreateButton("Line Y", "Arrange along the Y axis", () => OnArrangeInLineClicked?.Invoke(Vector3.up)));
                  arrangeRow1.Add(CreateButton("Line Z", "Arrange along the Z axis", () => OnArrangeInLineClicked?.Invoke(Vector3.forward)));
                  section.Add(arrangeRow1);

                  VisualElement arrangeRow2 = CreateButtonRow();
                  arrangeRow2.Add(CreateButton("Grid", "Arrange in a grid on the XZ plane", () => OnArrangeInGridClicked?.Invoke()));
                  arrangeRow2.Add(CreateButton("Circle", "Arrange in a circle on the XZ plane", () => OnArrangeInCircleClicked?.Invoke()));
                  arrangeRow2.Add(CreateButton("Random", "Arrange randomly around the first object", () => OnArrangeRandomlyClicked?.Invoke()));
                  section.Add(arrangeRow2);
            }

            private void CreateMeasurementSection()
            {
                  Foldout section = CreateSectionFoldout("Measurement", false);
                  _root.Add(section);

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

            private static Button CreateIconButton(string iconName, string tooltip, Action onClick)
            {
                  var btn = new Button(onClick)
                  {
                              tooltip = tooltip,
                              style = { flexGrow = 1, marginLeft = 1, marginRight = 1 }
                  };
                  btn.Add(new Image { image = EditorGUIUtility.IconContent(iconName).image });

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