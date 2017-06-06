
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TermsAndDefinitions.WebUI.ViewModels
{
    public class DefinitionViewModel
    {
        public string Description { get; set; }
        public string URL { get; set; }
        public string Name
        {
            get
            {
                if (URL.Contains(".pdf") || URL.Contains(".doc") || URL.Contains(".docx"))
                    return HttpUtility.UrlDecode(Path.GetFileName(new Uri(URL).AbsolutePath));
                else
                {
                    string result = URL;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest.Create(URL) as HttpWebRequest);
                        HttpWebResponse response = (request.GetResponse() as HttpWebResponse);

                        using (Stream stream = response.GetResponseStream())
                        {
                            Regex titleCheck = new Regex(@"<title>\s*(.+?)\s*</title>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            int bytesToRead = 8092;
                            byte[] buffer = new byte[bytesToRead];
                            string contents = "";
                            int length = 0;
                            while ((length = stream.Read(buffer, 0, bytesToRead)) > 0)
                            {
                                contents += Encoding.UTF8.GetString(buffer, 0, length);

                                Match m = titleCheck.Match(contents);
                                if (m.Success)
                                {
                                    result = m.Groups[1].Value.ToString();
                                    break;
                                }
                                else if (contents.Contains("</head>"))
                                {
                                    break;
                                }
                            }
                        }
                        return result;
                    }
                    catch
                    {
                        return URL;
                    }
                }
            }
        }
    }

    public class PreviewTermViewModel
    {
        public string TermName { get; set; }
        public DefinitionViewModel Definition { get; set; }
    }

    public class TermViewModel
    {
        public string TermName { get; set; }
        public IEnumerable<DefinitionViewModel> Definitions { get; set; }
        public IEnumerable<PreviewProjectViewModel> Projects { get; set; }
    }

    public class PreviewProjectViewModel
    {
        public string ProjectName { get; set; }
        public PreviewInfSysViewModel InformationSystem { get; set; }
    }

    public class ProjectViewModel
    {
        [Display(Name = "Название проекта:")]
        public string ProjectName { get; set; }
        [Display(Name = "Информационая система:")]
        public PreviewInfSysViewModel InformationSystem { get; set; }
        [Display(Name = "Жизненый цикл:")]
        public string LifeCycle { get; set; }
        public IEnumerable<PreviewTermViewModel> Glossary { get; set; }
        public IEnumerable <PreviewInfSysViewModel> InfSysList {get; set;}
        [Display(Name = "Файл технической документации:")]
        public HttpPostedFileBase File { get; set; }
    }

    public class PreviewInfSysViewModel
    {
        public string Name{ get; set; }
        public int Id { get; set; }
    }

    public class InfSysViewModel
    {
        string Name { get; set; }
        string Descripton { get; set; }
        int Id { get; set; }
        IEnumerable<PreviewProjectViewModel> Projects { get; set; }
    }
}