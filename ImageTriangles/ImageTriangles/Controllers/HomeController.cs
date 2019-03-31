using ImageTriangles.Models;
using ImageTriangles.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System.Diagnostics;
using System.IO;
using Constants = Enums.Constants.Constants;

namespace ImageTriangles.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImageServices _imageServices;
        public HomeController(ImageServices imageServices)
        {
            _imageServices = imageServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [RequestSizeLimit(Constants.MaxFileSize)]
        [ValidateAntiForgeryToken]
        public IActionResult UploadFile(IFormFile file)
        {
            var imageModel = new ImageViewModel();
            if (file != null && file.Length > 0)
            {
                byte[] data;
                using (var ms = new MemoryStream())
                {
                    using (var stream = file.OpenReadStream())
                    {
                        stream.CopyTo(ms);
                    }

                    data = ms.ToArray();
                }

                imageModel.ImageName = file.FileName;
                imageModel.ImageSize = file.Length;
                imageModel.ImageType = file.ContentType;
                imageModel.ImageStream = _imageServices.AddTriangleOverlay(data);

                TryValidateModel(imageModel);
            }
            else
            {
                ModelState.AddModelError("ImageName", "File not selected.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", imageModel);
            }

            return View(imageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetTriangleCoordinates(TriangleViewModel triangleViewModel)
        {
            if (_imageServices.GetTriangleCoordinatesForCurrentImage(triangleViewModel.Name, out var triangle))
            {
                triangleViewModel = new TriangleViewModel(triangle)
                {
                    TriangleFound = true
                };
            }
            else
            {
                triangleViewModel.TriangleFound = false;
            }

            return PartialView("_TriangleCoordinatesForm", triangleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetTriangleName(TriangleViewModel triangleViewModel)
        {
            ModelState.Remove("Name");
            Triangle triangle = triangleViewModel;
            if (_imageServices.GetTriangleNameForCurrentImage(ref triangle))
            {
                triangleViewModel = new TriangleViewModel(triangle)
                {
                    TriangleFound = true
                };
            }
            else
            {
                triangleViewModel.TriangleFound = false;
            }

            return PartialView("_TriangleNameForm", triangleViewModel);
        }
    }
}
