using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.Data.SqlClient;
using SQLite;

namespace FreePlantcer
{
    public partial class MainWindow : Window
    {
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;
        string rateString;
        double rate = 15.00;
        double hourlyRate;
        string hoursWorked;
        List<Contact> contacts;

        public MainWindow()
        {

            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            calendar1.SelectedDate = DateTime.Today;

            contacts = new List<Contact>();
            
        }
        void DeleteDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.Execute("DELETE FROM Contact");
            }
        }
        void WriteDatabase()
        {
            Contact contact = new Contact()
            {
                Date = datetxtblock.Text,
                loggedTime = clocktxtblock.Text,
                incomeMade = incomebox.Text,

            };

            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<Contact>();
                conn.Insert(contact);
            }
        }
        void ReadDatabase()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
            {
                conn.CreateTable<Contact>();
                contacts = (conn.Table<Contact>().ToList()).OrderBy(c => c.Date).ToList();
            }

            if (contacts != null)
            {
                elapsedtimeitem.Text = String.Join(", ", contacts) + "\n";
            }
        }

        void dt_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;

                rateString = ratebox.Text;
                rate = double.Parse(rateString);
                hoursWorked = String.Format("{0}.{1}", ts.Hours, ts.Minutes / 60);



                hourlyRate = rate * double.Parse(hoursWorked);


                timelogged.Text = hourlyRate.ToString();

                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
                clocktxtblock.Text = currentTime;

            }
        }

        // Start Button
        private void startbtn_Click(object sender, RoutedEventArgs e)
        {
            sw.Start();
            dt.Start();
        }

        // Stop Button
        private void stopbtn_Click(object sender, RoutedEventArgs e)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }
        }

        // Reset Button
        private void resetbtn_Click(object sender, RoutedEventArgs e)
        {
            sw.Reset();
            clocktxtblock.Text = "00:00:00";
        }

        // Log Button
        private void logbtn_Click(object sender, RoutedEventArgs e)
        {
            
            WriteDatabase();
            ReadDatabase();
        }
    }
    // Contact Class
    public class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Date { get; set; }
        public string loggedTime { get; set; }
        public string incomeMade { get; set; }
        public override string ToString()
        {
            return $" | {Date} - {loggedTime} - {incomeMade} | ";
        }
    }
}

