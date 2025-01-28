using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Var8.TG_Bot_Help
{
    /// <summary>
    /// Class to handle the execution of the /help command, providing a list of available commands.
    /// </summary>
    public class Command_Help
    {
        /// <summary>
        /// Executes the /help command and sends a message containing the list of available commands.
        /// </summary>
        /// <param name="bot">The Telegram Bot Client.</param>
        /// <param name="message">The incoming message.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ExecuteAsync(ITelegramBotClient bot, Message message, CancellationToken token)
        {
            string startCommandHelp = "/start - начало/перезапуск бота";
            string helpCommandHelp = "/help - узнать список команд";
            string uploadFileCommandHelp = "/file - загрузить CSV или JSON файл, соответствующий варианту";
            string selectCommandHelp = "/select - сортировка по полю в файле";
            string sortCommandHelp = "/sort - выборка по полю в файле";
            string downloadCommandHelp = "/download - скачать обработанный файл";

            string text = $"В данном боте доступны следующие команды:\n{startCommandHelp}\n{helpCommandHelp}\n{uploadFileCommandHelp}\n{selectCommandHelp}\n{sortCommandHelp}\n{downloadCommandHelp}";

            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: token
            );
        }
    }
}