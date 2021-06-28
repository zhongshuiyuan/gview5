﻿using gView.GraphicsEngine.Abstraction;
using gView.GraphicsEngine.GdiPlus.Extensions;
using System;
using System.Drawing;

namespace gView.GraphicsEngine.GdiPlus
{
    internal class Canvas : ICanvas
    {
        private Graphics _graphics;

        public Canvas(Bitmap bitmap)
        {
            _graphics = Graphics.FromImage(bitmap);
        }

        #region ICanvas

        public float DpiX => _graphics != null ? _graphics.DpiX : 0;

        public float DpiY => _graphics != null ? _graphics.DpiY : 0;

        public CompositingMode CompositingMode
        {
            set
            {
                CheckUsability();

                _graphics.CompositingMode = value.ToGdiCompositionMode();
            }
        }

        public InterpolationMode InterpolationMode
        {
            get
            {
                CheckUsability();

                return _graphics.InterpolationMode.ToInterpolationMode();
            }
            set
            {
                CheckUsability();

                _graphics.InterpolationMode = value.ToGdiInterpolationMode();
            }
        }

        public void FillRectangle(IBrush brush, int left, int right, int width, int height)
        {
            CheckUsability();

            _graphics.FillRectangle((Brush)brush.EngineElement, left, right, width, height);
        }

        public void DrawRectangle(IPen pen, CanvasRectangle rectangle)
        {
            CheckUsability();

            _graphics.DrawRectangle((Pen)pen.EngineElement, rectangle.ToGdiRectangle());
        }

        public void DrawRectangle(IPen pen, CanvasRectangleF rectangleF)
        {
            CheckUsability();

            _graphics.DrawRectangle((Pen)pen.EngineElement,
                rectangleF.Left,
                rectangleF.Top,
                rectangleF.Width,
                rectangleF.Height);
        }

        public void FillRectangle(IBrush brush, CanvasRectangle rectangle)
        {
            CheckUsability();

            _graphics.FillRectangle((Brush)brush.EngineElement, rectangle.ToGdiRectangle());
        }

        public void FillRectangle(IBrush brush, CanvasRectangleF rectangleF)
        {
            CheckUsability();

            _graphics.FillRectangle((Brush)brush.EngineElement, rectangleF.ToGdiRectangleF());
        }

        public void DrawBitmap(IBitmap bitmap, CanvasPoint point)
        {
            CheckUsability();

            _graphics.DrawImage((Bitmap)bitmap.EngineElement, point.ToGdiPoint());
        }

        public void DrawBitmap(IBitmap bitmap, CanvasPointF pointF)
        {
            CheckUsability();

            _graphics.DrawImage((Bitmap)bitmap.EngineElement, pointF.ToGdiPointF());
        }

        public void DrawBitmap(IBitmap bitmap, CanvasRectangle dest, CanvasRectangle source, float opacity = 1.0f)
        {
            CheckUsability();

            var imageAttributes = CreateImageAttributes(opacity);

            if (imageAttributes != null)
            {
                _graphics.DrawImage((Bitmap)bitmap.EngineElement,
                    dest.ToGdiRectangle(),
                    source.Left, source.Top, source.Width, source.Height,
                    GraphicsUnit.Pixel,
                    imageAttrs: imageAttributes);
            } else
            {
                _graphics.DrawImage((Bitmap)bitmap.EngineElement,
                    dest.ToGdiRectangle(),
                    source.ToGdiRectangle(),
                    GraphicsUnit.Pixel);
            }
        }

        public void DrawBitmap(IBitmap bitmap, CanvasRectangleF dest, CanvasRectangleF source)
        {
            CheckUsability();

            _graphics.DrawImage((Bitmap)bitmap.EngineElement,
                dest.ToGdiRectangleF(),
                source.ToGdiRectangleF(),
                GraphicsUnit.Pixel);
        }

