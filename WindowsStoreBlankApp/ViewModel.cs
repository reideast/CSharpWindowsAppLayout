using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

//For RESTful Entity Model interaction:
using System.Net.Http;
using System.Net.Http.Headers;

namespace WindowsStoreBlankApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        private const string serverURL = "http://localhost:50000/";
        private HttpClient client = null;

        private List<Customer> customers;
        public List<Customer> AllCustomers
        {
            get { return this.customers; }
        }

        private int currentCustomer;

        public Command NextCustomer { get; private set; }
        public Command PreviousCustomer { get; private set; }

        private bool _isAtStart;
        public bool IsAtStart
        {
            get { return this._isAtStart; }
            set
            {
                this._isAtStart = value;
                this.OnPropertyChanged("IsAtStart");
            }
        }

        private bool _isAtEnd;
        public bool IsAtEnd
        {
            get { return this._isAtEnd; }
            set
            {
                this._isAtEnd = value;
                this.OnPropertyChanged("IsAtStart");
            }
        }

        public ViewModel()
        {
            this.currentCustomer = 0;
            this.IsAtStart = true;
            this.IsAtEnd = false;
            this.NextCustomer = new Command(this.Next, () => { return this.customers != null && this.customers.Count > 0 && !this.IsAtEnd; });
            this.PreviousCustomer = new Command(this.Previous, () => { return this.customers != null && this.customers.Count > 0 && !this.IsAtStart; });

            //this.customers = DataSource.Customers;
            this.customers = null;
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(serverURL);
            this.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task GetDataAsync()
        {
            try
            {
                this.IsBusy = true;
                var response = await this.client.GetAsync("api/customers"); //NOTE: fetches ALL rows! bad idea in a production program
                if (response.IsSuccessStatusCode)
                {
                    var customerData = await response.Content.ReadAsAsync<IEnumerable<Customer>>();
                    this.customers = customerData as List<Customer>;
                    this.currentCustomer = 0;
                    this.OnPropertyChanged("Current");
                    this.IsAtStart = true;
                    this.IsAtEnd = (this.customers.Count == 0);
                }
                else
                {
                    //TODO: Handle GET fail
                }
            }
            catch (Exception e)
            {
                //TODO: Handle exception
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return this._isBusy; }
            set
            {
                this._isBusy = value;
                this.OnPropertyChanged("IsBusy");
            }

        }

        public Customer Current 
        {
            get 
            {
                if (this.customers != null)
                    return this.customers[currentCustomer];
                else
                    return null;
            } 
        }

        public void GoTo(Customer customer)
        {
            this.currentCustomer = this.customers.IndexOf(customer);
            this.OnPropertyChanged("Current");
            this.IsAtStart = (this.currentCustomer == 0);
            this.IsAtEnd = (this.customers.Count - 1 == this.currentCustomer);
        }

        private void Next()
        {
            if (this.customers.Count - 1 > this.currentCustomer)
            {
                this.currentCustomer++;
                this.OnPropertyChanged("Current");
                this.IsAtStart = false;
                this.IsAtEnd = (this.customers.Count - 1 == this.currentCustomer);
            }
        }

        private void Previous()
        {
            if (this.currentCustomer > 0)
            {
                this.currentCustomer--;
                this.OnPropertyChanged("Current");
                this.IsAtEnd = false;
                this.IsAtStart = (this.currentCustomer == 0);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
