using System.Drawing;

namespace Common
{
    public class Utils
    {
        public static string findColorCode(string colorName)
        {
            /* string hexColorCode = "";
             Color color = Color.FromName(colorName);
             if (color.IsKnownColor)
             {
                 hexColorCode = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
             } else
                 hexColorCode = "NaN";
             return hexColorCode;*/

            var customColors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
             {
                {"Blue", "rgb(0, 116, 217)" },
                {"Pink", "rgb(241, 169, 196)" },
                {"Green","rgb(94, 177, 96)" },
                {"Black","rgb(54, 69, 79)" },
                {"Yellow","rgb(234, 220, 50)" },
                {"White","rgb(255, 255, 255)" },
                {"Red","rgb(211, 75, 86)" },
                {"Purple","rgb(128, 0, 128)" },
                {"Maroon","color: rgb(176, 48, 96)" },
                {"Navy Blue","rgb(60, 68, 119)" },
                {"Grey","rgb(159, 168, 171)" },
                {"Mustard","rgb(204, 156, 51)" },
                {"Peach","rgb(255, 229, 180)" },
                {"Beige","rgb(232, 230, 207)" },
                {"Off White","rgb(242, 242, 242)" },
                {"Teal","rgb(0, 128, 128)" },
                {"Orange","rgb(242, 141, 32)" },
                {"Brown","rgb(145, 80, 57)" },
                {"Cream","rgb(237, 230, 185)" },
                {"Turquoise Blue","rgb(64, 224, 208)" },
                {"Sea green","rgb(46, 139, 87)" },
                {"Lavender","rgb(214, 214, 229)" },
                {"Olive","rgb(61, 153, 112)" },
                {"Burgundy","rgb(160, 50, 69)" },
                {"Magenta","rgb(185, 82, 159)" },
                {"Rust","rgb(183, 65, 14)" },
                {"Mauve","rgb(224, 176, 255)" },
                {"Lime Green","rgb(93, 182, 83)" },
                {"Coral","rgb(255, 127, 80)" },
                {"Violet","rgb(127, 0, 255)" },
                {"Charcoal","rgb(54, 69, 79)" },
                {"Coffee Brown","rgb(75, 48, 47)" },
                {"Gold","rgb(229, 199, 74)" },
                {"Rose","rgb(221, 47, 134)" },
                {"Taupe","rgb(72, 60, 50)" },
                {"Khaki","rgb(195, 176, 145)" },
                {"Grey Melange","rgb(159, 168, 171)" },
                {"Rose Gold","rgb(183, 110, 121)" },
                {"Camel Brown","rgb(165, 102, 57)" },
                {"Fluorescent Green","rgb(141, 192, 74)" },
                {"Tan","rgb(210, 180, 140)" },
                {"Bude","rgb(219, 175, 151)" },
                {"Silver","rgb(179, 179, 179)" },
                {"Bronze","rgb(204, 130, 64)" },
                {"Copper","rgb(170, 108, 57)" },
                {"Steel","rgb(179, 179, 179)" },
                {"Metallic","rgb(224, 208, 197)" },
             };

            if (customColors.TryGetValue(colorName, out string rgbColorCode))
            {
                return rgbColorCode;
            }

            Color color = Color.FromName(colorName);
            if (color.IsKnownColor && !color.IsSystemColor)
            {
                return $"rgb({color.R}, {color.G}, {color.B})";
            }
            return "NaN";
        }
    }
}
