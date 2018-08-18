using MoodMovies.DataAccessLayer;

namespace MoodMovies.Resources.Validation
{
    public static class CredentialValidation
    {
        /// <summary>
        /// Determinate the supplied string can be a valid Email address
        /// </summary>
        /// <returns><c>TRUE</c> if can be,
        /// <c>FALSE</c> otherwise</returns>
        public static bool IsValidEmail(string emailAddress)
        {
            if (emailAddress.Length < 5)
                return false;

            if (!emailAddress.Contains("@"))
                return false;

            if (!emailAddress.Contains("."))
                return false;

            return true;
        }

        /// <summary>
        /// Determinate the supplied string can be a valid password
        /// </summary>
        /// <returns><c>TRUE</c> if can be,
        /// <c>FALSE</c> otherwise</returns>
        public static bool IsValidPassword(string password)
        {
            if (password.Length < 5)
                return false;

            return true;
        }

        /// <summary>
        /// Check if the supplied string is a valid first name
        /// </summary>
        /// <returns><c>TRUE</c> if it's valid,
        /// <c>FALSE</c> otherwise</returns>
        public static bool IsValidFirstName(string firstName)
        {
            if (firstName.Length < 2)
                return false;

            return true;
        }

        /// <summary>
        /// Check if the supplied string is a valid surname
        /// </summary>
        /// <returns><c>TRUE</c> if it's valid,
        /// <c>FALSE</c> otherwise</returns>
        public static bool IsValidSurName(string surName)
        {
            return IsValidFirstName(surName);
        }

        /// <summary>
        /// Check if the supplied string is a valid TMDb API key
        /// </summary>
        /// <returns><c>TRUE</c> if it's valid,
        /// <c>FALSE</c> otherwise</returns>
        public static bool IsValidApiKey(string apiKey, IOnlineServiceProvider tmdbClient)
        {
            if (apiKey.Length == 0)
                return false;

            try
            {
                tmdbClient.ChangeClient(apiKey);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}