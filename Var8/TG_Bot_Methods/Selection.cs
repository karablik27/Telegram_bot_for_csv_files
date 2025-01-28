using Geraldic_Signs_Library;
using Geraldic_Signs_Library.Processing;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Var8.TG_Bot_Help;
using Var8.TG_Bot_Main;
using static Var8.TG_Bot_Main.File_Main;

namespace Var8.TG_Bot_Methods
{
    /// <summary>
    /// Handles the selection of Geraldic Signs based on user input.
    /// </summary>
    public class Selection
    {
        private readonly User_Main _user;
        private readonly File_Main _file;
        private readonly string _field;
        private readonly string[] _val;

        /// <summary>
        /// Initializes a new instance of the <see cref="Selection"/> class.
        /// </summary>
        /// <param name="user">The user manager instance.</param>
        /// <param name="file">The file manager instance.</param>
        /// <param name="field">The field to perform the selection on.</param>
        /// <param name="val">The values to filter the selection by.</param>
        public Selection(User_Main user, File_Main file, string field, string[] val)
        {
            _user = user;
            _file = file;
            _field = field;
            _val = val;
        }

        /// <summary>
        /// Executes the selection process based on user input.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message received from the user.</param>
        /// <param name="token">The cancellation token.</param>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            Stream stream = _user.GetFile(message.From.Id);

            if (stream == null)
            {
                await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Файл не найден", cancellationToken: token);
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

                string[] value = message.Text.Trim().Split(';');

                List<Geraldic_Signs> selected = SelectGeraldicSigns(lib, value);

                var key = CreateReplyKeyboardMarkup();

                if (!selected.Any())
                {
                    await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Результат выборки пуст.", replyMarkup: key, cancellationToken: token);
                }
                else
                {
                    _user.SetState(message.From.Id, "ProcessingCompleted");

                    await bot.SendTextMessageAsync(
                         chatId: message.Chat.Id,
                         text: "Выборка произведена",
                         cancellationToken: token);

                    await bot.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Для загрузки нового файла используйте /download",
                        replyMarkup: key,
                        cancellationToken: token);

                }

                _user.SetGeraldicSigns(message.From.Id, selected);
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
        /// Selects Geraldic Signs from the provided list based on the specified field and values.
        /// </summary>
        /// <param name="четонепонятное">The list of Geraldic Signs to select from.</param>
        /// <param name="val">The values to filter the Geraldic Signs.</param>
        /// <returns>A list of selected Geraldic Signs.</returns>
        private List<Geraldic_Signs> SelectGeraldicSigns(List<Geraldic_Signs> четонепонятное, string[] val)
        {
            List<Geraldic_Signs> selected = new List<Geraldic_Signs>();

            switch (_field)
            {
                case "Type":
                    if (val.Length != 1) break;
                    selected = четонепонятное.Where(sign => sign.Type.Equals(val[0], StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "RegistrationDate":
                    if (val.Length != 1) break;
                    DateTime inputDate;
                    if (DateTime.TryParseExact(val[0], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out inputDate))
                    {
                        selected = четонепонятное.Where(sign => DateTime.TryParseExact(sign.RegistrationDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime signDate) && signDate == inputDate).ToList();
                    }
                    break;
                case "RegistrationDateAndCertificateHolderName":
                    if (val.Length != 2) break;
                    selected = четонепонятное.Where(lib => lib.RegistrationDate.Equals(_val[0], StringComparison.OrdinalIgnoreCase) && lib.CertificateHolderName.Equals(_val[1], StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    Console.WriteLine("Ошибка выбора поля.");
                    break;
            }

            return selected;
        }

        /// <summary>
        /// Creates a reply keyboard markup with predefined options.
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
