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
    /// Represents a command to handle file uploads.
    /// </summary>
    public class File_Help
    {
        private readonly User_Main _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="File_Help"/> class.
        /// </summary>
        /// <param name="user">The user manager.</param>
        public File_Help(User_Main user)
        {
            _user = user;
        }

        /// <summary>
        /// Executes the file help command by setting the user's state to awaiting file upload and sending a message to prompt file upload.
        /// </summary>
        /// <param name="bot">The Telegram bot client.</param>
        /// <param name="message">The message object representing the command request.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            // Logic to check and retrieve the file from the message and process it

            _user.SetState(message.From.Id, "AwaitingFileUpload");

            await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Загрузите CSV или JSON файл.", cancellationToken: token);
        }
    }
}
