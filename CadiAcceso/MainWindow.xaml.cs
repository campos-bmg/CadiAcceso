using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Data;
using MySql.Data.MySqlClient;

namespace CadiAcceso
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private int factorEscalado = 18;
        private int tiempoMostrar = 5;
        private int contador = 0;
        private DispatcherTimer tmr = new DispatcherTimer();
        private Color _sombras = Color.FromRgb(0, 0, 0);
        public Color sombras
        {
            get{ return _sombras; }
            set{ _sombras = value; }

        }
        public MainWindow()
        {

        DataContext = this;
            //Se crea la ventana principal (y única)
            InitializeComponent();
            //Se activa el foco en el cuadro de texto invisible...
            //...para que el escaner escriba en el sin acción por...
            //...parte del usuario
            int hola = 1;
            txtMatricula.IsReadOnly = true;
            txtMatricula.Focus();
            ClearData();
            imgAlumno.Width = 96.35;
            tmr.Interval = TimeSpan.FromSeconds(1);
            tmr.Tick += Tmr_Tick;
        }

        /*
         Al cambiar el tamaño de la ventana (aumentar o reducir)
        Se manda a llamar a la función Reescalado que cambia el tamaño
        del texto acorde al tamaño de la ventana, le pasamos como parametro
        el tamaño actual de la ventana para que realice el calculo
         */
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        { 
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
            imgAlumno.MinWidth = imgAlumno.ActualHeight / 3 * 2.5;
            lblNombre.FontSize = AltoVentana / factorEscalado;
            lblCarrera.FontSize = AltoVentana / factorEscalado;
            lblSemestre.FontSize = AltoVentana / factorEscalado;
            lblModalidad.FontSize = AltoVentana / factorEscalado;
            lblMatricula.FontSize = AltoVentana / factorEscalado;
        }
        
         //Función que ocurre al pasar el mouse sobre el boton cerrar
         
        private void btnClose_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //Nuevo objeto de tipo imageBrush
           ImageBrush b = new ImageBrush();
            //Seteamos la propiedad imagesource del objeto recien creado a un bitmap
            //en una uri que apunta a la imagen Close_hover.png dentro de los archivos de la aplicación
             b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Close_hover.png"));
            //Asignamos como relleno de el botón close el objeto b con la imagen antes mencionada
             btnClose.Fill = b;
        }
        //Función que ocurre al retirar el mouse del boton cerrar
        //Trabaja de la misma manera que la anterior, solo que ahora el relleno es 
        //la imagen Close.png que es su estado normal y por defecto
        private void btnClose_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ImageBrush b = new ImageBrush();
            b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Close.png"));
            btnClose.Fill = b;
        }
        //Se cierra la ventana al hacer click sobre el boton Close
        private void btnClose_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
        //Se ejecuta al pasar el mouse sobre el boton Maximizar/minimizar
        private void btnMax_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
           //Si el estado de la ventana es normal
            if(this.WindowState == WindowState.Normal)
            {   //Se cambia la imagen de fondo de btnMax por Max_hover.png
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Max_hover.png"));
                btnMax.Fill = b;
            }
            //Si el estado de la ventana es maximizado
            else if(this.WindowState == WindowState.Maximized)
            {   //Se cambia la imagen de fondo de btnMax por Min_hover 
                //Se usa esta imagen para diferenciar el boton de maximizar por el de restaurar
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Min_hover.png"));
                btnMax.Fill = b;
            }
            
        }
        //Esto restablece las imagenes de fondo por defecto de btnMax cuando se retira el mouse
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
        //Ocurre cuando se levanta el click sobre el boton btnMax
        private void btnMax_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {   //Si la ventana está normal, la maximiza y cambia su icono del boton
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Min.png"));
                btnMax.Fill = b;
            }
            //Si la ventana está maximizada, la pone en estado normal y cambia su icono del boton
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState= WindowState.Normal;
                ImageBrush b = new ImageBrush();
                b.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/img/1x/Max.png"));
                btnMax.Fill = b;
            }
            //Cada que ocurre un cambio de estado de la ventana por el boton btnMax
            //se manda a reescalar la fuente, se manda como parametro para el reescalado la 
            //altura actual de la ventana
            Reescalado(this.ActualHeight);
        }
        //Cada que se levanta la presión de una tecla del teclado dentro del textbox txtMatricula
        private void txtMatricula_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            lblMatricula.Content = e.Key;
            //Se evalua, si la tecla recién presionada es igual a Enter
            //Esto funciona por que el lector de codigo de barras debe envíar un
            //Enter cada que realiza una lectura
            if (!txtMatricula.IsReadOnly) { 

                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    //Variable para almacenar la matricula solo con digitos y eliminar el caracter '+'
                    string matriculaclean = "";
                    //bandera que evalua si se ingresaron solo numeros, por defecto es true
                    bool numeros = true;
                    //ciclo que evalua cada caracter de la cadena ingresada en txtMatricula
                    foreach (char c in txtMatricula.Text)
                    {
                        //switch que evalua el caracter actual de la cadena
                        //Si es un digito (1,2,3,4,5,6,7,8,9,0) o el simbolo '+' no hace nada
                        //Si es otro caracter distinto, cambia el valor de la bandera a false
                        //Por cada digito encontrado, lo añade a la cadena matriculaclean para
                        //obtener la matricula sin el simbolo '+' y queden solo los digitos
                        switch (c)
                        {

                            case '1': matriculaclean += c; break;
                            case '2': matriculaclean += c; break;
                            case '3': matriculaclean += c; break;
                            case '4': matriculaclean += c; break;
                            case '5': matriculaclean += c; break;
                            case '6': matriculaclean += c; break;
                            case '7': matriculaclean += c; break;
                            case '8': matriculaclean += c; break;
                            case '9': matriculaclean += c; break;
                            case '0': matriculaclean += c; break;
                            case '+': break;
                            default: numeros = false; break;

                        }
                        //Invertimos el valor de numeros, si es verdadero (falso) quiere decir
                        //Que se ingresó algún otro caracter en la posición actual y se rompe
                        //el ciclo foreach
                        if (!numeros) break;
                    }
                    //Evaluamos la bandera numeros, si es verdadero quiere decir que todo es correcto
                    //Y llamamos a la función Consulta con sobrecarga de string
                    //Si es falso quiere decir que se ingreso un caracter incorrecto
                    if (numeros) Consulta(matriculaclean);
                    else MessageBox.Show("El código escaneado no es valido, intente de nuevo");
                    
                    txtMatricula.Text = "";
                    txtMatricula.IsReadOnly = true;
                } }
            else if(e.Key == System.Windows.Input.Key.OemPlus)
            {
                txtMatricula.Text = "";
                txtMatricula.IsReadOnly = false;
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
        //Aqui te paso la string matricula como parametro para tu metodo, para que con ella hagas la
        //busqueda en la base de datos

        
        private void Consulta(string matricula)
        {

            string nombre, semestre, modalidad, carrera;
            bool error = false;
            //tu codigo de consulta empieza aquí

            MySql.Data.MySqlClient.MySqlConnection conec;
            string datos = "server='72.29.120.15'; user id='calormex_gio'; password='hardstyle8.0';database='calormex_ugmex'";
            try
            {
                conec = new MySql.Data.MySqlClient.MySqlConnection();
                conec.ConnectionString = datos;
                conec.Open();
                MySqlCommand coman2 = new MySqlCommand();
                coman2.Connection = conec;
                coman2.CommandText = "SELECT Nombre, Semestre, Modalidad, Carrera FROM Alumnos WHERE Matricula like'" + matricula + "'";
                MySqlDataReader Read = coman2.ExecuteReader();
                if (Read.Read())
                {
                    nombre = Read.GetString(0);
                    semestre = Read.GetString(1); ;
                    modalidad = Read.GetString(2);
                    carrera = Read.GetString(3);
                    MostrarDatos(nombre, semestre, matricula, modalidad, carrera);
                    
                }
                conec.Close();            
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            

            //Asigna valor a la variable error, le asignas true si es que no se encontró nada en la base de datos
            //o si en general no se pudo conectar con la base de datos, le asignas false cuando si se encuentre algo en
            //la base de datos
            if (error)
            {//esta parte la dejas sin modificar
                MostrarDatos();
            }
            //Aqui quitas los valores que puse y pones los valores que obtuviste de la base
            //de datos en las variables correspondientes
            /*else
            {
                
                
            } */
            //Aquí termina tu codigo de consulta
        }

        private void MostrarDatos()
        {
            
            lblNombre.Foreground = new SolidColorBrush(Color.FromRgb(242,46,98));
            lblNombre.Content = "No se encontró la matricula escaneada, intente de nuevo";
            imgAlumno.Stroke = new SolidColorBrush(Color.FromRgb(242, 46, 98));
            tmr.Start();
        }
        private void MostrarDatos(string nombre, string semestre, string matricula, string modalidad, string carrera)
        {
            contador = 0;
            lblNombre.Content = nombre;
            lblSemestre.Content = semestre+" semestre";
            lblMatricula.Content = matricula;
            lblModalidad.Content = modalidad;
            lblCarrera.Content = carrera;
            imgAlumno.Fill = new ImageBrush(new BitmapImage(new Uri($"http://cadi.calormex.com/img/{matricula}.jpg")));
            tmr.Start();

        }

        private void Tmr_Tick(object? sender, EventArgs e)
        {
            contador++;
            if(contador >= tiempoMostrar)
            {
                contador = 0;
                ClearData();
                tmr.Stop();
            }
        }

        private void ClearData()
        {
            lblNombre.Content = "Preparado para escanear...";
            lblSemestre.Content = "";
            lblModalidad.Content = "";
            lblCarrera.Content = "";
            lblMatricula.Content = "";
            lblNombre.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            imgAlumno.Fill = new SolidColorBrush(Color.FromRgb(57, 62, 89));
            imgAlumno.Stroke = null;
            
        }
    }
}
