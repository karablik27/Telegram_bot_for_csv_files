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

namespace Var8.TG_Bot_Help
{
    /// <summary>
    /// Represents a command to initiate selection process.
    /// </summary>
    public class Selection_Help
    {
        private readonly User_Main _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="Selection_Help"/> class.
        /// </summary>
        /// <param name="user">The user manager.</param>
        public Selection_Help(User_Main user)
        {
            _user = user;
        }

        /// <summary>
        /// Executes the selection help command by setting the user's state to awaiting selection and sending a message with options for selection.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message object representing the command request.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            _user.SetState(message.From.Id, "AwaitingSelection");

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "/Type", "/RegistrationDate" },
                new KeyboardButton[] { "/RegistrationDateAndCertificateHolderName" },
                new KeyboardButton[] { "/help" },
                new KeyboardButton[] { "/start" },
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Выберете поле для выборки", replyMarkup: replyKeyboardMarkup, cancellationToken: token);
        }
    }
}
