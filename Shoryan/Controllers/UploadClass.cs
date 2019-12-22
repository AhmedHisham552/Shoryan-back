
using DBapplication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
namespace Shoryan.Controllers
{
    [Produces("application/json")]

    public class UploadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        DBManager dbMan;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            dbMan = new DBManager();
        }

        [HttpPost("api/upload"), DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = Path.Combine("images", "profilePics");
                //string webRootPath = _hostingEnvironment.WebRootPath;
                //string newPath = Path.Combine(webRootPath, folderName);

                string webRootPath = _hostingEnvironment.WebRootPath;

                var newPath = Path.Combine(webRootPath, folderName);
                string relativePath = "";

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    relativePath = Path.Combine(folderName, fileName);
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return StatusCode(200, relativePath);
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }

        [HttpPost("api/uploadListings/{listingId}"), DisableRequestSizeLimit]
        public ActionResult UploadListings(int listingId)
        {
            try
            {
                string folderName = Path.Combine("images","listings", Convert.ToString(listingId));

                string webRootPath = _hostingEnvironment.WebRootPath;
                var newPath = Path.Combine(webRootPath, folderName);

                List<string> listingsImgsUrls = new List<string>();

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                foreach (var file in Request.Form.Files)
                {
                    if (file.Length > 0)
                    {
                        string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        listingsImgsUrls.Add(Path.Combine(folderName, fileName));
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }

                }

                string StoredProcedureName = ListingsProcedures.addListingImgUrl;


                int i = 0;
                foreach (var listingImgUrl in listingsImgsUrls)
                {
                    Dictionary<string, object> Parameters = new Dictionary<string, object>();
                    Parameters.Add("@listing_id", listingId);
                    Parameters.Add("@img_no",i);
                    Parameters.Add("@url", listingImgUrl);

                    dbMan.ExecuteReader(StoredProcedureName, Parameters);
                    i++;
                }



                return StatusCode(200, listingsImgsUrls);
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }
    }
}
