using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Var8.TG_Bot_Help;
using Var8.TG_Bot_Main;
using Var8.TG_Bot_Methods;

namespace Var8
{
    /// <summary>
    /// Represents a refactored version of the Telegram bot implementation.
    /// </summary>
    public class TelegramBotRefactored
    {
        readonly TelegramBotClient _bot;
        readonly Dictionary<string, Object> _dic;
        readonly User_Main _user;
        readonly File_Main _file;
        readonly CancellationTokenSource _cancel;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramBotRefactored"/> class.
        /// </summary>
        /// <param name="token">The bot token.</param>
        public TelegramBotRefactored(string token)
        {
            _bot = new TelegramBotClient(token);
            _dic = new Dictionary<string, Object>();
            _user = new User_Main();
            _file = new File_Main();
            _cancel = new CancellationTokenSource();
            InitializeCommandHandlers();
        }
        /// <summary>
        /// Starts receiving updates from the Telegram bot.
        /// </summary>
        public async Task StartReceiving()
        {
            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // Receive all update types except ChatMember related updates
            };

            _bot.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                _cancel.Token
            );

            var me = await _bot.GetMeAsync();
            Console.WriteLine($"Бот @{me.Username} активен.");

            await Task.Delay(Timeout.Infinite, _cancel.Token);
        }
        /// <summary>
        /// Handles incoming updates from the Telegram bot.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="update">The incoming update.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
        {
            if (update.Message is not { } message) return;

            if (message.Type == MessageType.Document && _user.GetState(message.From.Id) == "AwaitingFileUpload")
            {
                await new Upload(_user, _file).HandleFileAsync(bot, message, token);
                return;
            }

            if (message.Type != MessageType.Text) return;

            var text = message.Text.Split(';', StringSplitOptions.RemoveEmptyEntries);
            var state = _user.GetState(message.From.Id);
            var varr = _dic.GetValueOrDefault(text[0]);

            if (state == "AwaitingFieldValue")
            {
                var field = _user.GetField(message.From.Id);
                var dataHandler = new Selection(_user, _file, field, text);
                await dataHandler.ExecuteAsync(bot, message, token);
                return;
            }

            if (varr != null && CommandAvailableForState(text[0], state))
            {
                switch (text[0])
                {
                    case "/file":
                        await ((File_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/select":
                        await ((Selection_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/sort":
                        await ((Sort_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/download":
                        await ((Download_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/Type":
                        await ((Select_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/RegistrationDate":
                        await ((Select_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/CertificateHolderName":
                        await ((Select_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/RegistrationDateAndCertificateHolderName":
                        await ((Select_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/RegistrationNumberAscending":
                        await ((Sorting)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/RegistrationNumberDescending":
                        await ((Sorting)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/CSV":
                        await ((Download)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/JSON":
                        await ((Download)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/start":
                        await ((Starter_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                    case "/help":
                        await ((Command_Help)varr).ExecuteAsync(bot, message, token);
                        break;
                }
            }
            else
            {
                await HandleInvalidCommandAsync(bot, message, text, state, token);
            }
        }

        /// <summary>
        /// Handles invalid commands received by the Telegram bot.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message containing the invalid command.</param>
        /// <param name="text">The array containing the command text.</param>
        /// <param name="state">The current state of the user.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task HandleInvalidCommandAsync(ITelegramBotClient bot, Message message, string[] text, string state, CancellationToken token)
        {
            if (state == "FileUploaded" && text[0] == "/file")
            {
                await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Вы уже загрузили файл.", cancellationToken: token);
            }
            else if ((state == "Normal" || state == "AwaitingFileUpload") && (text[0] == "/select" || text[0] == "/sort" || text[0] == "/download"))
            {
                await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Сначала загрузите файл, соответствующий варианту.", cancellationToken: token);
            }
            else if (text[0] == "/RegistrationNumberAscending" || text[0] == "/RegistrationNumberDescending")
            {
                var sortingHandler = new Sorting(_user, _file, "RegistrationNumber", text[0] == "/RegistrationNumberAscending");
                await sortingHandler.ExecuteAsync(bot, message, token);
            }
            else
            {
                await SendUnsupportedMessageAsync(bot, message.Chat.Id, token);
            }
        }

        /// <summary>
        /// Sends an unsupported message error to the chat.
        /// </summary>
        /// <param name="botClient">The Telegram bot client.</param>
        /// <param name="chatId">The chat ID to which the message should be sent.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SendUnsupportedMessageAsync(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Ошибка команды. Нажмите /help.",
                cancellationToken: cancellationToken
            );
        }

        /// <summary>
        /// Checks if a command is available for the given state.
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <param name="state">The state for which the command availability should be checked.</param>
        /// <returns>True if the command is available for the given state; otherwise, false.</returns>
        private bool CommandAvailableForState(string command, string state)
        {
            return state switch
            {
                "Normal" => command == "/start" || command == "/help" || command == "/file",
                "FileUploaded" => command == "/select" || command == "/sort" || command == "/download" || command == "/help" || command == "/start",
                "AwaitingSelection" => command == "/Type" || command == "/RegistrationDate" || command == "/CertificateHolderName" || command == "/RegistrationDateAndCertificateHolderName" || command == "/help" || command == "/start",
                "AwaitingSorting" => command == "/RegistrationNumberAscending" || command == "/RegistrationNumberDescending" || command == "/help" || command == "/start",
                "ProcessingCompleted" => command == "/download" || command == "/help" || command == "/start",
                "AwaitingDownloading" => command == "/CSV" || command == "/JSON" || command == "/help" || command == "/start",
                _ => command == "/help" || command == "/start",
            };
        }

        /// <summary>
        /// Initializes command handlers for the bot.
        /// </summary>
        private void InitializeCommandHandlers()
        {
            _dic["/file"] = new File_Help(_user);
            _dic["/select"] = new Selection_Help(_user);
            _dic["/sort"] = new Sort_Help(_user);
            _dic["/download"] = new Download_Help(_user);
            _dic["/Type"] = new Select_Help(_user, "Type");
            _dic["/RegistrationDate"] = new Select_Help(_user, "RegistrationDate");
            _dic["/CertificateHolderName"] = new Select_Help(_user, "CertificateHolderName");
            _dic["/RegistrationDateAndCertificateHolderName"] = new Select_Help(_user, "RegistrationDateAndCertificateHolderName");
            _dic["/RegistrationNumberAscending"] = new Sorting(_user, _file, "RegistrationNumber", true);
            _dic["/RegistrationNumberDescending"] = new Sorting(_user, _file, "RegistrationNumber", false);
            _dic["/CSV"] = new Download(_user, _file, "CSV");
            _dic["/JSON"] = new Download(_user, _file, "JSON");
            _dic["/start"] = new Starter_Help(_user);
            _dic["/help"] = new Command_Help();
        }

        /// <summary>
        /// Handles polling errors occurred during bot operation.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="ex">The exception occurred.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken token)
        {
            var errorMessage = ex switch
            {
                ApiRequestException api => $"Telegram API Error:\n[{api.ErrorCode}]",
                _ => ex.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

    }
}
