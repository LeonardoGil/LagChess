using LagChessForm.Themes;

namespace LagChessForm
{
    internal static class Program
    {
        internal static Theme Theme = new();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new ChessForm());
        }
    }
}