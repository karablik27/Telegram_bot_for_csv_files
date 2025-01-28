using Geraldic_Signs_Library.Processing;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Var8.TG_Bot_Help;
using Var8.TG_Bot_Main;
using Geraldic_Signs_Library;

namespace Var8.TG_Bot_Methods
{
    /// <summary>
    /// Handles the downloading of files based on user input.
    /// </summary>
    public class Download
    {
        private readonly User_Main _user;
        private readonly File_Main _file;
        private string _ex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Download"/> class.
        /// </summary>
        /// <param name="user">The user manager instance.</param>
        /// <param name="file">The file manager instance.</param>
        /// <param name="ex">The file extension (CSV or JSON).</param>
        public Download(User_Main user, File_Main file, string ex)
        {
            _user = user;
            _file = file;
            _ex = ex;
        }

        /// <summary>
        /// Executes the downloading process based on user input.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message received from the user.</param>
        /// <param name="token">The cancellation token.</param>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            List<Geraldic_Signs> lib = _user.GetLibraries(message.From.Id);

            if (lib == null)
            {
                await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Коллекция не существует.", cancellationToken: token);
                return;
            }

            await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Подготовка файла", cancellationToken: token);

            try
            {
                Stream stream = CreateStreamForDownload(lib);

                if (stream == null)
                {
                    Console.WriteLine("Ошибка при создании потока для скачивания.");
                    await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Произошла ошибка при подготовке файла.", cancellationToken: token);
                    return;
                }

                // Ensure the stream position is at the beginning
                stream.Position = 0;

                string name = $"Geraldic_Signs.{_ex}";

                // Sending the file to the user
                await bot.SendDocumentAsync(
                    chatId: message.Chat.Id,
                    document: InputFile.FromStream(stream: stream, fileName: name),
                    caption: "Файл готов к скачиванию",
                    cancellationToken: token
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ошибка при подготовке файла.", cancellationToken: token);
            }

            await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Для перезапуска используйте /start", cancellationToken: token);
        }
        /// <summary>
        /// Creates a stream for downloading the provided list of Geraldic_Signs in the specified format.
        /// </summary>
        /// <param name="lib">The list of Geraldic_Signs to be written to the stream.</param>
        /// <returns>A stream containing the data in the specified format.</returns>
        private Stream CreateStreamForDownload(List<Geraldic_Signs> lib)
        {
            Stream stream = null;

            if (_ex == "CSV")
            {
                CSVProcessing csv = new CSVProcessing();
                stream = csv.Write(lib);
            }
            else if (_ex == "JSON")
            {
                JSONProcessing json = new JSONProcessing();
                stream = json.Write(lib);
            }
            else
            {
                Console.WriteLine("Некорректное расширение файла.");
            }

            return stream;
        }
    }
}
