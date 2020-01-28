using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Resume
{
    class Candidate : DependencyObject, IDataErrorInfo
    {
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(Candidate), new PropertyMetadata(0));

        public string Name 
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Candidate), new PropertyMetadata(""));

        public string MidleName
        {
            get { return (string)GetValue(MidleNameProperty); }
            set { SetValue(MidleNameProperty, value); }
        }

        public static readonly DependencyProperty MidleNameProperty =
            DependencyProperty.Register("MidleName", typeof(string), typeof(Candidate), new PropertyMetadata(""));

        public string Surname
        {
            get { return (string)GetValue(SurnameProperty); }
            set { SetValue(SurnameProperty, value); }
        }

        public static readonly DependencyProperty SurnameProperty =
            DependencyProperty.Register("Surname", typeof(string), typeof(Candidate), new PropertyMetadata(""));

        public int Age
        {
            get { return (int)GetValue(AgeProperty); }
            set { SetValue(AgeProperty, value); }
        }

        public static readonly DependencyProperty AgeProperty =
            DependencyProperty.Register("Age", typeof(int), typeof(Candidate), new PropertyMetadata(0));

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(Candidate), new PropertyMetadata(""));

        public string Phone
        {
            get { return (string)GetValue(PhoneProperty); }
            set { SetValue(PhoneProperty, value); }
        }

        public static readonly DependencyProperty PhoneProperty =
            DependencyProperty.Register("Phone", typeof(string), typeof(Candidate), new PropertyMetadata(""));

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(Candidate), new PropertyMetadata(""));

        public string MaritalStatus
        {
            get { return (string)GetValue(MaritalStatusProperty); }
            set { SetValue(MaritalStatusProperty, value); }
        }

        public static readonly DependencyProperty MaritalStatusProperty =
            DependencyProperty.Register("MaritalStatus", typeof(string), typeof(Candidate), new PropertyMetadata(""));

        public string Education
        {
            get { return (string)GetValue(EducationProperty); }
            set { SetValue(EducationProperty, value); }
        }

        public static readonly DependencyProperty EducationProperty =
            DependencyProperty.Register("Education", typeof(string), typeof(Candidate), new PropertyMetadata(""));
        public virtual ICollection<ProgrammingLanguage> ProgrammingLanguages { get; set; }
        public virtual ICollection<Framework> Frameworks { get; set; }
        public virtual ICollection<Language> Languages { get; set; }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Age":
                        if ((Age < 18) || (Age > 65))
                        {
                            error = "Возраст должен быть больше 17 и меньше 66";
                        }
                        break;
                    case "Email":
                        if(string.IsNullOrEmpty(Email))
                        {
                            error = "Email является обязательным полем!";
                        }
                        else if(!IsValidEmail)
                        {
                            error = "Некорректный ввод Email!";
                        }
                        break;
                }
                return error;
            }
        }

        private Regex emailReg;

        public bool IsValidEmail
        {
            get { return emailReg.IsMatch(Email); }
        }
        public Candidate() 
        {
            emailReg = new Regex(@"^([\w\.\-]+)@([a-z\-]+)((\.([a-z]){2,3})+)$");
            ProgrammingLanguages = new ObservableCollection<ProgrammingLanguage>();
            Languages = new ObservableCollection<Language>();
            Frameworks = new ObservableCollection<Framework>();
        }

        public Candidate(Candidate candidate)
        {
            emailReg = new Regex(@"^([\w\.\-]+)@([a-z\-]+)((\.([a-z]){2,3})+)$");
            Name = candidate.Name;
            Surname = candidate.Surname;
            MidleName = candidate.MidleName;
            Age = candidate.Age;
            Address = candidate.Address;
            Phone = candidate.Phone;
            Email = candidate.Email;
            MaritalStatus = candidate.MaritalStatus;
            Education = candidate.Education;
            ProgrammingLanguages = candidate.ProgrammingLanguages;
            Frameworks = candidate.Frameworks;
            Languages = candidate.Languages;
        }
    }
}
