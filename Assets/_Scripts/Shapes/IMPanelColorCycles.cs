using Shapes;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class IMPanelColorCycles : ImmediateModePanel
{
    [SerializeField]
    private CyclesManager _manager;
    public CyclesManager Manager => _manager;

    [Header("Style")]
    [SerializeField] float _radius = 2500f;
    [SerializeField] float _thickness = 1000f;
    [SerializeField] float _bordersThickness = 50f;
    [SerializeField] float _needleThickess = 100f;
    [SerializeField] float _needleCenterOffset = 2f;

    #region Serialized in custom Editor
    public Color[] _cyclesColors;
    #endregion

    public override void DrawPanelShapes(Rect rect, ImCanvasContext ctx)
    {
        if (CyclesManager.Instance == null)
            return;

        if (GameManager.Instance.State != GameState.BeginCycles)
            return;

        var manager = CyclesManager.Instance;
        float angleDeg = manager.NeedleAngleDeg;
        int nbCycles = manager.NbCycles;
        float degreesPerCycle = manager.DegreesPerCycle;

        Draw.ZTest = CompareFunction.Always;
        Draw.BlendMode = ShapesBlendMode.Transparent;
        Draw.RadiusSpace = ThicknessSpace.Meters;
        Draw.ThicknessSpace = ThicknessSpace.Meters;

        // Arcs
        for (int i = 0; i < nbCycles; i++)
        {
            float angleStart = -i * degreesPerCycle * Mathf.Deg2Rad;
            float angleEnd = -(i + 1) * degreesPerCycle * Mathf.Deg2Rad;

            Draw.Arc(Vector3.zero, _radius, _thickness, angleStart, angleEnd, ArcEndCap.None, _cyclesColors[i]);

            // Fake glow effect using additive, only for active Arc
            if (CyclesManager.Instance.CurrentSegmentIndex == i)
            {
                Draw.BlendMode = ShapesBlendMode.Additive;
                for (int iAdditive = 0; iAdditive < 10; iAdditive++)
                {
                    Draw.Arc(Vector3.zero, _radius, _thickness, angleStart, angleEnd, ArcEndCap.None, _cyclesColors[i]);
                }
                Draw.BlendMode = ShapesBlendMode.Transparent;
            }
        }

        // Borders
        float inner = _radius - (_thickness * 0.5f);
        float outer = _radius + (_thickness * 0.5f);
        Draw.Ring(Vector3.zero, inner, _bordersThickness, Color.black);
        Draw.Ring(Vector3.zero, outer, _bordersThickness, Color.black);

        // Needle
        float angleRad = -angleDeg * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f);
        Vector3 start = Vector3.zero + dir * _needleCenterOffset;
        Vector3 end = Vector3.zero + dir * (outer - _needleThickess / 2);

        Draw.Line(start, end, _needleThickess, LineEndCap.Square, Color.black);
    }

}
