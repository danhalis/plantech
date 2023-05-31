using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

/*
 * Team: PlanTech (Team #1)
 * Semester: Winter 2022
 * Course: 420-6A6-AB APPLICATION DEVELOPMENT III
 * Date: 03 May 2022
 */
namespace PlanTech.ViewModels
{
    /// <summary>
    /// Represents a communication between a domain model and a view.
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        virtual public INavigation Navigation { get; set; }
    }
}

