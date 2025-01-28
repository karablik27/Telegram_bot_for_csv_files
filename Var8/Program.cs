using Geraldic_Signs_Library.Processing;
using Geraldic_Signs_Library;

namespace Var8
{
    /// <summary>
    /// Represents the entry point of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            TelegramBotRefactored telegramBot = new TelegramBotRefactored("6918545895:AAEgV4NjdLC3pz1o-tjcva9TPVy0j4TdWrA");
            await telegramBot.StartReceiving();
        }
    }
}
