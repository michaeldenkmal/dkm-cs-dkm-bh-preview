using System;
using System.ComponentModel;
using System.Windows.Forms;

public class NullableDateTimePicker : DateTimePicker
{
    private bool _isNull = true;
    private string _nonNullFormat = "dd.MM.yyyy";

    public NullableDateTimePicker()
    {
        // Wir arbeiten immer mit CustomFormat, steuern Anzeige selbst
        Format = DateTimePickerFormat.Custom;
        CustomFormat = " "; // leer anzeigen = "null"
    }

    /// <summary>
    /// Nullable-Wert des DatePickers. null = kein Datum gesetzt.
    /// </summary>
    [Browsable(true)]
    [Bindable(true)]
    public new DateTime? Value
    {
        get
        {
            return _isNull ? (DateTime?)null : base.Value;
        }
        set
        {
            if (value.HasValue)
            {
                // von null auf Wert wechseln
                if (_isNull)
                {
                    _isNull = false;
                    CustomFormat = _nonNullFormat;
                }

                // base.Value darf NIE MinValue o.ä. sein, wenn _isNull=false
                base.Value = value.Value;
            }
            else
            {
                // Null setzen
                _isNull = true;
                CustomFormat = " ";
            }
        }
    }

    /// <summary>
    /// Format, das angezeigt wird, wenn ein Datum vorhanden ist.
    /// </summary>
    [Browsable(true)]
    [DefaultValue("dd.MM.yyyy")]
    public string NonNullCustomFormat
    {
        get { return _nonNullFormat; }
        set
        {
            _nonNullFormat = value ?? "dd.MM.yyyy";
            if (!_isNull)
            {
                CustomFormat = _nonNullFormat;
            }
        }
    }

    // Wenn der Benutzer ein Datum auswählt (Dropdown zu)
    protected override void OnCloseUp(EventArgs eventargs)
    {
        if (_isNull)
        {
            _isNull = false;
            CustomFormat = _nonNullFormat;
        }

        base.OnCloseUp(eventargs);
    }

    // Sobald ValueChanged kommt, gehen wir davon aus, dass ein Datum gesetzt ist
    protected override void OnValueChanged(EventArgs eventargs)
    {
        if (_isNull)
        {
            _isNull = false;
            CustomFormat = _nonNullFormat;
        }

        base.OnValueChanged(eventargs);
    }

    // Delete / Backspace = Datum löschen → null
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
        {
            this.Value = null;
            e.Handled = true;
            return;
        }

        base.OnKeyDown(e);
    }
}
