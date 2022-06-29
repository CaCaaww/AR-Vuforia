using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to create a flexible grid layout
/// </summary>
public class FlexibleGridLayout : LayoutGroup
{   
    #region Internal classes
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FiexdColumns
    }
    #endregion

    #region Inspector
    public FitType fitType;
    public int rows;
    public int columns;
    public Vector2 cellSize;
    public Vector2 spacing;
    public bool fitX;
    public bool fitY;
    #endregion

    #region Internal methods
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        // Check the fit type
        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {  
            // Find the numer of rows and columns by finding the square root of the number of the children in the transform
            float sqRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqRt);
            columns = Mathf.CeilToInt(sqRt);
        }
        if (fitType == FitType.Width || fitType == FitType.FiexdColumns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        // Get the width and the height of our container so we kwon how mush space we are able to work with
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        // Define a size for each of the children based of the infos we now have
        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * 2) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        // Assign these values to the x and y position to the cellSize variable
        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        // Keep count of our row and column indexes as we lay everything out
        int columnCount = 0;
        int rowCount = 0;

        // Iterate over all the childern in the rect transform
        for(int i=0; i < rectChildren.Count; i++)
        {
            // Find the current row index and current column index
            rowCount = i / columns;
            columnCount = i % columns;

            // Get a reference to the current child object
            var item = rectChildren[i];

            // Get a refence to an x and y position
            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount)+ (spacing.y * rowCount) + padding.top;

            // Set the child position along the axes
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);        
        }
    }

    public override void CalculateLayoutInputVertical() {}

    public override void SetLayoutHorizontal() {}

    public override void SetLayoutVertical() {}
    #endregion
}
