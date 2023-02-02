using Bakers.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Bakers.Models
{
    public class Language
    {
        public static List<Language> Languages { get; set; }
        public static Dictionary<string, Language> LanguagesDictionary { get; set; }


        public static string[] SupportedCultures { get; set; }


        [Display(Name = "Code")]
        public string Id { get; set; }

        [Display(Name = "Language")]
        public string Name { get; set; }

        [Display(Name = "Cultures List")]
        public string? Cultures { get; set; } = "";

        [Display(Name = "Show?")]
        public bool IsShown { get; set; } = true;

        public static void Initialize(BakersDbContext context)
        {
            Language.Languages = context.Language.OrderBy(e => e.Name).ToList();
            Language.LanguagesDictionary = new Dictionary<string, Language>();
            List<string> supportedLanguages = new List<string>();

            foreach (Language l in Language.Languages)
            {
                Language.LanguagesDictionary[l.Id] = l;
                if (l.Id != "-")
                {
                    supportedLanguages.Add(l.Id);
                    string[] cultures = l.Cultures.Split(";");
                    foreach (string culture in cultures)
                    {
                        supportedLanguages.Add(l.Id + "-" + culture);
                    }
                }
            }
            Language.SupportedCultures = supportedLanguages.ToArray();

        }
    
    }
}