        public void DrawBitmap(IBitmap bitmap, CanvasPointF[] points, CanvasRectangleF source, float opacity = 1)
        {
            CheckUsability();

            var imageAttributes = CreateImageAttributes(opacity);

            if (imageAttributes != null)
            {
                _graphics.DrawImage((Bitmap)bitmap.EngineElement,
                    points.ToGdiPointFArray(),
                    source.ToGdiRectangleF(),
                    GraphicsUnit.Pixel,
                    imageAttr: imageAttributes);
            }
            else
            {
                _graphics.DrawImage((Bitmap)bitmap.EngineElement,
                    points.ToGdiPointFArray(),
                    source.ToGdiRectangleF(),
                    GraphicsUnit.Pixel);
            }
        }

        public void DrawText(string text, IFont font, IBrush brush, CanvasPoint point)
        {
            CheckUsability();

            _graphics.DrawString(text, (Font)font.EngineElement, (Brush)brush.EngineElement, point.ToGdiPoint());
        }

        public void DrawText(string text, IFont font, IBrush brush, CanvasPointF pointF)
        {
            CheckUsability();

            _graphics.DrawString(text, (Font)font.EngineElement, (Brush)brush.EngineElement, pointF.ToGdiPointF());
        }

        public void DrawText(string text, IFont font, IBrush brush, int x, int y)
        {
            CheckUsability();

            _graphics.DrawString(text, (Font)font.EngineElement, (Brush)brush.EngineElement, x, y);
        }

        public void DrawText(string text, IFont font, IBrush brush, float x, float y)
        {
            CheckUsability();

            _graphics.DrawString(text, (Font)font.EngineElement, (Brush)brush.EngineElement, x, y);
        }

        public CanvasSizeF MeasureText(string text, IFont font)
        {
            CheckUsability();

            var sizeF = _graphics.MeasureString(text, (Font)font.EngineElement);
            return new CanvasSizeF(sizeF.Width, sizeF.Height);
        }

        public void DrawLine(IPen pen, CanvasPoint p1, CanvasPoint p2)
        {
            CheckUsability();

            _graphics.DrawLine((Pen)pen.EngineElement, p1.ToGdiPoint(), p2.ToGdiPoint());
        }

        public void DrawLine(IPen pen, CanvasPointF p1, CanvasPointF p2)
        {
            CheckUsability();

            _graphics.DrawLine((Pen)pen.EngineElement, p1.ToGdiPointF(), p2.ToGdiPointF());
        }

        public void DrawLine(IPen pen, int x1, int y1, int x2, int y2)
        {
            CheckUsability();

            _graphics.DrawLine((Pen)pen.EngineElement, x1, y1, x2, y2);
        }

        public void DrawLine(IPen pen, float x1, float y1, float x2, float y2)
        {
            CheckUsability();

            _graphics.DrawLine((Pen)pen.EngineElement, x1, y1, x2, y2);
        }

        #endregion ICanvas

        #region IDisposable

        public void Dispose()
        {
            if (_graphics != null)
            {
                _graphics.Dispose();
                _graphics = null;
            }
        }

        #endregion IDisposable

        #region Helper

        private void CheckUsability()
        {
            if (_graphics == null)
            {
                throw new Exception("Canvas already disposed...");
            }
        }

        private System.Drawing.Imaging.ImageAttributes CreateImageAttributes(float opacity)
        {
            if (opacity >= 0 && opacity < 1f)
            {
                float[][] ptsArray ={
                                        new float[] {1, 0, 0, 0, 0},
                                        new float[] {0, 1, 0, 0, 0},
                                        new float[] {0, 0, 1, 0, 0},
                                        new float[] {0, 0, 0, opacity, 0},
                                        new float[] {0, 0, 0, 0, 1}};

                System.Drawing.Imaging.ColorMatrix clrMatrix = new System.Drawing.Imaging.ColorMatrix(ptsArray);
                System.Drawing.Imaging.ImageAttributes imgAttributes = new System.Drawing.Imaging.ImageAttributes();
                imgAttributes.SetColorMatrix(clrMatrix,
                                             System.Drawing.Imaging.ColorMatrixFlag.Default,
                                             System.Drawing.Imaging.ColorAdjustType.Bitmap);

                return imgAttributes;
            }

            return null;
        }


        #endregion Helper
    }
}