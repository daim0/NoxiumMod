using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NoxiumMod.Utilities
{
	public class BezierCurve
	{
		/// <summary>
		/// Creates a new bezier curve, based on an array of control points.
		/// </summary>
		public BezierCurve(Vector2[] controls)
		{
			controlPoints = controls;
		}

		/// <summary>
		/// Gets the specified amount of points on the bezier curve.
		/// </summary>
		public List<Vector2> GetPoints(int amount)
		{
			float reciprocalPointAmount = 1f / amount;
			List<Vector2> list = new List<Vector2>();
			for (float i = 0f; i <= 1f; i += reciprocalPointAmount)
			{
				list.Add(Evaluate(i));
			}
			return list;
		}

		private Vector2 Evaluate(float T)
		{
			T = MathHelper.Clamp(T, 0, 1);
			return PreEvaluate(controlPoints, T);
		}

		private Vector2 PreEvaluate(Vector2[] points, float T)
		{
			if (points.Length > 2)
			{
				Vector2[] newPoints = new Vector2[points.Length - 1];
				for (int i = 0; i < points.Length - 1; i++)
				{
					newPoints[i] = Vector2.Lerp(points[i], points[i + 1], T);
				}
				return PreEvaluate(newPoints, T);
			}
			return Vector2.Lerp(points[0], points[1], T);
		}

		private readonly Vector2[] controlPoints;
	}
}