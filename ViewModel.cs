using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Resume
{
    class ViewModel : DependencyObject, IDisposable
    {

        #region Properties
        public Candidate currentCandidate { get; set; }

        CandidateContext context;

        public Candidate selectedCandidate
        {
            get { return (Candidate)GetValue(selectedCandidateProperty); }
            set { SetValue(selectedCandidateProperty, value); }
        }

        public static readonly DependencyProperty selectedCandidateProperty =
            DependencyProperty.Register("selectedCandidate", typeof(Candidate), typeof(ViewModel), new PropertyMetadata(null));

        public ObservableCollection<Candidate> Candidates { get; set; }
        public ObservableCollection<ProgrammingLanguage> ProgrammingLanguagesList { get; set; }
        public ObservableCollection<Language> LanguagesList { get; set; }
        public ObservableCollection<Framework> FrameworksList { get; set; }
        #endregion 
        public ViewModel()
        {
            currentCandidate = new Candidate();
            selectedCandidate = null;
            context = new CandidateContext();
            ProgrammingLanguagesList = new ObservableCollection<ProgrammingLanguage>(context.ProgrammingLanguages
                .Include(x => x.Candidates).ToList<ProgrammingLanguage>());
            LanguagesList = new ObservableCollection<Language>(context.Languages
                .Include(x => x.Candidates).ToList<Language>());
            FrameworksList = new ObservableCollection<Framework>(context.Frameworks
                .Include(x => x.Candidates).ToList<Framework>());
            Candidates = new ObservableCollection<Candidate>(context.Candidates.Include(x => x.Frameworks)
                .Include(x => x.ProgrammingLanguages).Include(x => x.Languages).ToList<Candidate>());

        }
        #region Commands
        private ResumeCommand addCommand;

        public ResumeCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new ResumeCommand(Add, CanAdd));
            }
        }

        private void Add(object obj)
        {
            Candidate candidate = new Candidate();
            SetCandidateChange(candidate, currentCandidate);
            Candidates.Add(candidate);
            FillCandidateSkils(candidate);
            context.SaveChanges();
            ClearForm();
        }

        private bool CanAdd(object obj)
        {
            if (string.IsNullOrEmpty(currentCandidate.Name) || string.IsNullOrEmpty(currentCandidate.Surname) ||
                string.IsNullOrEmpty(currentCandidate.MidleName) || string.IsNullOrEmpty(currentCandidate.Phone) ||
                string.IsNullOrEmpty(currentCandidate.Address) || string.IsNullOrEmpty(currentCandidate.MaritalStatus) ||
                string.IsNullOrEmpty(currentCandidate.Education) || currentCandidate.Age < 18 || currentCandidate.Age > 65 ||
                !currentCandidate.IsValidEmail || !ProgrammingLanguagesList.Where(x => x.IsChecked == true).Any() ||
                Candidates.Where(x => x.Id == currentCandidate.Id).Any() || !FrameworksList.Where(x => x.IsChecked == true).Any() ||
                !LanguagesList.Where(x => x.IsChecked == true).Any())
                return false;
            else
                return true;
        }

        private ResumeCommand showCommand;

        public ResumeCommand ShowCommand
        {
            get
            {
                return showCommand ?? (showCommand = new ResumeCommand(Show, CanShow));
            }
        }

        private void Show(object obj)
        {
            ClearForm();
            SetCandidateChange(currentCandidate, selectedCandidate);
            FillCheckboxes(currentCandidate);
        }

        private bool CanShow(object obj)
        {
            if (selectedCandidate != null)
                return true;
            else
                return false;
        }

        private ResumeCommand editCommand;

        public ResumeCommand EditCommand
        {
            get => editCommand ?? (editCommand = new ResumeCommand(Edit, CanEdit));
        }

        private void Edit(object obj)
        {
            SetCandidateChange(selectedCandidate, currentCandidate);
            FillCandidateSkils(selectedCandidate);
            context.SaveChanges();
            ClearForm();
        }

        private bool CanEdit(object obj)
        {
            if (selectedCandidate != null && currentCandidate.Id == selectedCandidate.Id && (!CheckSkilsState(selectedCandidate) ||
                currentCandidate.Name != selectedCandidate.Name || currentCandidate.MidleName != selectedCandidate.MidleName ||
                currentCandidate.Surname != selectedCandidate.Surname || currentCandidate.Age != selectedCandidate.Age ||
                currentCandidate.Address != selectedCandidate.Address || currentCandidate.Phone != selectedCandidate.Phone ||
                currentCandidate.Email != selectedCandidate.Email || currentCandidate.MaritalStatus != selectedCandidate.MaritalStatus ||
                currentCandidate.Education != selectedCandidate.Education))
                return true;
            else
                return false;
        }

        private ResumeCommand cancelCommand;

        public ResumeCommand CancelCommand
        {
            get => cancelCommand ?? (cancelCommand = new ResumeCommand(Cancel, CanCancel));
        }

        private void Cancel(object obj)
        {
            ClearForm();
        }

        private bool CanCancel(object obj)
        {
            if (string.IsNullOrEmpty(currentCandidate.Name) && string.IsNullOrEmpty(currentCandidate.Surname) &&
                string.IsNullOrEmpty(currentCandidate.MidleName) && string.IsNullOrEmpty(currentCandidate.Phone) &&
                string.IsNullOrEmpty(currentCandidate.Address) && string.IsNullOrEmpty(currentCandidate.MaritalStatus) &&
                string.IsNullOrEmpty(currentCandidate.Education) && currentCandidate.Age == 0 &&
                !currentCandidate.IsValidEmail && !ProgrammingLanguagesList.Where(x => x.IsChecked == true).Any() &&
                !FrameworksList.Where(x => x.IsChecked == true).Any() && !LanguagesList.Where(x => x.IsChecked == true).Any())
                return false;
            else
                return true;
        }

        private ResumeCommand removeCommand;

        public ResumeCommand RemoveCommand
        {
            get => removeCommand ?? (removeCommand = new ResumeCommand(Remove, CanRemove));
        }

        private void Remove(object obj)
        {
            context.Candidates.Remove(selectedCandidate);
            Candidates.Remove(selectedCandidate);
            context.SaveChanges();
            ClearForm();
        }

        private bool CanRemove(object obj)
        {
            if (selectedCandidate == null)
                return false;
            else
                return true;
        }
        #endregion
        #region Service methods
        private void ClearForm()
        {
            currentCandidate.Id = 0;
            currentCandidate.Name = "";
            currentCandidate.MidleName = "";
            currentCandidate.Surname = "";
            currentCandidate.Age = 0;
            currentCandidate.Phone = "";
            currentCandidate.Address = "";
            currentCandidate.Email = "";
            currentCandidate.Education = "";
            currentCandidate.MaritalStatus = "";
            currentCandidate.ProgrammingLanguages = new ObservableCollection<ProgrammingLanguage>();
            currentCandidate.Frameworks = new ObservableCollection<Framework>();
            currentCandidate.Languages = new ObservableCollection<Language>();
            foreach (var item in ProgrammingLanguagesList)
            {
                item.IsChecked = false;
            }
            foreach (var item in FrameworksList)
            {
                item.IsChecked = false;
            }
            foreach (var item in LanguagesList)
            {
                item.IsChecked = false;
            }
        }
        private void FillCheckboxes(Candidate candidate)
        {
            foreach (var item in ProgrammingLanguagesList)
            {
                if (candidate.ProgrammingLanguages.Where(x => x.Name == item.Name).Any())
                    item.IsChecked = true;
            }
            foreach (var item in FrameworksList)
            {
                if (candidate.Frameworks.Where(x => x.Name == item.Name).Any())
                    item.IsChecked = true;
            }
            foreach (var item in LanguagesList)
            {
                if (candidate.Languages.Where(x => x.Name == item.Name).Any())
                    item.IsChecked = true;
            }
        }
        private void SetCandidateChange(Candidate candidate1, Candidate candidate2)
        {
            candidate1.Id = candidate2.Id;
            candidate1.Name = candidate2.Name;
            candidate1.MidleName = candidate2.MidleName;
            candidate1.Surname = candidate2.Surname;
            candidate1.Age = candidate2.Age;
            candidate1.Address = candidate2.Address;
            candidate1.Phone = candidate2.Phone;
            candidate1.Email = candidate2.Email;
            candidate1.Education = candidate2.Education;
            candidate1.MaritalStatus = candidate2.MaritalStatus;
            candidate1.ProgrammingLanguages = candidate2.ProgrammingLanguages;
            candidate1.Frameworks = candidate2.Frameworks;
            candidate1.Languages = candidate2.Languages;
        }

        private void FillCandidateSkils(Candidate candidate)
        {
            foreach (var el in ProgrammingLanguagesList)
            {
                if (el.IsChecked)
                {
                    if (!el.Candidates.Contains(candidate))
                        el.Candidates.Add(candidate);
                    el.IsChecked = false;
                }
                else
                {
                    if (el.Candidates.Contains(candidate))
                    {
                        el.Candidates.Remove(candidate);
                    }
                }
            }

            foreach (var el in FrameworksList)
            {
                if (el.IsChecked)
                {
                    if (!el.Candidates.Contains(candidate))
                        el.Candidates.Add(candidate);
                    el.IsChecked = false;
                }
                else
                {
                    if (el.Candidates.Contains(candidate))
                    {
                        el.Candidates.Remove(candidate);
                    }
                }
            }

            foreach (var el in LanguagesList)
            {
                if (el.IsChecked)
                {
                    if (!el.Candidates.Contains(candidate))
                        el.Candidates.Add(candidate);
                    el.IsChecked = false;
                }
                else
                {
                    if (el.Candidates.Contains(candidate))
                    {
                        el.Candidates.Remove(candidate);
                    }
                }
            }
        }

        private bool CheckSkilsState(Candidate candidate)
        {

            foreach (var item in ProgrammingLanguagesList)
            {
                if (item.IsChecked)
                {
                    if (!candidate.ProgrammingLanguages.Where(x => x.Name == item.Name).Any())
                        return false;
                }
                else
                {
                    if (candidate.ProgrammingLanguages.Where(x => x.Name == item.Name).Any())
                        return false;
                }
            }
            foreach (var item in FrameworksList)
            {
                if (item.IsChecked)
                {
                    if (!candidate.Frameworks.Where(x => x.Name == item.Name).Any())
                        return false;
                }
                else
                {
                    if (candidate.Frameworks.Where(x => x.Name == item.Name).Any())
                        return false;
                }
            }
            foreach (var item in LanguagesList)
            {
                if (item.IsChecked)
                {
                    if (!candidate.Languages.Where(x => x.Name == item.Name).Any())
                        return false;
                }
                else
                {
                    if (candidate.Languages.Where(x => x.Name == item.Name).Any())
                        return false;
                }
            }
            return true;
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
        #endregion

        ~ViewModel()
        {
            Dispose();
        }
    }
}
