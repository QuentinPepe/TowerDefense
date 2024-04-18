using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid
{
    public class GridState : MonoBehaviour
    {
        public int Width { get; private set; } = 11;
        public int Height { get; private set; } = 11;

        [SerializeField] private int[] stateMatrix;
        [SerializeField] private GridView gridView;

        public void InitializeMatrix(int width, int height)
        {
            if (width == Width && height == Height) return;
            Width = width;
            Height = height;
            stateMatrix = new int[width * height];
            ResetMatrix();
        }

        public void ResetMatrix()
        {
            for (int i = 0; i < stateMatrix.Length; i++)
            {
                stateMatrix[i] = 0;
            }
            gridView.OnValidate();
        }

        public int GetState(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException("x and y must be within the grid boundaries.");
            return stateMatrix[y * Width + x];
        }

        public void SetState(int x, int y, int state)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;
            stateMatrix[y * Width + x] = state;
            gridView.OnValidate();
        }
    }
}