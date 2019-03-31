using DeepEqual.Syntax;
using Models;
using Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using Xunit;

namespace Tests
{
    public class ImageTests
    {
        private readonly ImageServices _imageServices;

        public ImageTests()
        {
            _imageServices = new ImageServices();
        }

        [Fact]
        public void DrawLinesOnTheImage()
        {
            var base64ImageData = _imageServices.AddTriangleOverlay(CreateImage(60, 60), 10);
            if (base64ImageData != null)
            {
                // remove image metadata
                base64ImageData = base64ImageData.Remove(0, 23);
                Image<Rgba32> img = Image.Load<Rgba32>(Convert.FromBase64String(base64ImageData));

                Rgba32 rgba32 = new Rgba32();
                img[0, 10].ToRgba32(ref rgba32);
                Assert.True(ColourEqual(Rgba32.HotPink, rgba32,1));

                img[5, 10].ToRgba32(ref rgba32);

                Assert.True(ColourEqual(Rgba32.HotPink,rgba32,1));

                img[10, 10].ToRgba32(ref rgba32);
                Assert.True(ColourEqual(Rgba32.HotPink, rgba32,1));

                img[20, 20].ToRgba32(ref rgba32);
                Assert.True(ColourEqual(Rgba32.HotPink, rgba32,1));

                img[28, 28].ToRgba32(ref rgba32);
                Assert.True(ColourEqual(Rgba32.HotPink, rgba32,1));
            }
        }

        /// <summary>
        /// Check if RBGA colours match with ~1 error margin
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        private bool ColourEqual(Rgba32 expected, Rgba32 actual , byte tolerance)
        {
            return Math.Abs(expected.A - actual.A) <= tolerance && Math.Abs(expected.B - actual.B) <= tolerance &&
                   Math.Abs(expected.G - actual.G) <= tolerance  && Math.Abs(expected.R - actual.R) <= tolerance;
        }

        [Fact]
        public void GetTriangleCoordinatesByName()
        {
            if (_imageServices.AddTriangleOverlay(CreateImage(60, 60)) != null)
            {
                _imageServices.GetTriangleCoordinatesForCurrentImage("A1", out var a1);
                var expectedA1 = new Triangle(0, 10, 0, 0, 10, 10, "A1");
                Assert.True(a1.IsDeepEqual(expectedA1));

                _imageServices.GetTriangleCoordinatesForCurrentImage("F11", out var f11);
                var expectedF11 = new Triangle(50, 60, 50, 50, 60, 60, "F11");
                Assert.True(f11.IsDeepEqual(expectedF11));


                _imageServices.GetTriangleCoordinatesForCurrentImage("E4", out var e4);
                var expectedE4 = new Triangle(20, 40, 10, 40, 20, 50, "E4");
                Assert.True(e4.IsDeepEqual(expectedE4));
            }
        }

        [Fact]
        public void GetTriangleNameByCoordinates()
        {
            if (_imageServices.AddTriangleOverlay(CreateImage(60, 60)) != null)
            {
                var expectedA1 = new Triangle(0, 10, 0, 0, 10, 10, "");
                _imageServices.GetTriangleNameForCurrentImage(ref expectedA1);

                Assert.Equal("A1", expectedA1.Name);

                var expectedF11 = new Triangle(50, 60, 50, 50, 60, 60, "");
                _imageServices.GetTriangleNameForCurrentImage(ref expectedF11);

                Assert.Equal("F11", expectedF11.Name);

                var expectedE4 = new Triangle(20, 40, 10, 40, 20, 50, "");
                _imageServices.GetTriangleNameForCurrentImage(ref expectedE4);

                Assert.Equal("E4", expectedE4.Name);
            }
        }

        private byte[] CreateImage(int width, int height)
        {
            Image<Rgba32> img = new Image<Rgba32>(width, height);
            using (var ms = new MemoryStream())
            {
                img.Save(ms, JpegFormat.Instance);
                return ms.ToArray();
            }
        }
    }
}
