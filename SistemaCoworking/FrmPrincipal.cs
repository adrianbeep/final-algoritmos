using SistemaCoworking.Formularios;
using SistemaCoworking.Herramientas;
using System.Drawing.Text;

namespace SistemaCoworking;

/*
 La finalidad de este formulario es pura y exclusivamente para
 abrir y cerrar formularios en un panel
 esto nos garantiza de que aca no se toca nada sobre clientes, nada sobre recursos
 nada sobre reservas, es pura y exclusivamente de orquestacion de formularios.

 en un principio ibamos a usar mdi pero investigando bien tiene algunos errores al 
 cambiar de tamaño y todo el sistema sera responsive, entre sus alternativas
 usar un form embed que es embeber un formulario a un contenedor que herede de la clase
'Control' como por ejemplo panel, flowlayoutpanel, tablelayoutpanel, etc
 donde podemos abrir un formulario de manera que no usamos el motor del mdi que es un mas pesado
 la otra es usar un usercontrol que es basicamente hermano de un form pero tiene menos caracteristicas
 como por ej, el usercontrol tiene el evento load pero no tiene showdialog, windowstate, evento closed
 
 usamos form embed y simular que el sistema es un SPA (single page application)
 */

public partial class FrmPrincipal : Form
{
    //private Color btnBGColor = ;
    private Form _formActivo = null;
    private Button _botonActivo = null;
    private Form _overlay = null;

    //==============================
    //EVENTOS FORMULARIO
    //==============================
    public FrmPrincipal()
    {
        InitializeComponent();

        // pnModal del Designer ya no se usa: el velo real es una ventana propia
        // con Opacity (ver ModalOverlay). Un Panel hijo no compone con sus
        // hermanos y se ve gris opaco.
        pnForms.Controls.Remove(pnModal);

        _overlay = ModalOverlay.Configure(this, opacidad: 0.55);

        CargarFormularioAPanel(new FrmDashboard(), btnDashboard);
    }

    //==============================
    //TRANSICION SIDEBAR
    //==============================
    private bool sidebarExpand = true; // por defecto esta expandida
    private void sidebarTransicion_Tick(object sender, EventArgs e)
    {
        //representa tamaño maximo de ancho del sidebar
        const int expandida = 250;
        //representa tamaño minimo de ancho del sidebar
        const int cerrada = 60;

        if (sidebarExpand)
        {
            flpSidebar.Width -= 5;
            if (flpSidebar.Width <= cerrada)
            {
                sidebarExpand = false;
                sidebarTransicion.Stop();
            }
        }
        else
        {
            flpSidebar.Width += 5;
            if (flpSidebar.Width >= expandida)
            {
                sidebarExpand = true;
                sidebarTransicion.Stop();
            }
        }

        pnDashboard.Width = flpSidebar.Width;
        pnClientes.Width = flpSidebar.Width;
        pnRecursos.Width = flpSidebar.Width;
        pnReservas.Width = flpSidebar.Width;
    }

    private void btnHamburguesa_Click(object sender, EventArgs e)
    {
        sidebarTransicion.Start();

    }

    //==============================
    //CARGAR FORMULARIOS A PANEL
    //==============================
    private void CargarFormularioAPanel(Form formHijo, Button boton)
    {
        if (_formActivo != null)
        {
            _formActivo.Close();
            _formActivo.Dispose();
            _formActivo = null;
        }

        if (_botonActivo != null)
        {
            _botonActivo.BackColor = Color.FromArgb(23, 24, 29);
            _botonActivo.Enabled = true;
            _botonActivo = null;
        }

        _formActivo = formHijo;
        _formActivo.TopLevel = false;
        _formActivo.FormBorderStyle = FormBorderStyle.None;
        _formActivo.StartPosition = FormStartPosition.Manual;
        _formActivo.Dock = DockStyle.Fill;

        pnForms.Controls.Clear();
        pnForms.Controls.Add(_formActivo);
        _formActivo.Show();

        _botonActivo = boton;
        _botonActivo.Enabled = false;
        _botonActivo.BackColor = Color.FromArgb(80, 84, 101);
    }

    //==============================
    //BOTONES
    //==============================
    private void btnDashboard_Click(object sender, EventArgs e)
    {
        CargarFormularioAPanel(new FrmDashboard(), btnDashboard);
    }

    private void btnClientes_Click(object sender, EventArgs e)
    {
        CargarFormularioAPanel(new FrmClientes(_overlay), btnClientes);
    }

    private void btnRecursos_Click(object sender, EventArgs e)
    {
        CargarFormularioAPanel(new FrmRecursos(), btnRecursos);
    }

    private void btnReservas_Click(object sender, EventArgs e)
    {
        CargarFormularioAPanel(new FrmReservas(), btnReservas);
    }
}