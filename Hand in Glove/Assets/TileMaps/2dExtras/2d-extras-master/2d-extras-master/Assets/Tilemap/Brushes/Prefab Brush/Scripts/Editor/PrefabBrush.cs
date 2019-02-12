using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace UnityEditor
{
    [CreateAssetMenu(fileName = "Prefab brush", menuName = "Brushes/Prefab brush")]
	[CustomGridBrush(false, true, false, "Prefab Brush")]
	public class PrefabBrush : GridBrushBase
	{
        public GameObject prefab;
        public float rotation;

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

            GameObject instance = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
			if (instance != null)
			{
				Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
				Undo.RegisterCreatedObjectUndo((UnityEngine.Object)instance, "Paint Prefabs");
				instance.transform.SetParent(brushTarget.transform);
				instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, 0) + new Vector3(.5f, .5f, .5f)));
                instance.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            }
		}

        //private static void SetRotation(GameObject instance, Vector2 cellsize)
        //{
        //    Vector3 pos = instance.transform.position;

        //    bool topUsed = Physics2D.Raycast(pos + new Vector3(0f, cellsize.y, .1f), new Vector3(0f, 0f, -.2f));
        //    bool rightUsed = Physics2D.Raycast(pos + new Vector3(cellsize.x, 0f, .1f), new Vector3(0f, 0f, -.2f));
        //    bool botUsed = Physics2D.Raycast(pos + new Vector3(0f, -cellsize.y, .1f), new Vector3(0f, 0f, -.2f));
        //    bool leftUsed = Physics2D.Raycast(pos + new Vector3(-cellsize.x, 0f, .1f), new Vector3(0f, 0f, -.2f));
        //    if ((topUsed && rightUsed && botUsed && !leftUsed) || (!topUsed && rightUsed && !botUsed && !botUsed)) instance.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        //}

        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

			Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, 0));
			if (erased != null)
				Undo.DestroyObjectImmediate(erased.gameObject);
		}

		private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
		{
			int childCount = parent.childCount;
			Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
			Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
			Bounds bounds = new Bounds((max + min)*.5f, max - min);

			for (int i = 0; i < childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (bounds.Contains(child.position))
					return child;
			}
			return null;
		}

		private static float GetPerlinValue(Vector3Int position, float scale, float offset)
		{
			return Mathf.PerlinNoise((position.x + offset)*scale, (position.y + offset)*scale);
		}
	}

	//[CustomEditor(typeof(PrefabBrush))]
	//public class PrefabBrushEditor : GridBrushEditorBase
	//{
	//	private PrefabBrush prefabBrush { get { return target as PrefabBrush; } }

	//	//private SerializedProperty m_Prefabs;
	//	//private SerializedObject m_SerializedObject;

	//	protected void OnEnable()
	//	{
	//		m_SerializedObject = new SerializedObject(target);
	//		//m_Prefabs = m_SerializedObject.FindProperty("prefab");
	//	}

	//	public override void OnPaintInspectorGUI()
	//	{
	//		//m_SerializedObject.UpdateIfRequiredOrScript();
	//		//prefabBrush.m_Z = EditorGUILayout.IntField("Position Z", prefabBrush.m_Z);
				
	//		//EditorGUILayout.PropertyField(prefab, true);
	//		//m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
	//	}
	//}
}
