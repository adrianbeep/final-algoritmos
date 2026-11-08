using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using SistemaCoworking.Modelos;

namespace SistemaCoworking.Herramientas;

public static class ComboBoxConfiguration
{
    private static readonly Color TextoPrimario = Color.FromArgb(40, 40, 40);
    private static readonly Color Fondo = Color.White;

    // Se comparte una sola instancia, como en ButtonConfiguration e
    // InputConfiguration: cada Font nuevo es un handle de GDI que hay que
    // liberar.
    private static readonly Font ComboFont = new Font("Segoe UI", 9.5F);

    /// <summary>
    /// Carga el combo con los estados del recurso.
    /// Los nombres del enum son los mismos valores que usa el enum de MySQL en
    /// recursos.estado, asi que el item se puede mandar al procedimiento
    /// almacenado tal cual, sin traducir.
    /// </summary>
    public static void ConfigEstados(
        ComboBox combo,
        Recurso.EstadoRecurso? seleccionado = null)
    {
        AplicarEstilo(combo);

        combo.Items.Clear();
        combo.Items.AddRange(
            Enum.GetValues<Recurso.EstadoRecurso>().Cast<object>().ToArray());

        combo.SelectedItem = seleccionado ?? Recurso.EstadoRecurso.Disponible;
    }

    /// <summary>
    /// Carga el combo con los tipos de recurso. Cada item es el TipoRecurso
    /// completo: el combo muestra el nombre y el SelectedItem sigue siendo el
    /// objeto, asi que el Id se lee sin andar buscando por indice.
    /// </summary>
    public static void ConfigTipos(
        ComboBox combo,
        List<TipoRecurso> tipos,
        TipoRecurso? seleccionado = null)
    {
        AplicarEstilo(combo);

        combo.Items.Clear();
        foreach (TipoRecurso tipo in tipos)
        {
            combo.Items.Add(tipo);
        }

        combo.SelectedItem =
            seleccionado ?? (tipos.Count > 0 ? tipos[0] : null);
    }

    /// <summary>
    /// Estado seleccionado en el combo de estados.
    /// </summary>
    public static Recurso.EstadoRecurso EstadoSeleccionado(ComboBox combo)
    {
        return combo.SelectedItem is Recurso.EstadoRecurso estado
            ? estado
            : Recurso.EstadoRecurso.Disponible;
    }

    /// <summary>
    /// Tipo de recurso seleccionado en el combo de tipos, o null si no hay
    /// ninguno cargado.
    /// </summary>
    public static TipoRecurso? TipoSeleccionado(ComboBox combo)
    {
        return combo.SelectedItem as TipoRecurso;
    }

    //==================================================
    //COMBOS BUSCABLES
    //==================================================

    /// <summary>
    /// Configura un combo BUSCABLE de clientes: el usuario escribe y la
    /// lista se filtra por nombre, apellido o DNI.
    ///
    /// Con el texto vacio se muestra la lista entera. La busqueda es por
    /// Contains, no por StartsWith: el autocompletado nativo de WinForms
    /// busca con StartsWith y por eso queda desactivado.
    ///
    /// El Id del cliente es el Value del combo, asi que lo elegido se lee
    /// con SelectedValue.
    /// </summary>
    /// <param name="idSeleccionado">Id del cliente que queda elegido, para el modo edicion.</param>
    public static void ConfigBuscableClientes(
        ComboBox combo,
        List<Cliente> clientes,
        int? idSeleccionado = null)
    {
        ConfigBuscable(
            combo,
            clientes,
            idDe: c => c.Id,
            textoMostrado: c => $"{c.Nombre} {c.Apellido}",
            coincide: (c, t) => c.Nombre.Contains(t, StringComparison.OrdinalIgnoreCase)
                          || c.Apellido.Contains(t, StringComparison.OrdinalIgnoreCase)
                          || c.Dni.ToString().Contains(t),
            idSeleccionado);
    }

