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
    /// Represents a command to initiate sorting process.
    /// </summary>
    public class Sort_Help
    {
        private User_Main _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sort_Help"/> class.
        /// </summary>
        /// <param name="user">The user manager.</param>
        public Sort_Help(User_Main user)
        {
            _user = user;
        }

        /// <summary>
        /// Executes the sort help command by sending a message with options for sorting.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message object representing the command request.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "/RegistrationNumberAscending" },
                new KeyboardButton[] { "/RegistrationNumberDescending" },
                new KeyboardButton[] { "/help" },
                new KeyboardButton[] { "/start" },
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            await bot.SendTextMessageAsync(message.Chat.Id, "Выберите тип сортировки:", replyMarkup: replyKeyboardMarkup, cancellationToken: token);
        }
    }
}