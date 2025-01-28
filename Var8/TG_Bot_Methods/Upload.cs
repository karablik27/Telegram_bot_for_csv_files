using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Var8.TG_Bot_Main.User_Main;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Var8.TG_Bot_Main;
using static Var8.TG_Bot_Main.File_Main;

namespace Var8.TG_Bot_Methods
{
    public class Upload
    {
        readonly User_Main _user;
        readonly File_Main _file;

        /// <summary>
        /// Initializes a new instance of the <see cref="Upload"/> class.
        /// </summary>
        /// <param name="user">The user manager.</param>
        /// <param name="file">The file manager.</param>
        public Upload(User_Main user, File_Main file)
        {
            _user = user;
            _file = file;
        }

        /// <summary>
        /// Handles the uploaded file by checking its format and saving it to the user state manager.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The uploaded message.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task HandleFileAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            if (message.Document == null) return;

            var name = message.Document.FileName.ToLower();

            if (!name.EndsWith(".csv") && !name.EndsWith(".json"))
            {
                await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Загрузите файл в формате CSV или JSON.", cancellationToken: token);
                return;
            }

            var tg = await bot.GetFileAsync(message.Document.FileId, token);

            Stream fileStream = new MemoryStream();
            try
            {
                await DownloadFileAndSetPosition(bot, tg, fileStream, token);
            }
            catch (Exception ex)
            {
                await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Произошла ошибка при обработке файла.", cancellationToken: token);
                return;
            }

            _user.SetFile(message.From.Id, fileStream);

            SetFileType(name, message.From.Id);

            _user.SetState(message.From.Id, "FileUploaded");

            var key = CreateReplyKeyboardMarkup();

            await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Файл успешно загружен, выберите следующее действие.", replyMarkup: key, cancellationToken: token);
        }

        /// <summary>
        /// Downloads the file from Telegram and sets the position of the file stream to the beginning.
        /// </summary>
        /// <param name="botClient">The Telegram bot client.</param>
        /// <param name="telegramFile">The Telegram file to download.</param>
        /// <param name="fileStream">The file stream to write the downloaded file.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task DownloadFileAndSetPosition(ITelegramBotClient botClient, Telegram.Bot.Types.File telegramFile, Stream fileStream, CancellationToken cancellationToken)
        {
            await botClient.DownloadFileAsync(telegramFile.FilePath, fileStream, cancellationToken);
            fileStream.Position = 0; 

            // Проверяем, что поток не пустой
            if (fileStream.Length == 0)
            {
                throw new Exception("Файл пуст или произошла ошибка при скачивании.");
            }
        }
        /// <summary>
        /// Sets the type of the uploaded file based on its extension.
        /// </summary>
        /// <param name="fileName">The name of the uploaded file.</param>
        /// <param name="userId">The ID of the user who uploaded the file.</param>
        private void SetFileType(string fileName, long userId)
        {
            if (fileName.EndsWith(".csv"))
            {
                _file.SetType(userId, FileType.CSV);
            }
            else
            {
                _file.SetType(userId, FileType.JSON);
            }
        }

        /// <summary>
        /// Creates a reply keyboard markup for the Telegram bot.
        /// </summary>
        /// <returns>The created reply keyboard markup.</returns>
        private ReplyKeyboardMarkup CreateReplyKeyboardMarkup()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "/select", "/sort" },
                new KeyboardButton[] { "/download" },
                new KeyboardButton[] { "/help" },
                new KeyboardButton[] { "/start" },
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
    }
}