    /// <summary>
    /// Configura un combo BUSCABLE de recursos: el usuario escribe y la
    /// lista se filtra por nombre o ubicacion.
    ///
    /// El Id del recurso es el Value del combo.
    /// </summary>
    /// <param name="idSeleccionado">Id del recurso que queda elegido, para el modo edicion.</param>
    public static void ConfigBuscableRecursos(
        ComboBox combo,
        List<Recurso> recursos,
        int? idSeleccionado = null)
    {
        ConfigBuscable(
            combo,
            recursos,
            idDe: r => r.Id,
            textoMostrado: r => $"{r.Nombre} - {r.Ubicacion}",
            coincide: (r, t) => r.Nombre.Contains(t, StringComparison.OrdinalIgnoreCase)
                          || r.Ubicacion.Contains(t, StringComparison.OrdinalIgnoreCase),
            idSeleccionado);
    }

    /// <summary>
    /// Id de lo elegido en un combo buscable, o null si no hay nada
    /// seleccionado. Cada item del combo lleva su Id.
    /// </summary>
    public static int? IdSeleccionado(ComboBox combo)
    {
        return combo.SelectedItem is Envoltura<Cliente> env ? env.Id
             : combo.SelectedItem is Envoltura<Recurso> envR ? envR.Id
             : null;
    }

    /// <summary>
    /// Cliente elegido en un combo buscable de clientes, o null si no hay
    /// ninguno elegido.
    /// </summary>
    public static Cliente? ClienteSeleccionado(ComboBox combo)
    {
        return Desenvolver<Cliente>(combo.SelectedItem);
    }

    /// <summary>
    /// Recurso elegido en un combo buscable de recursos, o null si no hay
    /// ninguno elegido.
    /// </summary>
    public static Recurso? RecursoSeleccionado(ComboBox combo)
    {
        return Desenvolver<Recurso>(combo.SelectedItem);
    }

    /// <summary>
    /// Estado del combo buscable. Guarda el handler de TextChanged para
    /// poder sacarlo si el combo se vuelve a configurar: si no, el filtro
    /// se aplicaria dos veces.
    /// </summary>
    private sealed class EstadoBuscable
    {
        public EventHandler? Handler { get; set; }
    }

    private static readonly ConditionalWeakTable<ComboBox, EstadoBuscable> Estados = new();

    /// <summary>
    /// Envuelve el item original para poder mostrar otro texto en el combo y
    /// para llevar el Id. El combo guarda objetos, asi que el item va
    /// envuelto; el valor original se recupera con
    /// <see cref="Desenvolver{T}"/> y el Id con
    /// <see cref="IdSeleccionado"/>.
    ///
    /// El combo NO se bindea con DataSource a proposito: un combo con
    /// DataSource exige que el texto escrito coincida con el texto de un
    /// item, y al filtrar revierte lo que el usuario escribio. Un combo
    /// buscable necesita texto libre.
    /// </summary>
    private sealed class Envoltura<T> where T : class
    {
        public T Valor { get; }
        public int Id { get; }
        public string Texto { get; }

        public Envoltura(T valor, int id, string texto)
        {
            Valor = valor;
            Id = id;
            Texto = texto;
        }

        public override string ToString() => Texto;
    }

