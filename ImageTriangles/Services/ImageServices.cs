using Enums;
using Enums.Constants;
using Models;
using Models.Lines;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Services
{

    public class ImageServices
    {
        private Image<Rgba32> _currentImage;
        private readonly List<Row> _currentRows;
        private readonly List<Column> _currentColumns;
        private readonly List<Triangle> _currentTriangles;
        private int _overlayThickness;
        public ImageServices()
        {
            _currentImage = null;
            _currentColumns = new List<Column>();
            _currentRows = new List<Row>();
            _currentTriangles = new List<Triangle>();
        }

        private void ClearData()
        {
            _currentImage = null;
            _currentRows.Clear();
            _currentColumns.Clear();
            _currentTriangles.Clear();
        }

        private void CalculateColumns()
        {
            var height = _currentImage.Height;
            float width = _currentImage.Width / Constants.NumberOfColumns;
            float startingWidth = 0;
            for (var i = 1; i <= Constants.NumberOfColumns; i++)
            {
                _currentColumns.Add(new Column
                {
                    ColumnOrder = i,
                    StarPoint =
                    {
                        X = startingWidth + width ,
                        Y= 0
                    },
                    EndPoint =
                    {
                        X =startingWidth + width,
                        Y=height
                    },
                    Width = width
                });
                startingWidth += width;
            }
        }

        private void CalculateRows()
        {
            var height = _currentImage.Height / Constants.NumberOfRows;
            var width = _currentImage.Width;
            float startingHeight = 0;
            for (var i = 1; i <= Constants.NumberOfRows; i++)
            {
                _currentRows.Add(new Row
                {
                    RowOrder = (RowOrder)i,
                    StarPoint =
                    {
                        X =0 ,
                        Y= height + startingHeight
                    },
                    EndPoint =
                    {
                        X =width,
                        Y=height + startingHeight
                    },
                    Height = height
                });
                startingHeight += height;
            }
        }

        private void CalculateTriangles()
        {
            foreach (var row in _currentRows)
            {
                foreach (var col in _currentColumns)
                {
                    var firstTriangle = new Triangle
                    {
                        Name = GetTriangleName(row.RowOrder, col.ColumnOrder, true),
                        Point1 =
                        {
                            X= col.StarPoint.X - col.Width,
                            Y = row.StarPoint.Y
                        },
                        Point2 =
                        {
                            X=col.StarPoint.X - col.Width,
                            Y = row.StarPoint.Y - row.Height
                        },
                        Point3 =
                        {
                            X=  col.StarPoint.X,
                            Y = row.StarPoint.Y
                        },


                    };
                    _currentTriangles.Add(firstTriangle);
                    var secondTriangle = new Triangle(new Point(col.StarPoint.X,
                                                row.StarPoint.Y - row.Height),
                                                    GetTriangleName(row.RowOrder, col.ColumnOrder, false),
                                                    firstTriangle);
                    _currentTriangles.Add(secondTriangle);

                }
            }
        }

        private string GetTriangleName(RowOrder rowOrder, int columnOrder, bool firstInColumn)
        {
            var colOrder = firstInColumn ? columnOrder * 2 - 1 : columnOrder * 2;
            return $"{rowOrder}{colOrder}";
        }

        private void DrawColumnsOnCurrentImage()
        {
            for (var i = 0; i < _currentColumns.Count - 1; i++)
            {
                var i1 = i;
                _currentImage.Mutate(
                    x => x.DrawLines(
                        Rgba32.HotPink,
                        _overlayThickness,
                        new Vector2(_currentColumns[i1].StarPoint.X, _currentColumns[i1].StarPoint.Y),
                        new Vector2(_currentColumns[i1].EndPoint.X, _currentColumns[i1].EndPoint.Y)));
            }
        }

        private void DrawRowsOnCurrentImage()
        {
            for (var i = 0; i < _currentRows.Count - 1; i++)
            {
                var i1 = i;
                _currentImage.Mutate(
                    x => x.DrawLines(
                        Rgba32.HotPink,
                        _overlayThickness,
                        new Vector2(_currentRows[i1].StarPoint.X, _currentRows[i1].StarPoint.Y),
                        new Vector2(_currentRows[i1].EndPoint.X, _currentRows[i1].EndPoint.Y)));
            }
        }

        private void DrawTriangleLinesOnCurrentImage()
        {
            for (var i = 0; i < _currentTriangles.Count; i++)
            {
                var i1 = i;
                _currentImage.Mutate(
                    x => x.DrawLines(
                        Rgba32.HotPink,
                        _overlayThickness,
                        new Vector2(_currentTriangles[i1].Point2.X, _currentTriangles[i1].Point2.Y),
                        new Vector2(_currentTriangles[i1].Point3.X, _currentTriangles[i1].Point3.Y)));
            }
        }

        public string AddTriangleOverlay(byte[] imageData, int overlayThickness = 5)
        {
            ClearData();
            _overlayThickness = overlayThickness;
            string processedImage;
            try
            {
                _currentImage = Image.Load(imageData);
                CalculateColumns();
                CalculateRows();
                CalculateTriangles();

                DrawColumnsOnCurrentImage();
                DrawRowsOnCurrentImage();
                DrawTriangleLinesOnCurrentImage();
                var format = Image.DetectFormat(imageData);
                processedImage = _currentImage.ToBase64String(format);
            }
            catch (Exception e)
            {
                processedImage = null;
            }

            return processedImage;
        }

        public bool GetTriangleCoordinatesForCurrentImage(string triangleName, out Triangle outTriangle)
        {
            outTriangle = _currentTriangles.FirstOrDefault(triangle => triangle.Name.Equals(triangleName, StringComparison.InvariantCultureIgnoreCase));
            return outTriangle != null;
        }

        public bool GetTriangleNameForCurrentImage(ref Triangle triangle)
        {
            bool found = false;
            var triangle1 = triangle;
            var foundTriangle = _currentTriangles.FirstOrDefault(t =>
                t.Point1.Equals(triangle1.Point1) && t.Point2.Equals(triangle1.Point2) &&
                t.Point3.Equals(triangle1.Point3));

            if (foundTriangle != null)
            {
                triangle = foundTriangle;
                found = true;
            }

            return found;
        }
    }
}
