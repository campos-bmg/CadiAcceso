using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CadiAcceso
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int factorEscalado = 18;
        public MainWindow()
        {
            //Se crea la ventana principal (y única)
            InitializeComponent();
            //Se activa el foco en el cuadro de texto invisible...
            //...para que el escaner escriba en el sin acción por...
            //...parte del usuario
            txtMatricula.Focus();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*
             Al cambiar el tamaño de la ventana (aumentar o reducir)
            Se manda a llamar a la función Reescalado que cambia el tamaño
            del texto acorde al tamaño de la ventana, le pasamos como parametro
            el tamaño actual de la ventana para que realice el calculo
             */
            Reescalado(Ventana.Height);   
        }
        /*Función reescalado que acepta como argumento un
         dato de tipo double con el alto de la ventana para que realice
        el calculo y asigne el tamaño adecuado al texto
         */
        private void Reescalado(double AltoVentana)
        {
            /*
             Se divide el alto de la ventana obtenido al llamar a la función
            entre el factor de escalado para obtener el tamaño del texto adecuado
             */
            lblNombre.FontSize = AltoVentana / factorEscalado;
            lblCarrera.FontSize = AltoVentana / factorEscalado;
            lblSemestre.FontSize = AltoVentana / factorEscalado;
            lblModalidad.FontSize = AltoVentana / factorEscalado;
            lblMatricula.FontSize = AltoVentana / factorEscalado;
        }

        private void btnClose_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
           ImageBrush b = new ImageBrush();
             b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Close_hover.png"));
             btnClose.Fill = b;
        }

        private void btnClose_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ImageBrush b = new ImageBrush();
            b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Close.png"));
            btnClose.Fill = b;
        }

        private void btnClose_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btnMax_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(this.WindowState == WindowState.Normal)
            {
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Max_hover.png"));
                btnMax.Fill = b;
            }
            else if(this.WindowState == WindowState.Maximized)
            {
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Min_hover.png"));
                btnMax.Fill = b;
            }
            
        }

        private void btnMax_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Max.png"));
                btnMax.Fill = b;
            }
            else if(this.WindowState == WindowState.Maximized)
            {
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Min.png"));
                btnMax.Fill = b;
            }
        }

        private void btnMax_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Min.png"));
                btnMax.Fill = b;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState= WindowState.Normal;
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Max.png"));
                btnMax.Fill = b;
            }
            Reescalado(this.ActualHeight);
        }

        private void txtMatricula_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                bool numeros = true;
               foreach(char c in txtMatricula.Text)
                {
                    switch (c)
                    {

                        case '1': break;
                        case '2': break;
                        case '3': break;
                        case '4': break;
                        case '5': break;
                        case '6': break;
                        case '7': break;
                        case '8': break;
                        case '9': break;
                        case '0': break;
                            default: numeros = false; break;

                    }
                   
                }
                if (numeros)
                {
                    lblNombre.Content = txtMatricula.Text;
                    //consulta(txtMatricula.Text);
                }
                else
                {
                    MessageBox.Show("El código escaneado no es valido, intente de nuevo");
                }
                
            }
        }

        private void txtMatricula_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMatricula.Focus();
        }

        private void Cuadricula_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
