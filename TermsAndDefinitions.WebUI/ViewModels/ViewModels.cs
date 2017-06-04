using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TermsAndDefinitions.WebUI.ViewModels
{
    public class TermViewModel
    {
        public string TermName { get; set; }
        public IEnumerable<string> Definitions { get; set; }
        public IEnumerable<string> URLs { get; set; }
        public string GetLinkNameByURL(string url)
        {
            if(url.Contains(".pdf") || url.Contains(".doc") || url.Contains(".docx"))
                return HttpUtility.UrlDecode(Path.GetFileName(new Uri(url).AbsolutePath));
            else
            {
                string result = url;
                try
                {
                    HttpWebRequest request = (HttpWebRequest.Create(url) as HttpWebRequest);
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
                catch (Exception e)
                {
                    return url;
                }
            }
        }
    }

    public class PreviewTermViewModel
    {
        public string TermName { get; set; }
        public string Definition { get; set; }
        public string URL { get; set; }
    }

    //public class VDefinition
    //{
    //    public VDefinition() { }
    //    public VDefinition(string description, string url)
    //    {
    //        Description = description;
    //        URL = url;
    //    }

    //    public VDefinition( Definition  definition)
    //    {
    //        Description = definition.Description;
    //        URL = definition.URL;
    //        Term = definition.Term.TermName;
    //    }
    //    public string Term { get; set; }
    //   public string Description { get; set; }
    //   public string URL { get; set; }
    //}

    //public class VProject
    //{
    //    public VProject() { }

    //    public VProject(Project project)
    //    {
    //        Glossary = new List<VTerm>();
    //        ProjectName = project.ProjectName;
    //        InformationSystems = project.InformationSystem?.NameInformationSystem;
    //        LifeCycle = project.LifeСycle?.NameLifeСycle;
    //        DocumetationURLs = project.References.Select(x => x.URLToFile).ToList();
    //        foreach (var term in project.Terms)
    //            Glossary.Add(new VTerm(term));
    //    }        

        //public string Annotation {get; set;}

        //public string ProjectName { get; set; }

        //public string InformationSystems { get; set; }

        //public string LifeCycle { get; set; }

        //public List<string> DocumetationURLs = new List<string>();

        //public List<VTerm> Glossary {get; set;}
    //}
    
}