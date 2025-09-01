using System.Collections.Generic;
using OpalStudio.SceneManipulator.Editor.Services;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace OpalStudio.SceneManipulator.Editor
{
      [Overlay(typeof(SceneView), "Manipulator", true)]
      public class ManipulatorOverlay : Overlay
      {
            private ManipulatorUI _ui;
            private SelectionManager _selectionManager;
            private Vector3 _spacing = Vector3.right * 2f;

            public override VisualElement CreatePanelContent()
            {
                  _selectionManager = new SelectionManager();
                  _selectionManager.Initialize();
                  _selectionManager.OnSelectionChanged += HandleSelectionChanged;

                  _ui = new ManipulatorUI();
                  VisualElement root = _ui.CreatePanel();

                  SubscribeToUIEvents();

                  HandleSelectionChanged(_selectionManager.SelectedObjects);

                  return root;
            }

            private void HandleSelectionChanged(IReadOnlyList<GameObject> selectedObjects)
            {
                  string info = MeasurementService.GetMeasurementInfo(selectedObjects);
                  _ui.UpdateMeasurementInfo(info);

                  if (VisualizationService.ShowDistanceGizmos)
                  {
                        VisualizationService.UpdateConnections(selectedObjects);
                  }

                  PreviewService.CancelPreview();
            }

            private void SubscribeToUIEvents()
            {
                  _ui.OnSetPositionClicked += pos =>
                  {
                        PreviewService.CancelPreview();
                        PositioningService.SetPosition(_selectionManager.SelectedObjects, pos);
                  };

                  _ui.OnCenterClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        PositioningService.CenterObjects(_selectionManager.SelectedObjects);
                  };

                  _ui.OnMoveToOriginClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        PositioningService.MoveToOrigin(_selectionManager.SelectedObjects);
                  };

                  _ui.OnAlignClicked += (axis, target) =>
                  {
                        PreviewService.CancelPreview();
                        AlignmentService.AlignObjects(_selectionManager.SelectedObjects, axis, target);
                  };

                  _ui.OnSetRotationClicked += rot =>
                  {
                        PreviewService.CancelPreview();
                        RotationService.SetRotation(_selectionManager.SelectedObjects, rot);
                  };

                  _ui.OnResetRotationClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        RotationService.ResetRotation(_selectionManager.SelectedObjects);
                  };

                  _ui.OnAddRotationClicked += add =>
                  {
                        PreviewService.CancelPreview();
                        RotationService.AddRotation(_selectionManager.SelectedObjects, add);
                  };

                  _ui.OnRandomizeYRotationClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        RotationService.RandomizeRotation(_selectionManager.SelectedObjects, Vector3.zero, new Vector3(0, 360, 0));
                  };

                  _ui.OnSpacingChanged += spacing =>
                  {
                        _spacing = spacing;

                        PreviewService.CancelPreview();
                  };

                  _ui.OnArrangeInLineClicked += dir =>
                  {
                        PreviewService.CancelPreview();
                        ArrangementService.ArrangeInLine(_selectionManager.SelectedObjects, dir, _spacing.magnitude);
                        UpdateVisualizationAfterChange();
                  };

                  _ui.OnArrangeInGridClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        ArrangementService.ArrangeInGrid(_selectionManager.SelectedObjects, _spacing);
                        UpdateVisualizationAfterChange();
                  };

                  _ui.OnArrangeInCircleClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        ArrangementService.ArrangeInCircle(_selectionManager.SelectedObjects, _spacing.magnitude);
                        UpdateVisualizationAfterChange();
                  };

                  _ui.OnArrangeRandomlyClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        ArrangementService.ArrangeRandomly(_selectionManager.SelectedObjects, _spacing.magnitude);
                        UpdateVisualizationAfterChange();
                  };

                  _ui.OnSnapToGroundAdvancedClicked += alignToNormal =>
                  {
                        PreviewService.CancelPreview();
                        SnapService.SnapToGround(_selectionManager.SelectedObjects, alignToNormal);
                        UpdateVisualizationAfterChange();
                  };

                  _ui.OnSnapToBoundsClicked += direction =>
                  {
                        PreviewService.CancelPreview();
                        SnapService.SnapToFirstSelected(_selectionManager.SelectedObjects, direction);
                        UpdateVisualizationAfterChange();
                  };

                  _ui.OnSnapToGridClicked += gridSize =>
                  {
                        PreviewService.CancelPreview();
                        SnapService.SnapToGrid(_selectionManager.SelectedObjects, gridSize);
                        UpdateVisualizationAfterChange();
                  };

                  _ui.OnStackObjectsClicked += padding =>
                  {
                        PreviewService.CancelPreview();
                        SnapService.StackObjects(_selectionManager.SelectedObjects, padding);
                        UpdateVisualizationAfterChange();
                  };

                  _ui.OnDuplicateClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        UtilityService.DuplicateSelected(_selectionManager.SelectedObjects);
                  };

                  _ui.OnDeleteClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        UtilityService.DeleteSelected(_selectionManager.SelectedObjects);
                        _selectionManager.Clear();
                        VisualizationService.ClearConnections();
                  };

                  _ui.OnGroupClicked += () =>
                  {
                        PreviewService.CancelPreview();
                        UtilityService.GroupSelected(_selectionManager.SelectedObjects);
                  };
            }

            private void UpdateVisualizationAfterChange()
            {
                  string info = MeasurementService.GetMeasurementInfo(_selectionManager.SelectedObjects);
                  _ui.UpdateMeasurementInfo(info);

                  if (VisualizationService.ShowDistanceGizmos)
                  {
                        VisualizationService.UpdateConnections(_selectionManager.SelectedObjects);
                  }
            }

            public override void OnWillBeDestroyed()
            {
                  PreviewService.CancelPreview();
                  VisualizationService.ClearConnections();

                  if (_selectionManager != null)
                  {
                        _selectionManager.OnSelectionChanged -= HandleSelectionChanged;
                        _selectionManager.OnWillBeDestroyed();
                  }

                  base.OnWillBeDestroyed();
            }
      }
}