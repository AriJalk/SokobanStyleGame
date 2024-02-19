using UnityEngine;

public static class GameUtilities
{
    public static Color Red = new Color(0.6698113f, 0.2180046f, 0.2180046f, 1f);
    public static Color Blue = new Color(0.2021627f, 0.3961491f, 0.5566037f, 1f);

    /// <summary>
    /// Returns if position is in bounds of the grid
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool IsPositionInBounds(object[,] grid, Vector2Int position)
    {
        bool result = false;
        if (position.x >= 0 && position.x < grid.GetLength(0) &&
            position.y >= 0 && position.y < grid.GetLength(1))
        {
            result = true;
        }
        return result;
    }

    /// <summary>
    /// Set the child parent and reset position to 0,0,0 and scale to 1,1,1
    /// </summary>
    /// <param name="child"></param>
    /// <param name="parent"></param>
    public static void SetParentAndResetPosition(Transform child, Transform parent)
    {
        child.SetParent(parent);
        child.localPosition = Vector3.zero;
        child.localScale = Vector3.one;
    }

    public static void SetAnchorsAndResetPosition(RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax)
    {
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.sizeDelta = Vector2.zero;
    }

    public static void ResetAnchorsAndSize(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
    }

    public static GameActions GetOppositeAction(GameActions action)
    {
        switch (action)
        {
            case GameActions.MoveLeft:
                return GameActions.MoveRight;
            case GameActions.MoveRight:
                return GameActions.MoveLeft;
            case GameActions.MoveUp:
                return GameActions.MoveDown;
            case GameActions.MoveDown:
                return GameActions.MoveUp;
            default:
                return GameActions.Null;
        }
    }

    public static Color GameColorToUnityRGB(GameColors color)
    {
        switch (color)
        {
            case GameColors.Red:
                return Red;
            case GameColors.Blue:
                return Blue;
            default:
                return Color.clear;
        }
    }

    public static string ArrayToString<T>(T[,] array)
    {
        string str = string.Empty;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                str += array[i, j];
            }
            str += "\n";
        }
        return str;
    }

    public static void CopyStringToClipboard(string str)
    {
        GUIUtility.systemCopyBuffer = str;
        Debug.Log("String copied to clipboard: " + str);
    }

    public static void SquareAnchors(RectTransform rectTransform, Transform parentObject)
    {
        // Get the parent container's RectTransform
        RectTransform parentRect = parentObject.GetComponent<RectTransform>();

        // Calculate the reference size (width or height)
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        // Calculate the normalized size of the object relative to the parent container
        float normalizedSize = Mathf.Min(parentWidth, parentHeight);

        // Set the anchors to achieve a square proportion within the parent container
        rectTransform.anchorMin = new Vector2(0.5f - normalizedSize / (2f * parentWidth), 0.5f - normalizedSize / (2f * parentHeight));
        rectTransform.anchorMax = new Vector2(0.5f + normalizedSize / (2f * parentWidth), 0.5f + normalizedSize / (2f * parentHeight));
    }

    /// <summary>
    /// Transform editor position to corelate unity coordinates for game mode
    /// </summary>
    /// <param name="editorPosition"></param>
    /// <param name="gridSize"></param>
    /// <returns></returns>
    public static Vector2Int EditorToGamePosition(Vector2Int editorPosition, int gridSize)
    {
        return new Vector2Int(editorPosition.x, gridSize - editorPosition.y - 1);
    }

    public static GameDirection GetOppositeDirection(GameDirection direction)
    {
        switch (direction)
        {
            case GameDirection.Left:
                return GameDirection.Right;
            case GameDirection.Right:
                return GameDirection.Left;
            case GameDirection.Up:
                return GameDirection.Down;
            case GameDirection.Down:
                return GameDirection.Up;
            case GameDirection.Neutral:
            default:
                return GameDirection.Neutral;
        }
    }

    public static Vector2Int GetDirectionVector(GameDirection direction)
    {
        switch (direction)
        {
            case GameDirection.Left:
                return Vector2Int.left;
            case GameDirection.Right:
                return Vector2Int.right;
            case GameDirection.Up:
                return Vector2Int.up;
            case GameDirection.Down:
                return Vector2Int.down;
            case GameDirection.Neutral:
            default:
                return Vector2Int.zero;
        }
    }

    public static Vector2Int GetPositionInDirection(Vector2Int origin, GameDirection direction)
    {
        Vector2Int position;
        switch (direction)
        {
            case GameDirection.Left:
                position = origin + Vector2Int.left;
                break;
            case GameDirection.Right:
                position = origin + Vector2Int.right;
                break;
            case GameDirection.Up:
                position = origin + Vector2Int.up;
                break;
            case GameDirection.Down:
                position = origin + Vector2Int.down;
                break;
            case GameDirection.Neutral:
            default:
                position = origin;
                break;
        }
        return position;
    }
}