    /// <summary>
    /// Configura un combo BUSCABLE: el usuario escribe y la lista se filtra
    /// con <paramref name="coincide"/>.
    /// </summary>
    private static void ConfigBuscable<T>(
        ComboBox combo,
        List<T> items,
        Func<T, int> idDe,
        Func<T, string> textoMostrado,
        Func<T, string, bool> coincide,
        int? idSeleccionado) where T : class
    {
        // Editable: el usuario escribe. No se usa el AplicarEstilo de los
        // combos de lista, que fuerza DropDownList.
        combo.DropDownStyle = ComboBoxStyle.DropDown;

        // El autocompletado nativo busca con StartsWith y no sirve para
        // este filtro.
        combo.AutoCompleteMode = AutoCompleteMode.None;
        combo.AutoCompleteSource = AutoCompleteSource.None;

        combo.Font = ComboFont;
        combo.BackColor = Fondo;
        combo.ForeColor = TextoPrimario;

        // Si el combo ya estaba configurado como buscable, se saca el
        // handler anterior antes de poner el nuevo.
        EstadoBuscable estado = Estados.GetOrCreateValue(combo);
        if (estado.Handler is not null)
            combo.TextChanged -= estado.Handler;

        // El flag va capturado en el cierre: mientras se reconstruye la
        // lista, el TextChanged que dispara el propio Clear/Add/Select debe
        // ignorarse, o el filtro se realimenta en loop.
        bool reconstruyendo = false;

        EventHandler filtrar = (s, e) =>
        {
            if (reconstruyendo) return;

            // El texto y el caret se copian antes de tocar la lista: al
            // vaciarla, los dos se pierden.
            string texto = combo.Text;

            // Si el texto escrito es exactamente el texto de un item que ya
            // esta en la lista, no hay nada que filtrar: es el usuario
            // eligiendo uno de la lista desplegada. Si se reconstruyera la
            // lista en ese momento, se perderia la seleccion.
            if (IdDeTexto<T>(combo, texto) is not null) return;

            int caret = CaretActual(combo, texto.Length);

            reconstruyendo = true;
            try
            {
                Poblar(combo, items, idDe, textoMostrado, i => coincide(i, texto));

                // Al vaciar la lista de un ComboBox editable se borra tambien
                // el texto escrito, y el caret queda en 0. Se restauran los
                // dos: si no, el usuario escribe "sala" y cada tecla se
                // inserta al principio. Medido: terminaba escribiendo "alas"
                // con la lista en cero.
                combo.Text = texto;
                combo.SelectionStart = caret;

                // Si el texto escrito es exactamente el de un item, se
                // selecciona: el usuario puede haber escrito el nombre
                // completo en vez de elegirlo de la lista.
                int? id = IdDeTexto<T>(combo, texto);
                if (id is not null)
                    SeleccionarPorId<T>(combo, id.Value);
            }
            finally
            {
                reconstruyendo = false;
            }
        };

        // La lista completa se carga ANTES de enganchar el filtro: si el
        // filtro ya estuviera enganchado, la seleccion inicial dispararia
        // el TextChanged y dejaria la lista reducida a un solo item.
        Poblar(combo, items, idDe, textoMostrado, _ => true);

        if (idSeleccionado is not null)
            SeleccionarPorId<T>(combo, idSeleccionado.Value);

        estado.Handler = filtrar;
        combo.TextChanged += filtrar;
    }

    /// <summary>
    /// Recarga la lista del combo con los items que pasan el filtro.
    /// </summary>
    private static void Poblar<T>(
        ComboBox combo,
        List<T> items,
        Func<T, int> idDe,
        Func<T, string> textoMostrado,
        Func<T, bool> incluir) where T : class
    {
        combo.Items.Clear();

        foreach (T item in items)
        {
            if (!incluir(item)) continue;

            combo.Items.Add(new Envoltura<T>(item, idDe(item), textoMostrado(item)));
        }
    }

    /// <summary>
    /// Id del item cuyo texto es exactamente el indicado, o null si no hay
    /// ninguno.
    /// </summary>
    private static int? IdDeTexto<T>(ComboBox combo, string texto) where T : class
    {
        foreach (object? item in combo.Items)
        {
            if (item is Envoltura<T> env && env.Texto == texto)
                return env.Id;
        }

        return null;
    }

    /// <summary>
    /// Deja elegido el item con ese Id, si esta en la lista.
    /// </summary>
    private static void SeleccionarPorId<T>(ComboBox combo, int id) where T : class
    {
        for (int i = 0; i < combo.Items.Count; i++)
        {
            if (combo.Items[i] is Envoltura<T> env && env.Id == id)
            {
                combo.SelectedIndex = i;
                return;
            }
        }
    }

    /// <summary>
    /// Posicion del caret del combo, o <paramref name="porDefecto"/> si el
    /// combo todavia no tiene ventana.
    /// </summary>
    private static int CaretActual(ComboBox combo, int porDefecto)
    {
        try
        {
            int caret = combo.SelectionStart;
            return caret < 0 ? porDefecto : caret;
        }
        catch (InvalidOperationException)
        {
            // El combo todavia no creo su ventana: no hay caret todavia.
            return porDefecto;
        }
    }

    /// <summary>
    /// Saca la envoltura del item del combo y devuelve el valor original.
    /// </summary>
    private static T? Desenvolver<T>(object? item) where T : class
    {
        return item is Envoltura<T> env ? env.Valor : null;
    }

    private static void AplicarEstilo(ComboBox combo)
    {
        // Lista desplegable sin edicion: el usuario elige, no escribe.
        combo.DropDownStyle = ComboBoxStyle.DropDownList;

        // La misma fuente que usa el TextControl, para que el combo se vea
        // igual que los campos de texto.
        combo.Font = ComboFont;
        combo.BackColor = Fondo;
        combo.ForeColor = TextoPrimario;
    }
}
