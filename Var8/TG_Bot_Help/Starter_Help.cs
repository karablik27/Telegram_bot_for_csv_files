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
    /// Represents a command to handle bot startup or restart.
    /// </summary>
    public class Starter_Help
    {
        private readonly User_Main _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="Starter_Help"/> class.
        /// </summary>
        /// <param name="user">The user manager.</param>
        public Starter_Help(User_Main user)
        {
            _user = user;
        }

        /// <summary>
        /// Executes the starter help command by resetting the user's state and sending a welcome message with available commands.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message object representing the command request.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            _user.SetState(message.From.Id, "Normal");

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "/file" },
                new KeyboardButton[] { "/help" },
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Данный бот предназначен для работы с CSV и Json файлами geraldic_signs. Загрузите файл /file или узнаете список команд -  /help.",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: token
            );
        }
    }

}
