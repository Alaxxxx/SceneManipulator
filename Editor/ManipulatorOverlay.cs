using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;
using OpalStudio.SceneManipulator.Services;

namespace OpalStudio.SceneManipulator
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
            }

            private void SubscribeToUIEvents()
            {
                  // Positioning
                  _ui.OnSetPositionClicked += (pos) => PositioningService.SetPosition(_selectionManager.SelectedObjects, pos);
                  _ui.OnCenterClicked += () => PositioningService.CenterObjects(_selectionManager.SelectedObjects);
                  _ui.OnSnapToGroundClicked += () => PositioningService.SnapToGround(_selectionManager.SelectedObjects);
                  _ui.OnMoveToOriginClicked += () => PositioningService.MoveToOrigin(_selectionManager.SelectedObjects);

                  // Alignment
                  _ui.OnAlignClicked += (axis, target) => AlignmentService.AlignObjects(_selectionManager.SelectedObjects, axis, target);

                  // Rotation
                  _ui.OnSetRotationClicked += (rot) => RotationService.SetRotation(_selectionManager.SelectedObjects, rot);
                  _ui.OnResetRotationClicked += () => RotationService.ResetRotation(_selectionManager.SelectedObjects);
                  _ui.OnAddRotationClicked += (add) => RotationService.AddRotation(_selectionManager.SelectedObjects, add);
                  _ui.OnRandomizeYRotationClicked += () => RotationService.RandomizeRotation(_selectionManager.SelectedObjects, Vector3.zero, new Vector3(0, 360, 0));

                  // Arrangement
                  _ui.OnSpacingChanged += (spacing) => _spacing = spacing;
                  _ui.OnArrangeInLineClicked += (dir) => ArrangementService.ArrangeInLine(_selectionManager.SelectedObjects, dir, _spacing.magnitude);
                  _ui.OnArrangeInGridClicked += () => ArrangementService.ArrangeInGrid(_selectionManager.SelectedObjects, _spacing);
                  _ui.OnArrangeInCircleClicked += () => ArrangementService.ArrangeInCircle(_selectionManager.SelectedObjects, _spacing.magnitude);
                  _ui.OnArrangeRandomlyClicked += () => ArrangementService.ArrangeRandomly(_selectionManager.SelectedObjects, _spacing.magnitude);

                  // Utilities
                  _ui.OnDuplicateClicked += () => UtilityService.DuplicateSelected(_selectionManager.SelectedObjects);

                  _ui.OnDeleteClicked += () =>
                  {
                        UtilityService.DeleteSelected(_selectionManager.SelectedObjects);
                        _selectionManager.Clear();
                  };
                  _ui.OnGroupClicked += () => UtilityService.GroupSelected(_selectionManager.SelectedObjects);
            }

            public override void OnWillBeDestroyed()
            {
                  if (_selectionManager != null)
                  {
                        _selectionManager.OnSelectionChanged -= HandleSelectionChanged;
                        _selectionManager.OnWillBeDestroyed();
                  }

                  base.OnWillBeDestroyed();
            }
      }
}