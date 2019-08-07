using GoLV2.scr.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GoLV2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public struct ButtonCard
        {
            public String name;
            public CanvasButton btn;
        }

        bool take_ini_world = false;
        GoLWorld golWorld;

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static ButtonCard[,] BtnHolder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataObject.AddPastingHandler(txtMyText1, TextBoxPasting);
            DataObject.AddPastingHandler(txtMyText2, TextBoxPasting);

            //initialized
            golWorld = new GoLWorld(30, 30);
            drawField(golWorld.getRowLength(), golWorld.getColumnLength());
            take_ini_world = true;
        }


        private void drawField(double _rows, double _columns)
        {
            Canvas canv = (Canvas)FindName("MainCanvas");
            Grid grid = (Grid)FindName("MainGrid");

            if (canv.Children.Count > 0)
            {
                canv.Children.Clear();
            }

            double column = _columns;
            double rows = _rows;
            double posL = 0;
            double posT = 0;
            double sizeW = canv.ActualWidth / column;
            double sizeH = canv.ActualHeight / rows;



            BtnHolder = new ButtonCard[(int)rows, (int)column];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    //create a button at the correct position
                    CanvasButton btn = new CanvasButton(i, j);
                    btn.Background = Brushes.White;
                    btn.Width = sizeW;
                    btn.Height = sizeH;
                    Canvas.SetLeft(btn, posL);
                    Canvas.SetTop(btn, posT);
                    canv.Children.Add(btn);

                    //create a card that stores all important information
                    ButtonCard tmpBtnCard = new ButtonCard();
                    tmpBtnCard.name = "Button_" + i + "_" + j;
                    tmpBtnCard.btn = btn;
                    tmpBtnCard.btn.Click += new RoutedEventHandler(button_Click);
                    //assigne the ButtonCard to the correct position inside the holder
                    BtnHolder[i, j] = tmpBtnCard;

                    posL += sizeW;
                }
                posL = 0;
                posT += sizeH;
            }
        }

        /// <summary>
        /// Depending on the buttons color, a click on it
        /// will set the given cell at the position to alive or death.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            CanvasButton button = sender as CanvasButton;
            if (button.Background == Brushes.Black)
            {
                golWorld.setValue(button._row, button._column, 0);//dead
                button.Background = Brushes.White;
                //System.Console.WriteLine("button pos x,y: " +button._row + " " + button._column);
            }
            else
            {
                golWorld.setValue(button._row, button._column, 1);//alive
                button.Background = Brushes.Black;
                //System.Console.WriteLine("button pos x,y: " +button._row + " " + button._column);
            }
        }

        void OnNew(object sender, RoutedEventArgs e)
        {
            //check if textfields were 0 or not
            TextBox tb1 = (TextBox)FindName("txtMyText1");
            TextBox tb2 = (TextBox)FindName("txtMyText2");

            int rows = 0;
            int cols = 0;
            int.TryParse(tb1.Text, out rows);
            int.TryParse(tb2.Text, out cols);

            //if text fields were 0, dont draw, take ini world
            if (!(take_ini_world && (rows == 0 || cols == 0)))
            {
                take_ini_world = false;
                golWorld = new GoLWorld(rows, cols);
            }

            drawField(golWorld.getRowLength(), golWorld.getColumnLength());
        }

        void OnStartEvolve(object sender, RoutedEventArgs e)
        {

            golWorld.evolveFirstGen();
            golWorld.nextGen();
            int[,] currentWold = golWorld.getWold();

            for (int i = 0; i < golWorld.getRowLength(); i++)
            {
                for (int j = 0; j < golWorld.getColumnLength(); j++)
                {
                    if (currentWold[i, j] == 0)
                    {
                        BtnHolder[i, j].btn.Background = Brushes.White;
                    }
                    if (currentWold[i, j] == 1)
                    {
                        BtnHolder[i, j].btn.Background = Brushes.Black;
                    }
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Prohibit non-alphabetic
            if (!IsTextAllowed(e.Text))
                e.Handled = true;
        }

        /// <summary>
        /// Pasting text box event handler allows only numbers to be pasted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Prohibit space
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Length < 1)
            {
                MessageBox.Show("Please insert a size for your gameworld");
            }
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void txtMyText1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
