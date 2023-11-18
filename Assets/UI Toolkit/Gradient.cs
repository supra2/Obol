using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gradient : VisualElement
{

    public new class UxmlFactory : UxmlFactory<Gradient, GradientUxmlTraits> { }

    public class GradientUxmlTraits : UxmlTraits
    {
        UxmlColorAttributeDescription leftColor =
            new UxmlColorAttributeDescription { name = "left-color", defaultValue = Color.red };
        UxmlColorAttributeDescription rightColor =
            new UxmlColorAttributeDescription { name = "right-color", defaultValue = Color.black };
        
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            if (ve == null)
                throw new ArgumentNullException(nameof(ve));

            var grad = (Gradient)ve;
            grad._leftColor = leftColor.GetValueFromBag(bag, cc);
            grad._rightColor = rightColor.GetValueFromBag(bag, cc);
        }
    }

    public Gradient()
    {
        generateVisualContent += GenerateVisualContent;
    }

    private Color _leftColor;
    private Color _rightColor;

    public Color LeftColor
    {
        get => _leftColor;
        set => _leftColor = value;
    }

    public Color RightColor
    {
        get => _rightColor;
        set => _rightColor = value;
    }

    static readonly Vertex[] vertices = new Vertex[4];

    static readonly ushort[] indices = { 0, 1, 2, 2, 3, 0 };

    void GenerateVisualContent(MeshGenerationContext mgc)
    {
        var rect = contentRect;
        if (rect.width < 0.1f || rect.height < 0.1f)
            return;

        vertices[0].tint = _leftColor;
        vertices[1].tint = _leftColor;
        vertices[2].tint = _rightColor;
        vertices[3].tint = _rightColor;

        var left = 0f;
        var right = rect.width;
        var top = 0f;
        var bottom = rect.height;

        vertices[0].position = new Vector3(left, bottom, Vertex.nearZ);
        vertices[1].position = new Vector3(left, top, Vertex.nearZ);
        vertices[2].position = new Vector3(right, top, Vertex.nearZ);
        vertices[3].position = new Vector3(right, bottom, Vertex.nearZ);

        MeshWriteData mwd = mgc.Allocate(vertices.Length, indices.Length);
        mwd.SetAllVertices(vertices);
        mwd.SetAllIndices(indices);
    }

}