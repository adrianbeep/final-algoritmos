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

    // ============================================================
    // COMBO BUSCABLE
    // ============================================================

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
    /// Envuelve el item original para poder mostrar otro texto en el combo.
    /// El combo guarda objetos, asi que el item va envuelto; el valor
    /// original se recupera con <see cref="BuscableSeleccionado{T}"/>.
    /// </summary>
    private sealed class Envoltura<T> where T : class
    {
        public T Valor { get; }
        public string Texto { get; }

        public Envoltura(T valor, string texto)
        {
            Valor = valor;
            Texto = texto;
        }

        public override string ToString() => Texto;
    }

    /// <summary>
    /// Configura un combo BUSCABLE: el usuario escribe y la lista se filtra
    /// con <paramref name="coincide"/>.
    ///
    /// El autocompletado nativo de WinForms queda desactivado a proposito:
    /// el nativo busca con StartsWith y el filtro es propio.
    ///
    /// Cada item se muestra con <paramref name="textoMostrado"/>; el valor
    /// original (no el envuelto) se recupera con
    /// <see cref="BuscableSeleccionado{T}"/>.
    ///
    /// La lista arranca COMPLETA: el filtro no corre sobre el texto
    /// inicial.
    ///
    /// Al escribir, el texto del combo NO se pisa: se filtra la lista y se
    /// restaura lo escrito. La seleccion previa se mantiene solo si el item
    /// sigue en la lista y el texto escrito es el texto del item, o sea
    /// cuando el usuario eligio un item de la lista desplegada. Si esta
    /// escribiendo a medio camino, la seleccion se suelta y queda el texto.
    /// </summary>
    public static void ConfigBuscable<T>(
        ComboBox combo,
        List<T> items,
        Func<T, string> textoMostrado,
        Func<T, string, bool> coincide,
        T? seleccionado = null) where T : class
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

            // El texto se copia antes de tocar la lista: al vaciarla, el
            // texto escrito y el caret se pierden.
            string texto = combo.Text;

            // Si el texto escrito es exactamente el texto de un item que ya
            // esta en la lista, no hay nada que filtrar: es el usuario
            // eligiendo uno de la lista desplegada. Si se reconstruyera la
            // lista en ese momento, se perderia la seleccion.
            if (IndicePorTexto<T>(combo, texto) >= 0) return;

            int caret = CaretActual(combo, texto.Length);

            reconstruyendo = true;
            try
            {
                Poblar(combo, items, textoMostrado, item => coincide(item, texto));

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
                int indice = IndicePorTexto<T>(combo, texto);
                if (indice >= 0)
                {
                    combo.SelectedIndex = indice;
                }
            }
            finally
            {
                reconstruyendo = false;
            }
        };

        // La lista completa se carga ANTES de enganchar el filtro: si el
        // filtro ya estuviera enganchado, la seleccion inicial dispararia
        // el TextChanged y dejaria la lista reducida a un solo item.
        Poblar(combo, items, textoMostrado, _ => true);

        if (seleccionado is not null)
        {
            foreach (object? item in combo.Items)
            {
                if (item is Envoltura<T> env
                    && EqualityComparer<T>.Default.Equals(env.Valor, seleccionado))
                {
                    combo.SelectedItem = env;
                    break;
                }
            }
        }

        estado.Handler = filtrar;
        combo.TextChanged += filtrar;
    }

    /// <summary>
    /// Item original seleccionado en un combo buscable, o null si no hay
    /// nada seleccionado. Devuelve el valor original, no el envuelto.
    /// </summary>
    public static T? BuscableSeleccionado<T>(ComboBox combo) where T : class
    {
        return Desenvolver<T>(combo.SelectedItem);
    }

    /// <summary>
    /// Recarga la lista del combo con los items que pasan el filtro.
    /// </summary>
    private static void Poblar<T>(
        ComboBox combo,
        List<T> items,
        Func<T, string> textoMostrado,
        Func<T, bool> incluir) where T : class
    {
        combo.Items.Clear();

        foreach (T item in items)
        {
            if (!incluir(item)) continue;

            combo.Items.Add(new Envoltura<T>(item, textoMostrado(item)));
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

    /// <summary>
    /// Indice del item cuyo texto es exactamente el indicado, o -1 si no
    /// hay ninguno.
    /// </summary>
    private static int IndicePorTexto<T>(ComboBox combo, string texto) where T : class
    {
        for (int i = 0; i < combo.Items.Count; i++)
        {
            if (combo.Items[i] is Envoltura<T> env && env.Texto == texto)
            {
                return i;
            }
        }
        return -1;
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
