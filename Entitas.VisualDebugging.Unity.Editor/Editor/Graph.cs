using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DesperateDevs.Unity.Editor
{
	public class Graph
	{
		public float xBorder = 48f;

		public float yBorder = 20f;

		public int rightLinePadding = -15;

		public string labelFormat = "{0:0.0}";

		public string axisFormat = "{0:0.0}";

		public int gridLines = 1;

		public float axisRounding = 1f;

		public float anchorRadius = 1f;

		public Color lineColor = Color.magenta;

		private readonly GUIStyle _labelTextStyle;

		private readonly GUIStyle _centeredStyle;

		private readonly Vector3[] _cachedLinePointVerticies;

		private readonly Vector3[] _linePoints;

		public Graph(int dataLength)
		{
			_labelTextStyle = new GUIStyle(GUI.skin.label);
			_labelTextStyle.alignment = TextAnchor.UpperRight;
			_centeredStyle = new GUIStyle();
			_centeredStyle.alignment = TextAnchor.UpperCenter;
			_centeredStyle.normal.textColor = Color.white;
			_linePoints = new Vector3[dataLength];
			_cachedLinePointVerticies = new Vector3[4]
			{
				new Vector3(-1f, 1f, 0f) * anchorRadius,
				new Vector3(1f, 1f, 0f) * anchorRadius,
				new Vector3(1f, -1f, 0f) * anchorRadius,
				new Vector3(-1f, -1f, 0f) * anchorRadius
			};
		}

		public void Draw(float[] data, float height)
		{
			Rect rect = GUILayoutUtility.GetRect(EditorGUILayout.GetControlRect().width, height);
			float num = rect.y + yBorder;
			float num2 = rect.y + rect.height - yBorder;
			float availableHeight = num2 - num;
			float num3 = (data.Length != 0) ? data.Max() : 0f;
			if (num3 % axisRounding != 0f)
			{
				num3 = num3 + axisRounding - num3 % axisRounding;
			}
			drawGridLines(num, rect.width, availableHeight, num3);
			drawAvg(data, num, num2, rect.width, availableHeight, num3);
			drawLine(data, num2, rect.width, availableHeight, num3);
		}

		private void drawGridLines(float top, float width, float availableHeight, float max)
		{
			Color color = Handles.color;
			Handles.color = Color.grey;
			int num = gridLines + 1;
			float num2 = availableHeight / (float)num;
			for (int i = 0; i <= num; i++)
			{
				float num3 = top + num2 * (float)i;
				Handles.DrawLine(new Vector2(xBorder, num3), new Vector2(width - (float)rightLinePadding, num3));
				GUI.Label(new Rect(0f, num3 - 8f, xBorder - 2f, 50f), string.Format(axisFormat, max * (1f - (float)i / (float)num)), _labelTextStyle);
			}
			Handles.color = color;
		}

		private void drawAvg(float[] data, float top, float floor, float width, float availableHeight, float max)
		{
			Color color = Handles.color;
			Handles.color = Color.yellow;
			float num = data.Average();
			float y = floor - availableHeight * (num / max);
			Handles.DrawLine(new Vector2(xBorder, y), new Vector2(width - (float)rightLinePadding, y));
			Handles.color = color;
		}

		private void drawLine(float[] data, float floor, float width, float availableHeight, float max)
		{
			float num = (width - xBorder - (float)rightLinePadding) / (float)data.Length;
			Color color = Handles.color;
			Rect position = default(Rect);
			bool flag = false;
			float num2 = 0f;
			Handles.color = lineColor;
			Handles.matrix = Matrix4x4.identity;
			HandleUtility.handleMaterial.SetPass(0);
			for (int i = 0; i < data.Length; i++)
			{
				float num3 = data[i];
				float y = floor - availableHeight * (num3 / max);
				Vector2 vector = new Vector2(xBorder + num * (float)i, y);
				_linePoints[i] = new Vector3(vector.x, vector.y, 0f);
				float d = 1f;
				if (!flag)
				{
					float num4 = anchorRadius * 3f;
					float num5 = anchorRadius * 6f;
					Vector2 vector2 = vector - Vector2.up * 0.5f;
					position = new Rect(vector2.x - num4, vector2.y - num4, num5, num5);
					if (position.Contains(Event.current.mousePosition))
					{
						flag = true;
						num2 = num3;
						d = 3f;
					}
				}
				Handles.matrix = Matrix4x4.TRS(_linePoints[i], Quaternion.identity, Vector3.one * d);
				Handles.DrawAAConvexPolygon(_cachedLinePointVerticies);
			}
			Handles.matrix = Matrix4x4.identity;
			Handles.DrawAAPolyLine(2f, data.Length, _linePoints);
			if (flag)
			{
				position.y -= 16f;
				position.width += 50f;
				position.x -= 25f;
				GUI.Label(position, string.Format(labelFormat, num2), _centeredStyle);
			}
			Handles.color = color;
		}
	}
}
