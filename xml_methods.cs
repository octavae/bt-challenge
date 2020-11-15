using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Globalization;
using System.Net;

namespace Tavi
{
    public class CGWebClient : WebClient
    {
        private System.Net.CookieContainer cookieContainer;
        private string userAgent;
        private int timeout;

        public System.Net.CookieContainer CookieContainer
        {
            get { return cookieContainer; }
            set { cookieContainer = value; }
        }

        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public CGWebClient()
        {
            timeout = -1;
            userAgent = @"CodeGator Crawler v1.0";
            cookieContainer = new CookieContainer();
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (request.GetType() == typeof(HttpWebRequest))
            {
                ((HttpWebRequest)request).CookieContainer = cookieContainer;
                ((HttpWebRequest)request).UserAgent = userAgent;
                ((HttpWebRequest)request).Timeout = timeout;
            }

            return request;
        }
    }
  
    public static class LoadXmlMethods
    {

        #region Public Methods

        public static void DownLoadXml(string filePath)
        {
            using (CGWebClient wc = new CGWebClient())
            {
                wc.DownloadFile("url", filePath + "vente_new.xml");
                if (System.IO.File.Exists(filePath + "vente_new.xml"))
                {
                    FileInfo fi = new FileInfo(filePath + "vente_new.xml");
                    if (fi.Length > 1000000)
                    {
                        if (System.IO.File.Exists(filePath + "vente.xml"))
                        {
                            System.IO.File.Delete(filePath + "vente.xml");
                        }
                        System.IO.File.Copy(filePath + "vente_new.xml", filePath + "vente.xml", true);
                        System.IO.File.Delete(filePath + "vente_new.xml");
                    }
                }
            }
        }

