using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    class DbInitializer : DropCreateDatabaseIfModelChanges<CandidateContext>
    {
        protected override void Seed(CandidateContext context)
        {
            Language lng1 = new Language { Name = "English" };
            Language lng2 = new Language { Name = "Chinese" };
            Language lng3 = new Language { Name = "Germany" };
            Language lng4 = new Language { Name = "Spanish" };
            Language lng5 = new Language { Name = "French" };
            Language lng6 = new Language { Name = "Ukrainian" };

            ProgrammingLanguage plng1 = new ProgrammingLanguage { Name = "C++" };
            ProgrammingLanguage plng2 = new ProgrammingLanguage { Name = "C#" };
            ProgrammingLanguage plng3 = new ProgrammingLanguage { Name = "Java" };
            ProgrammingLanguage plng4 = new ProgrammingLanguage { Name = "Javascript" };
            ProgrammingLanguage plng5 = new ProgrammingLanguage { Name = "PHP" };
            ProgrammingLanguage plng6 = new ProgrammingLanguage { Name = "Phyton" };

            Framework framework1 = new Framework { Name = "STL" };
            Framework framework2 = new Framework { Name = "WinAPI" };
            Framework framework3 = new Framework { Name = "WinForms" };
            Framework framework4 = new Framework { Name = "WPF" };
            Framework framework5 = new Framework { Name = "Angular" };
            Framework framework6 = new Framework { Name = "React" };

            context.Languages.AddRange(new List<Language> { lng1, lng2, lng3, lng4, lng5, lng6 });
            context.ProgrammingLanguages.AddRange(new List<ProgrammingLanguage> { plng1, plng2, plng3, plng4, plng5, plng6 });
            context.Frameworks.AddRange(new List<Framework> { framework1, framework2, framework3, framework4, framework5, framework6 });
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
