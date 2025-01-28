using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Var8.TG_Bot_Main.User_Main;
using Telegram.Bot.Types;
using Telegram.Bot;
using Var8.TG_Bot_Main;

namespace Var8.TG_Bot_Help
{
    /// <summary>
    /// Represents a command to handle selection based on a specific field.
    /// </summary>
    public class Select_Help
    {
        private readonly User_Main _user;
        private readonly string _field;

        /// <summary>
        /// Initializes a new instance of the <see cref="Select_Help"/> class.
        /// </summary>
        /// <param name="user">The user manager.</param>
        /// <param name="field">The field to perform selection on.</param>
        public Select_Help(User_Main user, string field)
        {
            _user = user;
            _field = field;
        }

        /// <summary>
        /// Executes the select help command by setting the user's state and field for selection and sending a message to prompt user input.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message object representing the command request.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            _user.SetState(message.From.Id, "AwaitingFieldValue");
            _user.SetField(message.From.Id, _field);

            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Введите значение поля {_field} для выборки:",
                cancellationToken: token
            );

            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Если выборка по двум полям, то введите значения полей через точку с запятой без пробелов",
                cancellationToken: token
            );

        }

    }
}
