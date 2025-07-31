namespace WebApp.Helpers
{
    public static class General
    {
        public static string GetInitialFromUser(string name)
        {
            return !string.IsNullOrWhiteSpace(name) ? name.Trim()[0].ToString().ToUpper() : "A";
        }


        //TODO: Replace this method with Data that actually comes from the database
        public static string GetPillBadgeColor(string input)
        {
            const string primary = "bg-primary";

            if (string.IsNullOrWhiteSpace(input))
            {
                return primary;
            }

            const string secondary = "bg-secondary";
            const string success = "bg-success";
            const string danger = "bg-danger";
            const string warning = "bg-warning";
            const string info = "bg-info";
            const string light = "bg-light";
            const string dark = "bg-dark";

            // List of available badge classes
            var badgeClasses = new[] { primary, secondary, success, danger, warning, info, light, dark };



            // Use first and last character, uppercase for consistency
            char first = char.ToUpper(input.Trim()[0]);
            char last = char.ToUpper(input.Trim()[input.Trim().Length - 1]);

            // Deterministically combine first and last char codes
            int hash = (first * 31 + last) % badgeClasses.Length;
            if (hash < 0) hash = -hash; // Ensure non-negative

            return badgeClasses[hash];
        }

        public static string ToFriendlyDate(DateTime dateTime)
        {
            var now = DateTime.Now;
            var ts = now - dateTime;

            if (ts.TotalSeconds < 60)
                return "A moment ago";
            if (ts.TotalMinutes < 60)
                return $"{(int)ts.TotalMinutes} minute{(ts.TotalMinutes < 2 ? "" : "s")} ago";
            if (ts.TotalHours < 24)
                return $"{(int)ts.TotalHours} hour{(ts.TotalHours < 2 ? "" : "s")} ago";
            if (ts.TotalDays < 2)
                return "Yesterday";
            if (ts.TotalDays < 7)
                return $"{(int)ts.TotalDays} day{(ts.TotalDays < 2 ? "" : "s")} ago";
            if (ts.TotalDays < 14)
                return "Last week";
            if (dateTime.Year == now.Year)
                return dateTime.ToString("MMM dd");
            return dateTime.ToString("MMM dd, yyyy");
        }
    }
}
