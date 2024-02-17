using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace System_Programing_Thread_Homework2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int maxValue;
        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                maxValue = value;
                OnPropertyChanged();

            }
        }

        private int fileValue;
        public int FileValue
        {
            get { return fileValue; }
            set
            {
                fileValue = value;
                OnPropertyChanged();
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        private string text2;
        public string Text2
        {
            get { return text2; }
            set
            {
                text2 = value;
                OnPropertyChanged();
            }
        }
        public Thread WorkerThread { get; set; }
        public string srcPath { get; set; }
        public string destPath { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MaxValue = 100;
            
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
                    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "butun fayllar (*.*)|*.*";


            if (fileDialog.ShowDialog() == true)
            {
                fromTextBox.Text = fileDialog.FileName;
                WorkerThread = new Thread(f);
                WorkerThread.Start();
                void f()
                {
                    srcPath = fileDialog.FileName;
                    
                }

            }
        }

        private void OpenFile2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "butun fayllar (*.*)|*.*";


            if (fileDialog.ShowDialog() == true)
            {
                toTextBox.Text = fileDialog.FileName;
                WorkerThread = new Thread(f);
                WorkerThread.Start();
                void f()
                {
                    
                    destPath = fileDialog.FileName;
                }
            }
        }

        private void CopyFile(object sender, RoutedEventArgs e)
        {
            //Thread thread = new Thread(CopyData);
            WorkerThread = new Thread(CopyData);
            WorkerThread.Start();
            pb.Value = 0;
            //CopyData();


        }
        public void CopyData()
        {
            //string srcPath = @$"C:\Users\{Environment.UserName}\Desktop\from.txt";
            //string destPath = @$"C:\Users\{Environment.UserName}\Desktop\to.txt";

            
            if (!File.Exists(srcPath))
            {
                MessageBox.Show("From File not exists", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            if (!File.Exists(destPath))
            {
                MessageBox.Show("To File not exists", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            try
            {

                using (FileStream fsRead = new FileStream(srcPath, FileMode.Open, FileAccess.Read))
                {
                    Console.WriteLine($"Size {fsRead.Length} bytes");
                    using (FileStream fsWrite = new FileStream(destPath, FileMode.Create, FileAccess.Write))
                    {
                        var len = 10;
                        var fileSize = fsRead.Length;

                        MaxValue = (int)fileSize;
                        //MessageBox.Show(MaxValue.ToString());
                        byte[] buffer = new byte[len];


                        do
                        {
                            Thread.Sleep(10);
                            len = fsRead.Read(buffer, 0, buffer.Length); // 8
                            fsWrite.Write(buffer, 0, len);

                            Console.WriteLine(fileSize);
                            fileSize -= len;
                            FileValue += len;
                            
                            //MessageBox.Show(FileValue.ToString());

                        } while (len != 0);

                    }
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Change(object sender, TextChangedEventArgs e)
        {

            srcPath = fromTextBox.Text;
        }

        private void Change2(object sender, TextChangedEventArgs e)
        {

            destPath = toTextBox.Text;
        }
    }
}