        public static void ImportXml(string filePath)
        {

            DateTimeFormatInfo dfi = new CultureInfo("fr-FR", true).DateTimeFormat;

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(filePath);



                    if (doc.DocumentElement.HasChildNodes)
                    {
                        System.Xml.XmlNodeList nodeList = null;
                        nodeList = doc.SelectNodes("//bien");
                        if (nodeList != null)
                        {

                            foreach (System.Xml.XmlNode curNode in nodeList)
                            {
                                // populate object
                                objVente.ModuleId = mid;
                                objVente.ItemId = -1;
                                objVente.VNoDossier = GetNodeValue(curNode, "no_dossier");
                                objVente.VRefAgence = GetNodeValue(curNode, "ref_agence");
                                objVente.VNom = GetNodeValue(curNode, "nom/designation[@lang='fr']");
                                objVente.VType = GetNodeValue(curNode, "type/code");
                                objVente.VEtat = GetNodeValue(curNode, "etat/code");
                                objVente.VDateCreation = DateTime.Parse(GetNodeValue(curNode, "date_creation"));
                                objVente.VNbNiveaux = GetNodeIntValue(curNode, "nb_niveaux");

                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.LogException(exc);
            }
        }



        #endregion

        
        #region Helper Methods

        private static string GetNodeValue(System.Xml.XmlNode doc, string nodeName)
        {
            System.Xml.XmlNode node = null;
            node = doc.SelectSingleNode(nodeName);
            if (node != null) return node.InnerText;
            return String.Empty;
        }

        private static int GetNodeIntValue(System.Xml.XmlNode doc, string nodeName)
        {
            int localValue = 0;
            System.Xml.XmlNode node = null;
            node = doc.SelectSingleNode(nodeName);
            if (node != null)
            {
                Int32.TryParse(node.InnerText, out localValue);
            };
            return localValue;
        }

        private static float GetNodeFloatValue(System.Xml.XmlNode doc, string nodeName)
        {
            float localValue = 0;
            System.Xml.XmlNode node = null;
            node = doc.SelectSingleNode(nodeName);
            if (node != null)
            {
                float.TryParse(node.InnerText, out localValue);
            };
            return localValue;
        }


        private static void SaveImageToDisk(string imageName, string imageUrl, string imageFormat, string resizeImage, string objectID)
        {
            string fileOgResPath = System.Web.HttpRuntime.AppDomainAppPath + @"Portals\0\Vente\images\photos\ogres\" + imageName;
            string fileHiResPath = System.Web.HttpRuntime.AppDomainAppPath + @"Portals\0\Vente\images\photos\hires\" + imageName;
            string fileLoResPath = System.Web.HttpRuntime.AppDomainAppPath + @"Portals\0\Vente\images\photos\lores\" + imageName;


            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            System.Drawing.Image image = null;


            try
            {
                // originals
                if (!System.IO.File.Exists(fileOgResPath))
                {
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
                    if (httpWebRequest != null)
                    {
                        httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        if (httpWebResponse != null)
                        {
                            image = System.Drawing.Image.FromStream(httpWebResponse.GetResponseStream());
                            if (image != null)
                            {
                                switch (imageFormat)
                                {
                                    case "png":
                                        image.Save(fileOgResPath, System.Drawing.Imaging.ImageFormat.Png);
                                        break;
                                    case "gif":
                                        image.Save(fileOgResPath, System.Drawing.Imaging.ImageFormat.Gif);
                                        break;
                                    default:
                                        image.Save(fileOgResPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        break;
                                }
                            }
                        }
                    }
                }
                
                // hires image
                if (!System.IO.File.Exists(fileHiResPath) && System.IO.File.Exists(fileOgResPath))
                {
                    ImageResizer.ImageBuilder.Current.Build(fileOgResPath, fileHiResPath, new ImageResizer.ResizeSettings("width=344&height=239&crop=auto"));
                }

                // lores image
                if (resizeImage == "y")
                {
                    // delete the old thumbnail
                    ArrayList currentThumbs = new ArrayList();
                    currentThumbs.Add(imageName);
                    DeleteOldImagesFromDisk(objectID, currentThumbs, "thumb");
                    // saves the new thumbnail
                    if (!System.IO.File.Exists(fileLoResPath) && System.IO.File.Exists(fileOgResPath))
                    {
                        ImageResizer.ImageBuilder.Current.Build(fileOgResPath, fileLoResPath, new ImageResizer.ResizeSettings("width=137&height=108&crop=auto"));
                    }
                }

            }
            catch (Exception exc)
            {
                Exceptions.LogException(exc);
            }
            finally
            {
                if (httpWebResponse != null)
                { httpWebResponse.Close(); }
                if (image != null)
                { image.Dispose(); }
            }

        }

        private static void DeleteOldImagesFromDisk(string objectID, ArrayList currentImages, string imageType)
        {

            string photosPattern = objectID + "_*.*";

            string fileOgResPath = System.Web.HttpRuntime.AppDomainAppPath + @"Portals\0\Vente\images\photos\ogres";
            string fileHiResPath = System.Web.HttpRuntime.AppDomainAppPath + @"Portals\0\Vente\images\photos\hires";
            string fileLoResPath = System.Web.HttpRuntime.AppDomainAppPath + @"Portals\0\Vente\images\photos\lores";


            try
            {
                System.IO.DirectoryInfo dinfo;

                // original images
                if (imageType == "all")
                {
                    dinfo = new System.IO.DirectoryInfo(fileOgResPath);
                    if (dinfo.Exists)
                    {
                        System.IO.FileInfo[] photos;
                        if (objectID != String.Empty)
                        {
                            // get all photos named id+.+extension
                            photos = dinfo.GetFiles(photosPattern);

                            if (photos.Length > 0)
                            {
                                for (int c = 0; c < photos.Length; c++)
                                {
                                    if (!currentImages.Contains(photos[c].Name))
                                    {
                                        System.IO.File.Delete(fileOgResPath + "\\" + photos[c].Name);
                                    }
                                }
                            }
                        }
                    }
                }
                
                // hires images
                if (imageType == "all")
                {
                    dinfo = new System.IO.DirectoryInfo(fileHiResPath);
                    if (dinfo.Exists)
                    {
                        System.IO.FileInfo[] photos;
                        if (objectID != String.Empty)
                        {
                            // get all photos named id+.+extension
                            photos = dinfo.GetFiles(photosPattern);

                            if (photos.Length > 0)
                            {
                                for (int c = 0; c < photos.Length; c++)
                                {
                                    if (!currentImages.Contains(photos[c].Name))
                                    {
                                        System.IO.File.Delete(fileHiResPath + "\\" + photos[c].Name);
                                    }
                                }
                            }
                        }
                    }
                }

                // lores images
                dinfo = new System.IO.DirectoryInfo(fileLoResPath);
                if (dinfo.Exists)
                {
                    System.IO.FileInfo[] photos;
                    if (objectID != String.Empty)
                    {
                        // get all photos named id+.+extension
                        photos = dinfo.GetFiles(photosPattern);

                        if (photos.Length > 0)
                        {
                            for (int c = 0; c < photos.Length; c++)
                            {
                                if (!currentImages.Contains(photos[c].Name))
                                {
                                    System.IO.File.Delete(fileLoResPath + "\\" + photos[c].Name);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                Exceptions.LogException(exc);
            }
            finally
            {

            }

        }

        #endregion

    }


}
