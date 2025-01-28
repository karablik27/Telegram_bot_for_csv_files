using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Var8.TG_Bot_Main;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Var8.TG_Bot_Help
{
    /// <summary>
    /// Represents a helper class for handling download operations in the Telegram bot.
    /// </summary>
    public class Download_Help
    {
        readonly User_Main _userStateManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Download_Help"/> class.
        /// </summary>
        /// <param name="userStateManager">The user state manager instance.</param>
        public Download_Help(User_Main userStateManager)
        {
            _userStateManager = userStateManager;
        }

        /// <summary>
        /// Executes the download helper functionality asynchronously.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message received from the user.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            _userStateManager.SetState(message.From.Id, "AwaitingDownloading");

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "/CSV", "/JSON" },
                new KeyboardButton[] { "/help" },
                new KeyboardButton[] { "/start" },
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Выберете формат файла для загрузки.", replyMarkup: replyKeyboardMarkup, cancellationToken: token);
        }
    }
}
