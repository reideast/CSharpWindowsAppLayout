using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WindowsStoreBlankApp
{
    public class Customer : INotifyPropertyChanged
    {
        private int _cusomerID;
        public int CustomerID
        {
            get { return this._cusomerID; }
            set 
            { 
                this._cusomerID = value;
                this.OnPropertyChanged("CustomerID");
            }
        }

        private string _title;
        public string Title
        {
            get { return this._title; }
            set 
            { 
                this._title = value;
                this.OnPropertyChanged("Title");
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return this._firstName; }
            set 
            {
                this._firstName = value;
                this.OnPropertyChanged("FirstName");
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return this._lastName; }
            set 
            { 
                this._lastName = value;
                this.OnPropertyChanged("LastName");
            }
        }

        private string _emailAddress;
        public string EmailAddress
        {
            get { return this._emailAddress; }
            set 
            {
                this._emailAddress = value;
                this.OnPropertyChanged("EmailAddress");
            }
        }

        private string _phone;
        public string Phone
        {
            get { return this._phone; }
            set
            {
                this._phone = value;
                this.OnPropertyChanged("Phone");
            }
        }

        public System.Guid rowguid { get; set; }
        public System.DateTime ModifiedDate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
