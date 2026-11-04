using UnityEngine;
using UnityEngine.Localization.Settings;

public static class LocalizedKeyCode
{
    public static string GetLocalizedKeyCode(KeyCode keyCode)
    {
        if (LocalizationSettings.SelectedLocale.Identifier.Code == "ru")
        {
            return keyCode switch
            {
                KeyCode.E => "Ó",
                KeyCode.Q => "É",
                KeyCode.W => "Ö",
                KeyCode.R => "Ê",
                KeyCode.T => "Å",
                KeyCode.Y => "Í",
                KeyCode.U => "Ã",
                KeyCode.I => "Ø",
                KeyCode.O => "Ù",
                KeyCode.P => "Ç",
                KeyCode.A => "Ô",
                KeyCode.S => "Û",
                KeyCode.D => "Â",
                KeyCode.F => "À",
                KeyCode.G => "Ï",
                KeyCode.H => "Ð",
                KeyCode.J => "Î",
                KeyCode.K => "Ë",
                KeyCode.L => "Ä",
                KeyCode.Z => "ß",
                KeyCode.X => "×",
                KeyCode.C => "Ñ",
                KeyCode.V => "Ì",
                KeyCode.B => "È",
                KeyCode.N => "Ò",
                KeyCode.M => "Ü",
                _ => keyCode.ToString()
            };
        }

        return keyCode.ToString();
    }
}