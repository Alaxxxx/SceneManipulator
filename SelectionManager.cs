using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace OpalStudio.SceneManipulator
{
      public class SelectionManager
      {
            public IReadOnlyList<GameObject> SelectedObjects => _selectedObjects;
            private List<GameObject> _selectedObjects = new();

            public event Action<IReadOnlyList<GameObject>> OnSelectionChanged;

            public void Initialize()
            {
                  Selection.selectionChanged += UpdateSelectedObjects;
                  UpdateSelectedObjects();
            }

            private void UpdateSelectedObjects()
            {
                  _selectedObjects = Selection.gameObjects.ToList();
                  OnSelectionChanged?.Invoke(_selectedObjects);
            }

            public void Clear()
            {
                  _selectedObjects.Clear();
                  OnSelectionChanged?.Invoke(_selectedObjects);
            }

            public void OnWillBeDestroyed()
            {
                  Selection.selectionChanged -= UpdateSelectedObjects;
            }
      }
}