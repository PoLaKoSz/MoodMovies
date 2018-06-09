using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MoodMovies.Resources.Validation
{
    public class SearchBoxValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string searchText = value as string;
            searchText.Trim();

            if (!Regex.IsMatch(searchText, @"^[a-zA-Z0-9\s]+$") || string.IsNullOrEmpty(searchText))
            {
                return new ValidationResult(false, "Alpha-numeric characters only. ");
            }
            else if( searchText.Length > 50 )
            {
                return new ValidationResult(false, "Maximum 50 characters");
            }            
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
