using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    class ProgrammingLanguage : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        public virtual ICollection<Candidate> Candidates { get; set; }

        public ProgrammingLanguage()
        {
            Candidates = new List<Candidate>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
