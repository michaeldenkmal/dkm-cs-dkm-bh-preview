using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

public class CurrencyTextBox : TextBox
{
    private bool _formatting = false;

    [Browsable(true)]
    [Category("Behavior")]
    public CultureInfo Culture { get; set; } = new CultureInfo("de-AT");

    [Browsable(true)]
    [Category("Behavior")]
    public decimal? Value
    {
        get
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                return null;
            }
            decimal.TryParse(this.Text, NumberStyles.Any, Culture, out var val);
            return val;
        }
        set
        {
            if (value.HasValue)
            {
                decimal my_value = value.GetValueOrDefault();
                this.Text = my_value.ToString("N2", Culture);
            } else
            {
                this.Text = "";
            }
        }
    }

    public CurrencyTextBox()
    {
        this.TextAlign = HorizontalAlignment.Right;

        this.KeyPress += CurrencyTextBox_KeyPress;
        this.Leave += CurrencyTextBox_Leave;
    }

    private void CurrencyTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Steuerzeichen (Backspace, Tab …) erlauben
        if (char.IsControl(e.KeyChar))
            return;

        // Dezimal-Trennzeichen erlauben
        string decimalSep = Culture.NumberFormat.NumberDecimalSeparator;
        if (e.KeyChar.ToString() == decimalSep)
        {
            // nur 1x erlauben
            if (this.Text.Contains(decimalSep))
                e.Handled = true;
            return;
        }

        // Zahlen erlauben
        if (char.IsDigit(e.KeyChar))
            return;

        // Alles andere blockieren
        e.Handled = true;
    }

    private void CurrencyTextBox_Leave(object sender, EventArgs e)
    {
        FormatText();
    }

    private void FormatText()
    {
        if (_formatting) return;
        _formatting = true;

        if (decimal.TryParse(Text, NumberStyles.Any, Culture, out var v))
            Text = v.ToString("N2", Culture);

        _formatting = false;
    }
}
