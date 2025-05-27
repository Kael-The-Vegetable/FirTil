using UnityEngine;

public static class IntTypeExtensions
{
	public static Vector2Int RandomPointOnBorder(this RectInt rect)
	{
		int[] sides = new int[4]
		{ // top, bottom, left, right
			rect.width, rect.width, rect.height, rect.height
		};
		int total = (rect.width + rect.height) * 2;
		int rand = Random.Range(0, total + 1);

		int i = 0;
		while (rand > 0)
		{
			if (rand > sides[i])
			{
				rand -= sides[i];
				i++;
			} else
			{
				break;
			}
		}

		return new Vector2Int();
	}
}
