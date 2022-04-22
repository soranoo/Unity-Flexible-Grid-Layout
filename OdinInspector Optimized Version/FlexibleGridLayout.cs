using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

/// ----------------Script-Info--------------------
/// Reference:
/// https://github.com/soranoo/GameDevGuide-CustomTabsAndFlexibleGrid/blob/master/Custom%20Tabs%20and%20Flexible%20Grid/Assets/Scripts/FlexibleGridLayout.cs
/// -------------------END-------------------------

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }

    public FitType fitType = FitType.Uniform;

    [Min(1), ShowIf("@fitType == FitType.FixedRows")]
    public int Rows;
    [Min(1), ShowIf("@fitType == FitType.FixedColumns")]
    public int Columns;

    [DisableIf("@fitType == FitType.Uniform")]
    public Vector2 CellSize = new Vector2(500, 500);
    public Vector2 Spacing;

    [TitleGroup("Fit"), LabelText("Width"), EnableIf("@fitType == FitType.FixedRows || fitType == FitType.FixedColumns")]
    public bool FitX;
    [TitleGroup("Fit"), LabelText("Height"), EnableIf("@fitType == FitType.FixedRows || fitType == FitType.FixedColumns")]
    public bool FitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        int _childCount = 0;
        // calculate the number of children (without inactive child)
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf == true)
                _childCount++;
        }

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            float _squareRoot = Mathf.Sqrt(_childCount);
            Columns = Rows = Mathf.CeilToInt(_squareRoot) == 0 ? 1 : Mathf.CeilToInt(_squareRoot);
            switch (fitType)
            {
                case FitType.Width:
                    FitX = true;
                    FitY = false;
                    break;
                case FitType.Height:
                    FitX = false;
                    FitY = true;
                    break;
                case FitType.Uniform:
                    FitX = FitY = true;
                    break;
            }
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            Rows = Mathf.CeilToInt(_childCount / Columns) == 0 ? 1 : Mathf.CeilToInt(_childCount / Columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            Columns = Mathf.CeilToInt(_childCount / Rows) == 0 ? 1 : Mathf.CeilToInt(_childCount / Rows);
        }


        float _totalWidth = rectTransform.rect.width;
        float _totalHeight = rectTransform.rect.height;

        float _cellMaxWidth = _totalWidth / Columns - ((Spacing.x / Columns) * (Columns - 1))
            - (padding.left / Columns) - (padding.right / Columns);
        float _cellMaxHeight = _totalHeight / Rows - ((Spacing.y / Rows) * (Rows - 1))
            - (padding.top / Rows) - (padding.bottom / Rows); ;

        CellSize.x = FitX ? _cellMaxWidth : CellSize.x;
        CellSize.y = FitY ? _cellMaxHeight : CellSize.y;


        for (int i = 0; i < rectChildren.Count; i++)
        {
            int _rowCount = i / Columns;
            int _columnCount = i % Columns;

            float _columnWidth = CellSize.x * Columns + Spacing.x * (Columns - 1);
            float _rowHeight = CellSize.y * Rows + Spacing.y * (Rows - 1);

            RectTransform _child = rectChildren[i];

            float _xPos = (CellSize.x * _columnCount) + (Spacing.x * _columnCount) + padding.left - padding.right;
            float _yPos = (CellSize.y * _rowCount) + (Spacing.y * _rowCount) + padding.top - padding.bottom;

            float _xLeft = _xPos;
            float _xCenter = (_totalWidth - _columnWidth) / 2 + _xPos;
            float _xRight = _totalWidth - _columnWidth + _xPos;

            float _yTop = _yPos;
            float _yCenter = (_totalHeight - _rowHeight) / 2 + _yPos;
            float _yBottom = _totalHeight - _rowHeight + _yPos;

            switch (m_ChildAlignment)
            {
                default:
                case TextAnchor.UpperLeft:
                    _xPos = _xLeft;
                    _yPos = _yTop;
                    break;
                case TextAnchor.UpperCenter:
                    _xPos = _xCenter;
                    break;
                case TextAnchor.UpperRight:
                    _xPos = _xRight;
                    break;
                case TextAnchor.MiddleLeft:
                    _yPos = _yCenter;
                    break;
                case TextAnchor.MiddleCenter:
                    _xPos = _xCenter;
                    _yPos = _yCenter;
                    break;
                case TextAnchor.MiddleRight:
                    _xPos = _xRight;
                    _yPos = _yCenter;
                    break;
                case TextAnchor.LowerLeft:
                    _yPos = _yBottom;
                    break;
                case TextAnchor.LowerCenter:
                    _xPos = _xCenter;
                    _yPos = _yBottom;
                    break;
                case TextAnchor.LowerRight:
                    _xPos = _xRight;
                    _yPos = _yBottom;
                    break;
            }

            SetChildAlongAxis(_child, 0, _xPos, CellSize.x);
            SetChildAlongAxis(_child, 1, _yPos, CellSize.y);

        }
    }

    public override void CalculateLayoutInputVertical()
    {

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
}