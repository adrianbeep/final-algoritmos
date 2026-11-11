using MySqlConnector;
using System;
using System.Drawing;
using System.Windows.Forms;
using SistemaCoworking.Datos;
using SistemaCoworking.Herramientas;

namespace SistemaCoworking.Formularios;

/// <summary>
/// La pantalla de indicadores del sistema.
///
/// Es un form embebido en el panel del FrmPrincipal. Muestra cuatro tarjetas
/// con los valores que devuelven las funciones de indicadores de la base.
/// </summary>
public partial class FrmDashboard : Form
{
    private DashboardDAO _dao = new DashboardDAO();

    // El color de acento de cada tarjeta, y los umbrales del color de la
    // cuarta, que dependen de cuanto occupancy hay.
    private static readonly Color ColorAcentoTotal = Color.FromArgb(0, 120, 215);
    private static readonly Color ColorAcentoPendientes = Color.FromArgb(255, 159, 28);
    private static readonly Color ColorAcentoHoy = Color.FromArgb(56, 142, 60);
    private static readonly Color ColorOcupacionAlta = Color.FromArgb(198, 40, 40);

    public FrmDashboard()
    {
        InitializeComponent();

        ButtonConfiguration.ConfigSecondary(btnActualizar);

        CargarIndicadores();
    }

    /// <summary>
    /// Carga los indicadores y los muestra en las tarjetas.
    ///
    /// Las cuatro tarjetas muestran un valor numerico, salvo la cuarta, que
    /// muestra el porcentaje de ocupacion de hoy.
    /// </summary>
    private void CargarIndicadores()
    {
        try
        {
            lblValorCard1.Text = _dao.ObtenerTotalClientes().ToString();
            lblValorCard1.ForeColor = ColorAcentoTotal;
            lblValorCard2.Text = _dao.ObtenerReservasPorFinalizar().ToString();
            lblValorCard2.ForeColor = ColorAcentoPendientes;
            lblValorCard3.Text = _dao.ObtenerReservasHoy().ToString();
            lblValorCard3.ForeColor = ColorAcentoHoy;

            // La ocupacion de hoy es la proporcion de recursos que estan disponibles y
            // tienen una reserva confirmada para hoy. Si no hay recursos disponibles,
            // no hay nada que ocupar.
            int disponibles = _dao.ObtenerRecursosDisponibles();
            int reservadosHoy = _dao.ObtenerRecursosReservadosHoy();

            double porcentaje = disponibles == 0
                ? 0
                : (double)reservadosHoy / disponibles * 100;

            int porcentajeRedondeado = (int)Math.Round(porcentaje);

            lblValorCard4.Text = porcentajeRedondeado + " %";

            // El color de la cuarta tarjeta avisa cuanto hay de occupancy: rojo si
            // esta casi lleno, naranja si esta a la mitad, verde si hay lugar.
            if (porcentajeRedondeado > 80)
            {
                lblValorCard4.ForeColor = ColorOcupacionAlta;
            }
            else if (porcentajeRedondeado > 50)
            {
                lblValorCard4.ForeColor = ColorAcentoPendientes;
            }
            else
            {
                lblValorCard4.ForeColor = ColorAcentoHoy;
            }
        }
        catch (MySqlException ex)
        {
            // El DAO lanza y aca se captura. El SQLSTATE 45000 es una regla de
            // negocio: el mensaje es del dominio y se muestra tal cual.
            if (ex.SqlState == "45000" || ex.Number == 1644)
            {
                MessageBox.Show(
                    ex.Message,
                    "No se pudo completar la operacion",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(
                    "Ocurrio un error al trabajar con la base de datos. Intente de nuevo."
                        + Environment.NewLine + Environment.NewLine + ex.Message,
                    "Error de base de datos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            lblValorCard1.Text = "-";
            lblValorCard2.Text = "-";
            lblValorCard3.Text = "-";
            lblValorCard4.Text = "-";

            lblValorCard1.ForeColor = ColorAcentoTotal;
            lblValorCard2.ForeColor = ColorAcentoPendientes;
            lblValorCard3.ForeColor = ColorAcentoHoy;
            lblValorCard4.ForeColor = ColorAcentoHoy;

            MessageBox.Show(
                "Ocurrio un error inesperado. Intente de nuevo."
                    + Environment.NewLine + Environment.NewLine + ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnActualizar_Click(object sender, EventArgs e)
    {
        CargarIndicadores();
    }
}
