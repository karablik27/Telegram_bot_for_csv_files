using Geraldic_Signs_Library.Processing;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Var8.TG_Bot_Main.User_Main;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Var8.TG_Bot_Main;
using static Var8.TG_Bot_Main.File_Main;
using Var8.TG_Bot_Help;
using Geraldic_Signs_Library;

namespace Var8.TG_Bot_Methods
{
    /// <summary>
    /// Handles the sorting of Geraldic Signs based on a specified field.
    /// </summary>
    public class Sorting
    {
        readonly User_Main _user;
        readonly File_Main _file;
        readonly string _field;
        readonly bool _isAscending;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sorting"/> class.
        /// </summary>
        /// <param name="user">The user manager instance.</param>
        /// <param name="file">The file manager instance.</param>
        /// <param name="field">The field based on which the sorting is performed.</param>
        /// <param name="isAscending">Specifies whether the sorting should be performed in ascending order.</param>
        public Sorting(User_Main user, File_Main file, string field, bool isAscending)
        {
            _user = user;
            _file = file;
            _field = field;
            _isAscending = isAscending;
        }

        /// <summary>
        /// Executes the sorting process based on user input.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message received from the user.</param>
        /// <param name="token">The cancellation token.</param>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            Stream stream = _user.GetFile(message.From.Id);

            if (stream == null)
            {
                await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Файл не найден. Пожалуйста, загрузите файл.", cancellationToken: token);
                return;
            }

            try
            {
                List<Geraldic_Signs> lib;

                if (_file.GetType(message.From.Id) == FileType.CSV)
                {
                    CSVProcessing csv = new CSVProcessing();
                    lib = csv.Read(stream);
                }
                else
                {
                    JSONProcessing json = new JSONProcessing();
                    lib = json.Read(stream);
                }

                List<Geraldic_Signs> sorted = SortLibraries(lib);

                var key = CreateReplyKeyboardMarkup();

                if (sorted == null || !sorted.Any())
                {
                    await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ваш запрос не дал никаких результатов.", replyMarkup: key, cancellationToken: token);
                }
                else
                {
                    var responseText = string.Join("\n", sorted.Select(lib => lib.ToString()));

                    _user.SetState(message.From.Id, "ProcessingCompleted");

                    await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Сортировка произведена успешно!\nДля загрузки файла используйте команду /download", replyMarkup: key, cancellationToken: token);
                }

                _user.SetGeraldicSigns(message.From.Id, sorted ?? new List<Geraldic_Signs>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: ex.Message,
                    cancellationToken: token
                );

                await bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Используйте /start для перезапуска бота.",
                    cancellationToken: token
                );

            }

            _user.SetState(message.From.Id, "ProcessingCompleted");
        }
        /// <summary>
        /// Sorts the list of Geraldic Signs based on the registration number field.
        /// </summary>
        /// <param name="lib">The list of Geraldic Signs to be sorted.</param>
        /// <returns>The sorted list of Geraldic Signs.</returns>
        private List<Geraldic_Signs> SortLibraries(List<Geraldic_Signs> lib)
        {
            if (_field != "RegistrationNumber")
            {
                Console.WriteLine("Ошибка выбора поля.");
                return null;
            }

            return _isAscending
                ? lib
                    .OrderBy(lib =>
                    {
                        var a = lib.RegistrationNumber.Split('/');
                        return int.TryParse(a.Length > 1 ? a[1] : "0", out int result) ? result : int.MaxValue;
                    })
                    .ThenBy(lib =>
                    {
                        var b = lib.RegistrationNumber.Split('/');
                        return int.TryParse(b.Length > 0 ? b[0].Replace("МС №", "") : "0", out int result) ? result : int.MaxValue;
                    })
                    .ToList()
                : lib
                    .OrderByDescending(lib =>
                    {
                        var c = lib.RegistrationNumber.Split('/');
                        return int.TryParse(c.Length > 1 ? c[1] : "0", out int result) ? result : int.MaxValue;
                    })
                    .ThenByDescending(lib =>
                    {
                        var x = lib.RegistrationNumber.Split('/');
                        return int.TryParse(x.Length > 0 ? x[0].Replace("МС №", "") : "0", out int result) ? result : int.MaxValue;
                    })
                    .ToList();
        }
        /// <summary>
        /// Creates a reply keyboard markup for the Telegram bot.
        /// </summary>
        /// <returns>The created reply keyboard markup.</returns>
        private ReplyKeyboardMarkup CreateReplyKeyboardMarkup()
        {
            return new ReplyKeyboardMarkup(new[]
            {
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
